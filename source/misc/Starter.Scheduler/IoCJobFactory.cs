using System;
using Quartz;
using Quartz.Spi;
using StructureMap;

namespace DQF.Scheduler
{
    public class IoCJobFactory : IJobFactory
    {
        private readonly Container _container;

        public IoCJobFactory(Container container)
        {
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJobDetail jobDetail = bundle.JobDetail;
            Type jobType = jobDetail.JobType;

            // Return job that is registrated in container
            return _container.GetInstance(jobType) as IJob;
        }
    }
}