using AutoMapper;
using Iris.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Models.Dto
{
    /// <summary>
    /// 返回对象
    /// </summary>

    [AutoMap(typeof(User))]
    public class UserForListDto
    {
        public string Username { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }
    }
}
