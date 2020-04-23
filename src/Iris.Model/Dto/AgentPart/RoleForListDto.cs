using Iris.Models.Model.AgentPart;

namespace Iris.Models.Dto.AgentPart
{
    public class RoleForListDto
    {

        public string Username { get; set; }
        public string Nickname { get; set; }
        public string ProfilePicture { get; set; }
        public string Mobile { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }

        public static RoleForListDto MapTo(Role _)
        {
            //var model = _mapper.Map<UserForListDto>(user);

            //更多自定义赋值TODO


            return null;
        }
    }
}
