using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Infrastructure
{
    public class PublicationScheduler
    {
        public static void Start()
        {
            IEnumerable<Events> evnts;
            List<TimeOfDay> timers = new List<TimeOfDay>();
            List<DateTime> ti = new List<DateTime>();
            using (var context = new EFDbContext())
            {
                evnts = context.Events;

                foreach ( var item in evnts.Where(x => !x.Published))
                {
                    ti.Add(item.PublicationDate.Value);
                    TimeOfDay time = new TimeOfDay(item.PublicationDate.Value.Hour, item.PublicationDate.Value.Minute);
                    timers.Add(time);
                }

            }

            TimeSpan span = ti.FirstOrDefault().Subtract(DateTime.Now);

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<PublicationJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s => s
                    .WithInterval(span.Minutes, IntervalUnit.Minute)
                    .OnEveryDay()
                    .InTimeZone(TimeZoneInfo.Local)
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}