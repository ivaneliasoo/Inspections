using Inspections.Core;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces;
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

        public async Task<IEnumerable<Report>> GetAll(string filter, bool? closed, bool myReports)
        {
            var query = _context.Reports
                .Include(p => p.PhotoRecords);

            if (closed.HasValue && closed.Value)
                return await query.AsNoTracking().Where(r => (r.IsClosed) && (myReports ? EF.Property<string>(r, "LastEditUser").Contains(_userNameResolver.UserName) : true) && EF.Functions.Like(r.Name, $"%{filter}%")).OrderByDescending(r => r.Date).ToListAsync();

            return await query.AsNoTracking().Where(r => (myReports ? EF.Property<string>(r, "LastEditUser").Contains(_userNameResolver.UserName) : true) && EF.Functions.Like(r.Name, $"%{filter}%")).OrderByDescending(r => r.Date).ToListAsync();
        }

        public Task DeleteAsync(Report entity)
        {
            _context.CheckLists.RemoveRange(entity.CheckList);
            _context.Signatures.RemoveRange(entity.Signatures);
            _context.SaveChanges();
            _context.Reports.Remove(entity);
            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task<Report> GetByIdAsync(int id)
        {
            return await _context.Reports.Where(r => r.Id == id)
               .Include(p => p.CheckList)
                .ThenInclude(p => p.Checks)
                    .ThenInclude(p => p.TextParams)
               .Include(p => p.Signatures)
                .ThenInclude(p => p.Responsable)
               .Include(p => p.Notes)
               .Include(p => p.PhotoRecords)
               .Include(p => p.License)
               .SingleOrDefaultAsync();
        }

        public async Task<dynamic> GetByIdAsync(int id, bool projected)
        {
            return await _context.Reports
                .Select(report => new
                {
                    report.Id,
                    report.Name,
                    report.Address,
                    report.License,
                    report.Title,
                    report.FormName,
                    report.RemarksLabelText,
                    report.OperationalReadings,
                    Signatures = report.Signatures.OrderBy(o=>o.Order).Select(s => new
                    {
                        s.Date,
                        s.Annotation,
                        s.Designation,
                        s.DrawnSign,
                        s.Id,
                        s.Principal,
                        s.Remarks,
                        Responsable = new
                        {
                            s.Responsable.Name,
                            s.Responsable.Type
                        },
                        s.ResponsableName,
                        s.Title,
                    }),
                    report.Notes,
                    CheckList = report.CheckList.Select(ch => new
                    {
                        ch.Annotation,
                        ch.Checked,
                        ch.Checks,
                        ch.Id,
                        ch.Text
                    }),
                    report.PhotoRecords
                }).AsNoTracking().SingleOrDefaultAsync(r => r.Id == id);

        }


        public async Task UpdateAsync(Report entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
