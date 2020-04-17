using Iris.Models.Model.UserPart;

namespace Iris.Models.Dto
{

    //[AutoMap(typeof(User))]
    public class UserForDetailDto
    {
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string ProfilePicture { get; set; }
        public string Mobile { get; set; }
        public string IdCard { get; set; }
        public string IdCardPicture { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }


        public static UserForDetailDto MapTo(User _)
        {
            //var m = _mapper.Map<UserForDetailDto>(_);
            UserForDetailDto m = new UserForDetailDto();
            var _b = _.Baseinfo;
            m.Nickname = _b.Nickname;
            m.ProfilePicture = _b.ProfilePicture;
            m.Mobile = _b.Mobile;
            m.Email = _b.Email;
            m.UserType = _b.UserType;




            return m;
        }
    }
}
