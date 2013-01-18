using System;
using System.Collections.Specialized;
using System.Threading;
using DQF.Platform.Logging;
using Quartz;
using Quartz.Impl;
using StructureMap;

namespace DQF.Scheduler
{
    class Program
    {
        private static StdSchedulerFactory _schedulerFactory;
        private static Container _container;
        private static IScheduler _scheduler;

        static void Main(string[] args)
        {
            _container = new Container();
            new Bootstaper().Configure(_container);
            _container.Configure(x => x.AddRegistry<JobsRegistrator>());
            _schedulerFactory = new StdSchedulerFactory();
            var jobs = _container.GetAllInstances<IScheduledJob>();
            try
            {
               
                _scheduler = _schedulerFactory.GetScheduler();
                _scheduler.JobFactory = new IoCJobFactory(_container);
                foreach (var job in jobs)
                {
                     _scheduler.ScheduleJob(job.ConfigureJob(), job.ConfigureTrigger());
                }
                _scheduler.Start();
            }
            catch (Exception e)
            {
                _container.GetInstance<Logger>().Error(e, "Server initialization failed:" + e.Message);
                throw;
            }
        }
    }
}
