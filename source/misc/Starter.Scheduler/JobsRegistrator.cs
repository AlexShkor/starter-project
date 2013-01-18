using StructureMap.Configuration.DSL;

namespace DQF.Scheduler
{
    public class JobsRegistrator: Registry
    {
        public JobsRegistrator()
        {
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<IScheduledJob>();
                scanner.AddAllTypesOf<IScheduledJob>();
            });
        }
    }
}