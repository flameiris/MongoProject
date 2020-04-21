using System;

namespace Iris.Models.Request
{
    public class AgentForPageRequest
    {
        public string Agentname { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
