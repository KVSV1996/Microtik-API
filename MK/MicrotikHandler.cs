using Microsoft.Extensions.Configuration;
using Microtik_API_Web.MK.Interface;
using Microtik_API_Web.Models;
using MikrotikDotNet;

namespace Microtik_API_Web.MK
{
    public class MicrotikHandler : IMicrotikHandler
    {
        private MKConnection? conn;       
        private readonly IConfiguration _configuration;

        public MicrotikHandler(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (string.IsNullOrEmpty(_configuration["MikroTikSettings:IpAddress"]) || string.IsNullOrEmpty(_configuration["MikroTikSettings:Username"]) || string.IsNullOrEmpty(_configuration["MikroTikSettings:Password"]))
            {
                throw new ArgumentException("Строка не может быть пустой или null.");
            }            

            conn = new MKConnection(_configuration["MikroTikSettings:IpAddress"], _configuration["MikroTikSettings:Username"], _configuration["MikroTikSettings:Password"]);
            conn.Open();
        }
        public IEnumerable<VPNUsers> GetData()
        {
            var cmd = conn.CreateCommand("ppp secret print");
            var result = cmd.ExecuteReader<VPNUsers>();

            return result;
        }

        public void ChangePasswordMK(string nameToChangePassword, string newPassword)
        {
            if (string.IsNullOrEmpty(nameToChangePassword) || string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Строка не может быть пустой или null.");
            }

            var cmd = conn.CreateCommand($"ppp secret set");
            cmd.Parameters.Add("numbers", nameToChangePassword);
            cmd.Parameters.Add("password", newPassword);
            cmd.ExecuteNonQuery();

        }

        public void DeleteUserMK(string nameToDelete)
        {
            if (string.IsNullOrEmpty(nameToDelete))
            {
                throw new ArgumentException("Строка не может быть пустой или null.");
            }

            var cmd = conn.CreateCommand($"ppp secret remove");
            cmd.Parameters.Add("numbers", nameToDelete);
            cmd.ExecuteNonQuery();

        }

        public void CreateUserMK(string username, string password, string service, string profile)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(service) || string.IsNullOrEmpty(profile))
            {
                throw new ArgumentException("Строка не может быть пустой или null.");
            }

            var cmd = conn.CreateCommand("ppp secret add");
            cmd.Parameters.Add("name", username);
            cmd.Parameters.Add("password", password);
            cmd.Parameters.Add("profile", profile);
            cmd.Parameters.Add("service", service);
            cmd.ExecuteNonQuery();
        }
    }
}
