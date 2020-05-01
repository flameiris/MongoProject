using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Iris.Models.Model.ContactPart
{
    /// <summary>
    /// 联系人
    /// </summary>
    public class Contact
    {
        private DateTime _createTime = DateTime.Now.ToLocalTime();
        private DateTime _updateTime = DateTime.Now.ToLocalTime();

        public string UserId { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 用户标签
        /// </summary>
        public List<string> CustomerTagList { get; set; }
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value.ToLocalTime(); }
        }
        public DateTime UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value.ToLocalTime(); }
        }
    }
}
