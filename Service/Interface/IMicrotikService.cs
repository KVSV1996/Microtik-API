using Microtik_API_Web.Models;

namespace Microtik_API_Web.Service.Interface
{
    public interface IMicrotikService
    {
        IEnumerable<VPNUsers> IndexUsersLogic(string searchString);
        int UsersPageSize();
        void ChangePassword(string nameToChangePassword, string newPassword);
        void DeleteUser(string nameToDelete);
        void CreateUser(string username, string password, string service, string profile);
    }
}
