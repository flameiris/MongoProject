using Iris.Models.Enums;
using Iris.Models.Model.AgentPart;

namespace Iris.Models.Dto.AgentPart
{
    public class AgentForDetailDto
    {
        public string Id { get; set; }
        public string Agentname { get; set; }
        public string ProfilePicture { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public AgentTypeEnum AgentType { get; set; }
    }
}
