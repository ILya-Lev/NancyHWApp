using NLog;
using System.ServiceProcess;

namespace LoyaltyProgramEventConsumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var service = new Service();
        }
    }

    public class Service : ServiceBase
    {
        private readonly EventSubscriber _eventSubscriber;

        public Service()
        {
            var logger = LogManager.GetLogger("LoyaltyProgram.EventConsumer.Logger");
            _eventSubscriber = new EventSubscriber("http://localhost:28719", logger);
            //#if DEBUG
            //            _eventSubscriber.SubscriptionCycleCallback().Wait();
            //#else
            Run(this);
            //#endif
        }

        protected override void OnStart(string[] args) => _eventSubscriber.Start();

        protected override void OnStop() => _eventSubscriber.Stop();
    }
}
