using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Scheduler
{
    public static class PublicationScheduler
    {
        public static void Start(IEnumerable<Events> events)
        {
            Queue<DateTimeOffset> timers = new Queue<DateTimeOffset>();
            foreach (var item in events.Where(x => !x.Published))
            {
                timers.Enqueue(new DateTimeOffset(item.PublicationDate.Value));
            }

            Task.Run(() => 
            {
                foreach (var item in timers)
                {
                    Events publicEvent = new Events();
                    publicEvent = events.FirstOrDefault(x => x.PublicationDate == item);
                    IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                    scheduler.Start();

                    IJobDetail job = JobBuilder.Create<PublicationJob>().UsingJobData("eventToPublish" , publicEvent.EventID).Build();
                    ITrigger trigger = TriggerBuilder.Create().StartAt(item).Build();
                    scheduler.ScheduleJob(job, trigger);
                }
            });
        }
    }
}