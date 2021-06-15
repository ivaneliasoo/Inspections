﻿using AutoMapper;
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

        public async Task<IEnumerable<Report>> GetAll(string? filter, bool? closed, bool myReports)
        {
            var query = _context.Reports
                .Include(p => p.PhotoRecords);

            if (closed.HasValue && closed.Value)
                return await query.AsNoTracking().Where(r => (r.IsClosed) && (!myReports || EF.Property<string>(r, "LastEditUser").Contains(_userNameResolver.UserName)) && EF.Functions.Like(r.Name, $"%{filter}%")).OrderByDescending(r => r.Date).ToListAsync();

            return await query.AsNoTracking().Where(r => (!myReports || EF.Property<string>(r, "LastEditUser").Contains(_userNameResolver.UserName)) && EF.Functions.Like(r.Name, $"%{filter}%")).OrderByDescending(r => r.Date).ToListAsync();
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
            return await _context.Reports
                .Include("CheckList")
                .Include("Signatures")
               .AsNoTracking().SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ReportQueryResult> GetByIdAsync(int id, bool projected)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Signature, SignatureQueryResult>();
                cfg.CreateMap<CheckListItem, CheckListItemQueryResult>();
                cfg.CreateMap<CheckList, CheckListQueryResult>().ForMember(m => m.Checks, opt => opt.MapFrom(src =>src.Checks));
                cfg.CreateMap<Report, ReportQueryResult>()
                    .ForMember(m => m.Signatures, opt => opt.MapFrom(src => src.Signatures.OrderBy(ob => ob.Order)))
                    .ForMember(m => m.CheckLists, opt => opt.MapFrom(src => src.CheckList));
            });
            return await _context.Reports.Where(r => r.Id == id).ProjectTo<ReportQueryResult>(config).AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Report entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
