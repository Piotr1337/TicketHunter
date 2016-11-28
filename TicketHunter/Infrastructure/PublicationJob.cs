using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TicketHunter.Infrastructure
{
    public class PublicationJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Debug.WriteLine("PRACA");
        }
    }
}