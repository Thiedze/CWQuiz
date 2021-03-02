using CW.Thiedze.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CW.Thiedze.Service
{
    public interface IUserService
    {
        public User Login(string username, string passwordSha256);
    }
}
