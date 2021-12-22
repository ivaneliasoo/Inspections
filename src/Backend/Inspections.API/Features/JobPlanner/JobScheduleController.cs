using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

#nullable enable
namespace Inspections.API.Features.JobPlanner
{
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Route("api/[controller]")]
    public class JobScheduleController : ControllerBase
    {
        private readonly ILogger<JobScheduleController> _logger;
        private readonly InspectionsContext _context;

        public JobScheduleController(ILogger<JobScheduleController> logger, InspectionsContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ScheduleData Get() {
            Console.WriteLine("GET jobschedule");
            var jobs = _context.Job.ToList();
            //var schedJobs = _context.SchedJob.OrderBy(sj => sj.team).ToList();
            var schedJobs = _context.SchedJob.ToList();
            var teams = _context.Team.ToList();
            var options = _context.Options.Where(op => op.id == 1).FirstOrDefault();
            var now = DateTime.Now;
            //_logger.LogInformation("schedJobs");
            //schedJobs.ForEach(sj => _logger.LogInformation("{0} {1}", sj.id, sj.date, sj.team));
            return new ScheduleData(now, jobs, schedJobs, teams, options);
        }

        // PUT: JobSchedule
        [HttpPut]
        public ScheduleData PutSchedJobs(ScheduleData scheduleData)
        {
            // Console.WriteLine("PUT jobschedule");
            var updatedJobs = SaveJobs(scheduleData.jobs);
            var updatedSchedJobs = SaveSchedJobs(scheduleData.schedJobs);
            var updatedTeams = SaveTeams(scheduleData.teams);
            return new ScheduleData(DateTime.Now, updatedJobs, updatedSchedJobs, updatedTeams);
        }

        [HttpPut("sched-job")]
        public List<SchedJob> SaveSchedJobs(IEnumerable<SchedJob> schedJobs)
        {
            var updated = new List<SchedJob>();
            _logger.LogInformation("Deleting scheduled jobs");
            foreach (SchedJob sj in schedJobs) {
                if (sj.id == -1) {
                    var prev = _context.SchedJob.Where(s => s.date == sj.date && s.team == sj.team).FirstOrDefault();
                    if (prev != null) {
                        if (sj.lastUpdate > prev.lastUpdate) {
                            if (sj.lastUpdate > prev.lastUpdate && sj.id < 0) {
                                //_logger.LogInformation("Deleting job: {0:D}: {1}", sj.team, sj.date);
                                _context.Remove(prev);
                        } else if (sj.lastUpdate < prev.lastUpdate) {
                            updated.Add(prev);
                        }                    }
                    }
                }
            }
            _context.SaveChanges();

            foreach (SchedJob sj in schedJobs) {
                if (sj.id == -1) {
                    continue;
                }
                var prev = _context.SchedJob.Where(s => s.date == sj.date && s.team == sj.team).FirstOrDefault();
                if (prev == null) {
                    // _logger.LogInformation("New scheduled job: {0:D}: {1}", sj.id, sj.job1);
                    _context.Add(sj);
                } else {
                    if  (sj.lastUpdate > prev.lastUpdate) {
                        prev.id = sj.id;
                        prev.team = sj.team;
                        prev.date = sj.date;
                        prev.shift = sj.shift;
                        prev.splitShift = sj.splitShift;
                        prev.job1 = sj.job1;
                        prev.job2 = sj.job2;
                        prev.teamMembers = sj.teamMembers;
                        prev.excludeSunday = sj.excludeSunday;
                        prev.lastUpdate = DateTime.Now;
                    } else if (sj.lastUpdate < prev.lastUpdate) {
                        updated.Add(prev);
                    }
                }
            }
            _context.SaveChanges();
            return updated;
        }

        [HttpPut("job")]
        public List<Job> SaveJobs(IEnumerable<Job> jobs)
        {
            _logger.LogInformation("Saving jobs");
            var updated = new List<Job>();
            foreach (Job job in jobs) {
                if (job.id == 0) {
                    _logger.LogInformation("New job: {0:D}: {1}", job.id, job.scope);
                    job.lastUpdate = DateTime.Now;
                    _context.Add(job);
                    updated.Add(job);
                } else { 
                    var prev = _context.Job.Where(j => j.id == Math.Abs(job.id)).FirstOrDefault();
                    if (prev != null) {
                        if (job.lastUpdate > prev.lastUpdate) {
                            if (job.id < 0) {
                                _logger.LogInformation("Deleting job: {0:D}: {1}", job.id, job.scope);
                                job.id = -job.id;
                                _context.Remove(prev);
                            } else {
                                _logger.LogInformation("Updating job: {0:D}: {1}", job.id, job.scope);
                                prev.status = job.status;
                                prev.priority = job.priority;
                                prev.scope = job.scope;
                                prev.value = job.value;
                                prev.tag = job.tag;
                                prev.comments = job.comments;
                                prev.teams = job.teams;
                                prev.teamCount = job.teamCount;
                                prev.duration = job.duration;
                                prev.shift = job.shift;
                                prev.salesPerson = job.salesPerson;
                                prev.lastUpdate = DateTime.Now;
                            }
                        } else if (job.lastUpdate < prev.lastUpdate) {
                            updated.Add(prev);
                        }
                    }
                } 
            }
            _context.SaveChanges();
            return updated;
        }

        [HttpPut("team")]
        public List<Team> SaveTeams(IEnumerable<Team> teams)
        {
             _logger.LogInformation("Saving teams");
            var updated = new List<Team>();
            foreach (Team team in teams) {
                if (team.id == 0) {
                    _logger.LogInformation("New team: {0:D}: {1}", team.id, team.foreman);
                    team.lastUpdate = DateTime.Now;
                    _context.Add(team);
                } else {
                    var prev = _context.Team.Where(j => j.id == Math.Abs(team.id)).FirstOrDefault();
                    if (prev != null) {
                        if (team.lastUpdate > prev.lastUpdate) {
                            if (team.id < 0) {
                                _logger.LogInformation("Deleting team: {0:D}: {1}", team.id, team.foreman);
                                team.id = -team.id;
                                _context.Remove(prev);
                            } else {
                                _logger.LogInformation("Updating team: {0:D}: {1}", team.id, team.foreman);
                                prev.vehicle = team.vehicle;
                                prev.foreman = team.foreman;
                                prev.position = team.position;
                                prev.teamMembers = team.teamMembers;
                                prev.lastUpdate = DateTime.Now;
                            }
                        } else if (team.lastUpdate < prev.lastUpdate) {
                            updated.Add(prev);
                        }
                    }
                } 
            }
            _context.SaveChanges();
            return updated;
        }

        [HttpPut("options")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateOptions(Options options)
        {
            Options opt = new Options();
            opt.id = 1;
            opt.scheduleWeeks = options.scheduleWeeks;
            opt.autosaveInterval = options.autosaveInterval;

            _context.Update(opt);

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

    public class ScheduleData
    {
        public DateTime timeStamp { get; set; }

        public IEnumerable<Job>? jobs { get; set; }

        public IEnumerable<SchedJob>? schedJobs { get; set; }

        public IEnumerable<Team>? teams { get; set; }

        public Options? options { get; set; }


        public ScheduleData(DateTime timeStamp, IEnumerable<Job> jobs, IEnumerable<SchedJob> schedJobs, 
                IEnumerable<Team> teams, Options opt) {
            this.timeStamp = timeStamp;
            this.jobs = jobs;
            this.schedJobs = schedJobs;
            this.teams = teams;
            this.options = opt;
        }

        public ScheduleData(DateTime timeStamp, IEnumerable<Job> jobs, IEnumerable<SchedJob> schedJobs, 
                IEnumerable<Team> teams) {
            this.timeStamp = timeStamp;
            this.jobs = jobs;
            this.schedJobs = schedJobs;
            this.teams = teams;
        }

        public ScheduleData(DateTime timeStamp, IEnumerable<Job> jobs, IEnumerable<SchedJob> schedJobs) {
            this.timeStamp = timeStamp;
            this.jobs = jobs;
            this.schedJobs = schedJobs;
            this.options = new Options();
        }
        
        public ScheduleData(IEnumerable<SchedJob> schedJobs) {
            //this.jobs = jobs;
            this.schedJobs = schedJobs;
        }

        public ScheduleData() {
        }        
    }
}
