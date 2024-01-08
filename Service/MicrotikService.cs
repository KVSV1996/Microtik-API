using Microtik_API_Web.MK.Interface;
using Microtik_API_Web.Models;
using Microtik_API_Web.Service.Interface;

namespace Microtik_API_Web.Service
{
    public class MicrotikService : IMicrotikService
    {
        private IMicrotikHandler _handler;

        public MicrotikService(IMicrotikHandler handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }
        public IEnumerable<VPNUsers> IndexUsersLogic(string searchString)
        {
            var users = _handler.GetData();

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name!.Contains(searchString));
            }

            return users;
        }

        public int UsersPageSize()
        {
            int pageSize = 10;
            return pageSize;
        }

        public void ChangePassword(string nameToChangePassword, string newPassword)
        {
            if (string.IsNullOrEmpty(nameToChangePassword) || string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Строка не может быть пустой или null.");
            }

            _handler.ChangePasswordMK(nameToChangePassword, newPassword);
        }

        public void DeleteUser(string nameToDelete)
        {
            if (string.IsNullOrEmpty(nameToDelete))
            {
                throw new ArgumentException("Строка не может быть пустой или null.");
            }

            _handler.DeleteUserMK(nameToDelete);
        }

       public void CreateUser(string username, string password, string service, string profile)
       {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(service) || string.IsNullOrEmpty(profile))
            {
                throw new ArgumentException("Строка не может быть пустой или null.");
            }

            _handler.CreateUserMK(username, password, service, profile);
       }
    }
}
