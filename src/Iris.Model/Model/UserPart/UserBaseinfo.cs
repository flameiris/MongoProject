using System.Collections.Generic;

namespace Iris.Models.Model.UserPart
{
    public class UserBaseinfo
    {
        /// <summary>
        /// 用户实名认证信息
        /// </summary>
        public UserRealInfo RealInfo { get; set; }
        /// <summary>
        /// 用户所处地区
        /// </summary>
        public UserAddress Address { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public List<UserDeliveryAddress> DeliveryAddressList { get; set; }


    }
}
