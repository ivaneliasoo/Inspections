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

        public async Task<dynamic> GetByIdAsync(int id, bool projected)
        {
            return await _context.Reports
                .Select(report => new
                {
                    report.Id,
                    report.Name,
                    report.Address,
                    License = report.License != null ? new
                    {
                        report.License.Number,
                        report.License.Name,
                        report.License.KVA,
                        report.License.Volt,
                        report.License.Amp,
                        report.License.Validity
                    }:null,
                    report.Title,
                    report.FormName,
                    report.RemarksLabelText,
                    report.OperationalReadings,
                    Signatures = report.Signatures.OrderBy(o => o.Order).Select(s => new
                    {
                        s.Date,
                        s.Annotation,
                        s.Designation,
                        s.DrawnSign,
                        s.Id,
                        s.Principal,
                        s.Remarks,
                        Responsible = new
                        {
                            s.Responsible.Name,
                            s.Responsible.Type
                        },
                        s.ResponsibleName,
                        s.Title,
                        s.Order
                    }),
                    report.Notes,
                    CheckList = report.CheckList.Select(ch => new
                    {
                        ch.Annotation,
                        ch.Checked,
                        Checks = ch.Checks.Select(chch => new
                        {
                            chch.Checked,
                            chch.CheckListId,
                            chch.Editable,
                            chch.Id,
                            chch.Remarks,
                            chch.Required,
                            chch.Text
                        }),
                        ch.Id,
                        ch.Text
                    })
                }).AsNoTracking().SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Report entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
