using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Concrete;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Scheduler
{
    public class PublicationJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            EFEventRepository rep = new EFEventRepository();

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            int idToExecute = dataMap.GetInt("eventToPublish");
            var ev = rep.GetEvent(idToExecute);
            ev.Published = true;
            rep.SaveEvent(ev);           
        }
    }
}