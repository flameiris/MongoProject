using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Models.Model
{
    public class UserBaseinfo
    {
        [BsonElement("Nickname")]
        public string Nickname { get; set; }
        public string ProfilePicture { get; set; }
        public string Mobile { get; set; }
        public string IdCard { get; set; }
        public string IdCardPicture { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public UserAddress Address { get; set; }
        public List<UserDeliveryAddress> DeliveryAddressList { get; set; }


    }
}
