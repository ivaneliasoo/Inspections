using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        private readonly string APIVersion = "v1.202";

        public JobScheduleController(ILogger<JobScheduleController> logger, InspectionsContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ScheduleData GetJobSchedule()
        {
            _logger.LogInformation("GET jobschedule");
            var jobs = _context.Job.ToList();
            var schedJobs = _context.SchedJob.ToList();
            var teams = _context.Team.ToList();
            var options = _context.Options.Where(op => op.id == 1).FirstOrDefault();
            _logger.LogInformation(APIVersion);
            return new ScheduleData(DateTime.Now, jobs, schedJobs, teams, options, APIVersion);
        }

        // PUT: JobSchedule
        [HttpPut]
        public ScheduleData PutSchedJobs(ScheduleData scheduleData)
        {
            // Console.WriteLine("PUT jobschedule");
            SaveTeams(scheduleData.teams);
            SaveJobs(scheduleData.jobs);
            SaveSchedJobs(scheduleData.schedJobs);
            return GetJobSchedule();
        }

        [HttpPut("sched-job")]
        public void SaveSchedJobs(IEnumerable<SchedJob> schedJobs)
        {
            _logger.LogInformation("Updating SchedJobs");
            foreach (SchedJob sj in schedJobs)
            {
                var prev = _context.SchedJob.Where(s => s.date == sj.date && s.team == sj.team).FirstOrDefault();
                if (prev == null)
                {
                    if (sj.lastUpdate == null)
                    {
                        _logger.LogInformation("New scheduled job: {0:D}: {1}", sj.id, sj.job1);
                        sj.lastUpdate = DateTime.Now;
                        _context.Add(sj);
                    }
                }
                else
                {
                    double result = 0;
                    if (sj.lastUpdate != null)
                    {
                        result = Math.Truncate((prev.lastUpdate - sj.lastUpdate).Value.TotalSeconds);
                    }
                    if (result <= 0)
                    {
                        if (sj.id < 0)
                        {
                            _logger.LogInformation("Deleting scheduled job: {0:D}: {1}", sj.id, sj.job1);
                            sj.id = -sj.id;
                            _context.Remove(prev);
                        }
                        else if (sj.updated)
                        {
                            _logger.LogInformation("Updating scheduled job: {0:D}: {1}", sj.id, sj.job1);
                            prev.id = sj.id;
                            prev.team = sj.team;
                            prev.date = sj.date;
                            prev.shift = sj.shift;
                            prev.splitShift = sj.splitShift;
                            prev.job1 = sj.job1;
                            prev.job2 = sj.job2;
                            prev.teamMembers = sj.teamMembers;
                            prev.excludeSunday = sj.excludeSunday;
                            prev.excludeSaturday = sj.excludeSaturday;
                            prev.lastUpdate = DateTime.Now;
                            prev.updated = false;
                        }
                    }
                }
            }
            _context.SaveChanges();
        }

        [HttpPut("job")]
        public void SaveJobs(IEnumerable<Job> jobs)
        {
            //_logger.LogInformation("Saving jobs");
            foreach (Job job in jobs)
            {
                if (job.id == 0)
                {
                    // _logger.LogInformation("New job: {0:D}: {1}", job.id, job.scope);
                    job.lastUpdate = DateTime.Now;
                    _context.Add(job);
                }
                else
                {
                    var prev = _context.Job.Where(j => j.id == Math.Abs(job.id)).FirstOrDefault();
                    if (prev != null)
                    {
                        var result = Math.Truncate((prev.lastUpdate - job.lastUpdate).Value.TotalSeconds);
                        if (result <= 0)
                        {
                            if (job.id < 0)
                            {
                                // _logger.LogInformation("Deleting job: {0:D}: {1}", job.id, job.scope);
                                job.id = -job.id;
                                _context.Remove(prev);
                            }
                            else if (job.updated)
                            {
                                //_logger.LogInformation("Updating job: {0:D}: {1}", job.id, job.scope);
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
                                prev.updated = false;
                            }
                        }
                    }
                }
            }
            _context.SaveChanges();
        }

        [HttpPut("team")]
        public void SaveTeams(IEnumerable<Team> teams)
        {
            foreach (Team team in teams)
            {
                if (team.id == 0)
                {
                    // if id == 0 this is a new team that must be inserted in the database
                    // _logger.LogInformation("New team: {0:D}: {1}", team.id, team.foreman);
                    team.lastUpdate = DateTime.Now;
                    _context.Add(team);
                    _logger.LogInformation("Adding team {0}", team.id);
                }
                else
                {
                    var prev = _context.Team.Where(j => j.id == Math.Abs(team.id)).FirstOrDefault();
                    if (prev == null)
                    {
                        // This team was deleted from the database by another user
                        // Return the team with id set to -id this will cause the team to be deleted from the page 
                        prev.id = -prev.id;
                    }
                    else
                    {
                        var result = Math.Truncate((prev.lastUpdate - team.lastUpdate).Value.TotalSeconds);
                        if (result > 0)
                        {
                            // _logger.LogInformation("Team: {0:D}: {1} was updated, adding to updated list", team.id, team.foreman);
                        }
                        else if (team.updated)
                        {
                            // _logger.LogInformation("Updating team: {0:D}: {1}", team.id, team.foreman);
                            prev.id = team.id;
                            prev.vehicle = team.vehicle;
                            prev.foreman = team.foreman;
                            prev.position = team.position;
                            prev.teamMembers = team.teamMembers;
                            prev.lastUpdate = DateTime.Now;
                            prev.updated = false;
                        }
                    }
                }
            }
            _context.SaveChanges();
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
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime? lastUpdate { get; set; }

        public IEnumerable<Job>? jobs { get; set; }

        public IEnumerable<SchedJob>? schedJobs { get; set; }

        public IEnumerable<Team>? teams { get; set; }

        public Options? options { get; set; }

        public string? apiVersion { get; set; }

        public ScheduleData(DateTime? timeStamp, IEnumerable<Job> jobs, IEnumerable<SchedJob> schedJobs,
                IEnumerable<Team> teams, Options? opt, string APIVersion)
        {
            this.lastUpdate = timeStamp;
            this.jobs = jobs;
            this.schedJobs = schedJobs;
            this.teams = teams;
            this.options = opt;
            this.apiVersion = APIVersion;
        }

        public ScheduleData(DateTime? timeStamp, IEnumerable<Job> jobs, IEnumerable<SchedJob> schedJobs,
                IEnumerable<Team> teams)
        {
            this.lastUpdate = timeStamp;
            this.jobs = jobs;
            this.schedJobs = schedJobs;
            this.teams = teams;
        }

        public ScheduleData(DateTime? timeStamp, IEnumerable<Job> jobs, IEnumerable<SchedJob> schedJobs)
        {
            this.lastUpdate = timeStamp;
            this.jobs = jobs;
            this.schedJobs = schedJobs;
            this.options = new Options();
        }

        public ScheduleData(IEnumerable<SchedJob> schedJobs)
        {
            //this.jobs = jobs;
            this.schedJobs = schedJobs;
        }

        public ScheduleData()
        {
        }
    }
}
