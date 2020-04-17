using Iris.Models.Model.UserPart;

namespace Iris.Models.Dto
{
    /// <summary>
    /// 返回对象
    /// </summary>

    //[AutoMap(typeof(User))]
    public class UserForListDto
    {
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string ProfilePicture { get; set; }
        public string Mobile { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }

        public static UserForListDto MapTo(User user)
        {
            //var model = _mapper.Map<UserForListDto>(user);

            //更多自定义赋值TODO


            return null;
        }
    }
}
