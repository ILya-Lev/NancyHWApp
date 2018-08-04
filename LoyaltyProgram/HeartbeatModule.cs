using Nancy;
using System;

namespace LoyaltyProgram
{
    public class HeartbeatModule : NancyModule
    {
        public HeartbeatModule() : base("/heartbeat")
        {
            Get("", _ => DateTime.Now);
        }
    }
}
