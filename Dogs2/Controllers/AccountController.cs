using Dogs2.Data;
using Dogs2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Dogs2.Controllers
{
    public class AccountController : Controller
    {
        //private SignInManager<LoginViewModel> _signManager;
        //private UserManager<LoginViewModel> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly DogsDB _dogsDB;
        public AccountController(DogsDB dogsDB, ILogger<AccountController> logger)
        //,UserManager<LoginViewModel> userManager, SignInManager<LoginViewModel> signManager)
        {
            _dogsDB = dogsDB;
            _logger = logger;
            //_userManager = userManager;
            //_signManager = signManager;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signManager.SignOutAsync();
        //    return RedirectToAction("Account", "Login");
        //}

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userdetails = await _dogsDB.Userdetails
                        .SingleOrDefaultAsync(m => m.email == model.Email && m.password == model.Password);
                    if (userdetails == null)
                    {
                        ModelState.AddModelError("Password", "Invalid login attempt.");
                        return View("Index");
                    }

                    HttpContext.Session.SetString("displayName", userdetails.displayName);
                    HttpContext.Session.SetString("userId", userdetails.userId.ToString());
                }
                else
                {
                    return View("Index");
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Login));
            }}

        [HttpPost]
        public ActionResult Registar(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {  
                try
                {
                    var emailParam = new SqlParameter("email", model.Email);
                    var email = _dogsDB.Userdetails.FromSqlRaw("EXECUTE SP_UserValidation @email", emailParam).ToList();
                    if (email.Count > 0)
                    {
                        ViewBag.EmailErrorMsg = "User with the same Email Already Exist";
                        return View("Register");
                    }
                    else
                    {
                        UsersModel user = new UsersModel
                        {
                            userName = model.Email,
                            displayName = model.Name,
                            email = model.Email,
                            password = model.Password,
                            phone = model.Mobile

                        };

                        _dogsDB.Add(user);
                        _dogsDB.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                return View("Register");
            }
            return RedirectToAction("Index", "Account");
        }

        // Register Page load
        // [HttpGet]
        public IActionResult Register()
        {
            ViewData["Message"] = "Registration Page";
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}