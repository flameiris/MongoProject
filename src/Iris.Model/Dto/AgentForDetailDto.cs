using Iris.Models.Enums;
using Iris.Models.Model.AgentPart;

namespace Iris.Models.Dto
{
    public class AgentForDetailDto
    {
        public string Agentname { get; set; }
        public string ProfilePicture { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public AgentTypeEnum AgentType { get; set; }


        public static AgentForDetailDto MapTo(Agent _)
        {
            //var m = _mapper.Map<UserForDetailDto>(_);
            AgentForDetailDto m = new AgentForDetailDto();
            m.Agentname = _.Agentname;
            m.ProfilePicture = _.ProfilePicture;
            m.Mobile = _.Mobile;
            m.Email = _.Email;
            m.AgentType = _.AgentType;
            return m;
        }
    }
}
