using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inspections.Core;
using Inspections.Core.Domain;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.Interfaces;
using Inspections.Core.QueryModels;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspections.Infrastructure.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly InspectionsContext _context;

        private readonly ILogger<ReportsRepository> _logger;
        private readonly IUserNameResolver _userNameResolver;

        private readonly MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Signature, SignatureQueryResult>();
            cfg.CreateMap<Note, NoteQueryResult>();
            cfg.CreateMap<CheckListItem, CheckListItemQueryResult>();
            cfg.CreateMap<CheckList, CheckListQueryResult>().ForMember(m => m.Checks, opt => opt.MapFrom(src => src.Checks));
            cfg.CreateMap<Report, ReportQueryResult>()
                .ForMember(m => m.Signatures, opt => opt.MapFrom(src => src.Signatures.OrderBy(ob => ob.Order)))
                .ForMember(m => m.CheckLists, opt => opt.MapFrom(src => src.CheckList));
            cfg.CreateMap<Report, ReportListItem>()
                .ForMember(m => m.HasNotes, opt => opt.MapFrom(src => src.Notes.Any()))
                .ForMember(m => m.HasPhotoRecords, opt => opt.MapFrom(src => src.PhotoRecords.Any()))
                .ForMember(m => m.HasSignatures, opt => opt.MapFrom(src => src.Signatures.Any()))
                .ForMember(m => m.CompletedSignaturesCount, opt => opt.MapFrom(src => src.Signatures.Count()))
                .ForMember(m => m.PhotosCount, opt => opt.MapFrom(src => src.PhotoRecords.Count()))
                .ForMember(m => m.NotesCount, opt => opt.MapFrom(src => src.Signatures.Count()));
        });

        public ReportsRepository(InspectionsContext context, ILogger<ReportsRepository> logger
            , IUserNameResolver userNameResolver)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userNameResolver = userNameResolver ?? throw new ArgumentNullException(nameof(userNameResolver));
        }

        public async Task<Report> AddAsync(Report entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<ReportListItem>> GetAll(string? filter, bool? closed, bool myReports)
        {
            var query = _context.Reports
                .Include(p => p.PhotoRecords);

            if (closed.HasValue && closed.Value)
                return await query.AsNoTracking().Where(r => (r.IsClosed) && (!myReports || EF.Property<string>(r, "LastEditUser").Contains(_userNameResolver.UserName)) && EF.Functions.Like(r.Name, $"%{filter}%"))
                    .OrderByDescending(r => r.Date)
                    .ProjectTo<ReportListItem>(config)
                    .ToListAsync();

            return await query.AsNoTracking()
                .Where(r => (!myReports || EF.Property<string>(r, "LastEditUser").Contains(_userNameResolver.UserName)) 
                && (EF.Functions.Like(r.Name, $"%{filter}%") || EF.Functions.Like(r.License!.Name, $"%{filter}%")))
                .OrderByDescending(r => r.Date)
                .ProjectTo<ReportListItem>(config)
                .ToListAsync();
        }

        public Task DeleteAsync(Report entity)
        {
            _context.CheckLists.RemoveRange(entity.CheckList);
            _context.OperationalReadings.RemoveRange(entity.OperationalReadings);
            _context.Signatures.RemoveRange(entity.Signatures);
            _context.SaveChanges();
            _context.Reports.Remove(entity);
            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task<Report> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Reports
                        .Include("CheckList")
                        .Include("Signatures")
                        .Include("PhotoRecords")
                        .Include("Notes")
                        .Include("OperationalReadings")
                       .SingleOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Fetching the report domain data");
                throw;
            }
        }

        public async Task<ReportQueryResult> GetByIdAsync(int id, bool projected)
        {
            return await _context.Reports.Where(r => r.Id == id).ProjectTo<ReportQueryResult>(config).AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Report entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            if (entity.OperationalReadings is not null)
                _context.Entry(entity.OperationalReadings).State = EntityState.Modified;
            if (entity.PhotoRecords is not null)
            {
                foreach (var photo in entity.PhotoRecords)
                {
                    if (photo.Id == 0)
                        _context.Entry(photo).State = EntityState.Added;
                    else
                        _context.Entry(photo).State = EntityState.Modified;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
