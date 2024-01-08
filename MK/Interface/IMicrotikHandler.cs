using Microtik_API_Web.Models;

namespace Microtik_API_Web.MK.Interface
{
    public interface IMicrotikHandler
    {
        IEnumerable<VPNUsers> GetData();
        void ChangePasswordMK(string nameToChangePassword, string newPassword);
        void DeleteUserMK(string nameToDelete);
        void CreateUserMK(string username, string password, string service, string profile);
    }
}
