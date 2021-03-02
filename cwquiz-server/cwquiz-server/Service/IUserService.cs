using CW.Thiedze.Domain;

namespace CW.Thiedze.Service
{
    public interface IUserService
    {
        public User Login(string username, string password);
    }
}
