using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodsStore.Models;
using GoodsStore.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GoodsStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminManagmentController : Controller
    {        
        private readonly UserManager<User> userManager;        

        public AdminManagmentController(UserManager<User> _userManager)
        {            
            userManager = _userManager;            
        }

        // GET: AdminManagment
        public async Task<ActionResult> Index()
        {
            string roleName = "Administrator";            
            var users = await userManager.GetUsersInRoleAsync(roleName);

            return View(users);
        }        

        // GET: AdminManagment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminManagment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel registerViewModel)
        {
            try
            {
                string emailAdmin = registerViewModel.Email;
                string passwordAdmin = registerViewModel.Password;

                var testAdmin = userManager.FindByEmailAsync(emailAdmin);
                testAdmin.Wait();

                if (testAdmin.Result == null)
                {
                    User administrator = new User
                    {
                        Email = emailAdmin,
                        UserName = emailAdmin,
                    };

                    Task<IdentityResult> newUser = userManager.CreateAsync(administrator, passwordAdmin);
                    newUser.Wait();

                    if (newUser.Result.Succeeded)
                    {
                        Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Administrator");
                        newUserRole.Wait();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ошибка при входе.");
                        return View();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        // GET: AdminManagment/Delete/5
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            return View(user);
        }

        // POST: AdminManagment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                await userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}