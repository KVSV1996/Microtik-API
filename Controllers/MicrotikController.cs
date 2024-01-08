using Microsoft.AspNetCore.Mvc;
using Microtik_API_Web.Models;
using Microtik_API_Web.Service.Interface;
using MikrotikDotNet;
using System.Data.Common;

namespace Microtik_API_Web.Controllers
{
    public class MicrotikController : Controller
    {
        MKConnection? conn;
        IMicrotikService _microtikService;

        public MicrotikController(IMicrotikService microtikService)
        {
            _microtikService = microtikService ?? throw new ArgumentNullException(nameof(microtikService));
            conn = new MKConnection("185.183.94.18", "Api_user", "apiuser");
            conn.Open();
        }
        public IActionResult Index(string searchString, int? pageNumber)
        {          
            return View(PaginatedList<VPNUsers>.CreateAsync(_microtikService.IndexUsersLogic(searchString), pageNumber ?? 1, _microtikService.UsersPageSize()));
        }

        public IActionResult ConfirmDelete(string nameToDelete)
        {            
            return View(model: nameToDelete);
        }

        [HttpPost]
        public IActionResult Delete(string nameToDelete)
        {
            Console.WriteLine("Удаляем " + nameToDelete);
            _microtikService.DeleteUser(nameToDelete);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult ChangePassword(string nameToChangePassword, string newPassword)
        {
            Console.WriteLine("Меняе пароль" + nameToChangePassword + " " + newPassword);
            _microtikService.ChangePassword(nameToChangePassword, newPassword);

            return RedirectToAction("Index");
        }

        public IActionResult ChangePasswordPage(string nameToChangePassword)
        {
            return View(model: nameToChangePassword);
        }

        public IActionResult Create()
        {            
            var existingUsers = _microtikService.IndexUsersLogic(null);           

            return View(existingUsers);
        }

        [HttpPost]
        public IActionResult Create(string username, string password, string selectedUser)
        {            

            var user = _microtikService.IndexUsersLogic(null).FirstOrDefault(u => u.Name == selectedUser);
            if (user != null)
            {
                string service = user.Service;
                string profile = user.Profile;

                Console.WriteLine($"Создаём {username} {password} {service} {profile}");
                _microtikService.CreateUser(username, password, service, profile);               
            }           

            return RedirectToAction("Index");
        }             
    }
}
