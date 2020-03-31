using AutoMapper;
using Iris.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Models.Dto
{

    [AutoMap(typeof(User))]
    public class UserForDetailDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public static UserForDetailDto MapTo(IMapper _mapper, User user)
        {
            return _mapper.Map<UserForDetailDto>(user);
        }
    }
}
