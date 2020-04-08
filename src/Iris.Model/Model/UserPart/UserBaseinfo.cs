using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Iris.Models.Model.UserPart
{
    public class UserBaseinfo
    {
        [BsonElement("Nickname")]
        public string Nickname { get; set; }
        public string ProfilePicture { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public UserRealInfo RealInfo { get; set; }
        public UserAddress Address { get; set; }
        public List<UserDeliveryAddress> DeliveryAddressList { get; set; }


    }
}
