using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        // static string teamQuery = "SELECT team.id as id, f.id AS foreman_id, f.name AS foreman, vehicle, team.last_update, " +
        //     "ARRAY(SELECT m.id FROM team_member, employee m WHERE team_member.team_id = team.id AND m.id = team_member.employee_id) AS member_ids, " +
        //     "ARRAY(SELECT m.name FROM team_member, employee m WHERE team_member.team_id = team.id AND m.id = team_member.employee_id) AS members " + 
        //     "FROM team, employee f WHERE team.foreman_id=f.id";

        // static string teamQuery = 
        //     "SELECT id, foreman, vehicle, last_update FROM team";

        // static string memberQuery = 
        //     "SELECT id, team_id, name, team_member.last_update FROM team_member";

        [HttpGet]
        public ScheduleData Get() {
            Console.WriteLine("GET jobschedule");
            var jobs = _context.Job.ToList();
            //var schedJobs = _context.SchedJob.OrderBy(sj => sj.team).ToList();
            var schedJobs = _context.SchedJob.ToList();
            var teams = _context.Team.ToList();
            var now = DateTime.Now;
            _logger.LogInformation("schedJobs");
            schedJobs.ForEach(sj => _logger.LogInformation("{0} {1}", sj.id, sj.date, sj.team));
            return new ScheduleData(now, jobs, schedJobs, teams);
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

        // [HttpPut]
        // public async Task<IActionResult> PutSchedJobs()
        // {
        //     if (!this.Request.Body.CanSeek)
        //     {
        //         this.Request.EnableBuffering();
        //     }

        //     this.Request.Body.Position = 0;
        //     var reader = new StreamReader(this.Request.Body, Encoding.UTF8);
        //     var body = await reader.ReadToEndAsync().ConfigureAwait(false);
        //     Console.WriteLine(body);
        //     this.Request.Body.Position = 0;
        //     var options = new JsonSerializerOptions();
        //     ScheduleData? scheduleData = JsonSerializer.Deserialize<ScheduleData>(body, options);
           
        //     return Ok();
        // }

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
                                _logger.LogInformation("Deleting job: {0:D}: {1}", sj.team, sj.date);
                                _context.Remove(prev);
                        } else if (sj.lastUpdate < prev.lastUpdate) {
                            updated.Add(prev);
                        }                    }
                    }
                }
            }
            _context.SaveChanges();

            _logger.LogInformation("Saving scheduled jobs");
            foreach (SchedJob sj in schedJobs) {
                if (sj.id == -1) {
                    continue;
                }
                var prev = _context.SchedJob.Where(s => s.date == sj.date && s.team == sj.team).FirstOrDefault();
                if (prev == null) {
                    _logger.LogInformation("New scheduled job: {0:D}: {1}", sj.id, sj.job1);
                    _context.Add(sj);
                } else {
                    if  (sj.lastUpdate > prev.lastUpdate) {
                        _logger.LogInformation("Updating scheduled job: {0:D}: {1}", sj.id, sj.job1);
                        Console.WriteLine("Updating sjob: {0:D}: {1} {2} {3}", prev.id, prev.job1, prev.teamMembers, prev.lastUpdate);
                        Console.WriteLine("with         : {0:D}: {1} {2} {3}", sj.id, sj.job1, sj.teamMembers, sj.lastUpdate);
                        prev.id = sj.id;
                        prev.team = sj.team;
                        prev.date = sj.date;
                        prev.shift = sj.shift;
                        prev.splitShift = sj.splitShift;
                        prev.job1 = sj.job1;
                        prev.job2 = sj.job2;
                        prev.teamMembers = sj.teamMembers;
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
                    var prev = _context.Team.Where(j => j.id == team.id).FirstOrDefault();
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
    }

    public class ScheduleData
    {
        public DateTime timeStamp { get; set; }

        public IEnumerable<Job>? jobs { get; set; }

        public IEnumerable<SchedJob>? schedJobs { get; set; }

        public IEnumerable<Team>? teams { get; set; }

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
        }
        
        public ScheduleData(IEnumerable<SchedJob> schedJobs) {
            //this.jobs = jobs;
            this.schedJobs = schedJobs;
        }

        public ScheduleData() {
        }        
    }
}
