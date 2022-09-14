using Application.Models;
using RwaLib.DAL.Managers;
using RwaLib.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class AccountController : Controller
    {
        private UserManager um = new UserManager(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        private UserActionsManager uam = new UserActionsManager(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        User user = new User();
        private BookManager bm = new BookManager(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(ApplicationRegisteredUser ruser)
        {
            if (ModelState.IsValid)
            {
                um.Create(new User
                {
                    FirstName = ruser.FirstName,
                    LastName = ruser.LastName,
                    Email = ruser.Email,
                    DateOfBirth = ruser.DateOfBirth,
                    CityName = ruser.CityName,
                    ZipCode = int.Parse(ruser.ZipCode),
                    Address = ruser.Address
                }, ruser.Password);
                User user = um.GetAll().FirstOrDefault(u => u.Email == ruser.Email);
        
                return RedirectToAction("Login", "Account", new { emailSent = "true" });
            }
            return View("Register");
        }


        
        [HttpPost]
        public ActionResult Update(string firstname, string lastname, string address, string cityname, string zipcode, DateTime dateofbirth)
        {
            User user = (User)Session["user"];
            user.FirstName = firstname;
            user.LastName = lastname;
            user.Address = address;
            user.CityName = cityname;
            user.ZipCode = int.Parse(zipcode);
            user.DateOfBirth = dateofbirth;
            string password = um.GetUserPassword(user.IDUser);
            um.Update(user, password);
            return View("Index");
        }

        public ActionResult NewEmployee()
        {
            return View();
        }


        [HttpPost]
        public ActionResult NewEmployee(ApplicationAdmin model)
        {
            if (ModelState.IsValid)
            {
                string password = model.ConfirmPassword;
                User user = ApplicationAdmin.ParseToUser(model);
                um.CreateEmployee(user, password);
                return RedirectToAction("Index", "Account");

            }
            return View();
        }

        [HttpGet]
        public ActionResult Login(string email, string password, string emailSent)
        {
            ViewBag.emailSent = emailSent;
            if (email != null && password != null)
            {
                user = um.AuthUser(email, password);
                if (user == null)
                {
                    ViewBag.msg = "User with that password and email does not exist!";
                    return View("Login");
                }
                if (um.CheckEmail(user) || user.IsAdmin)
                {
                    Session["user"] = user;
                    return RedirectToAction("Index", "Home");
                }
            
                Session["user"] = user;
            }

            return View("Login");
        }

        [HttpPost]
        public ActionResult UpdatePassword(string password, string confirmPassword)
        {
            User user = (User)Session["user"];//noviPassword123!
            bool t1 = password.Any(char.IsUpper);
            bool t2 = password.Any(char.IsDigit);
            bool t3 = password.Any(char.IsSymbol);
            if (password == confirmPassword && password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsDigit) && password.Any(char.IsSymbol))
            {
                um.ChangePassword(user, Cryptography.HashPassword(password));
                ViewBag.msg = "Password is updated!";
                return View("Index", user);
            }
            ViewBag.msg = "Password has to be longer than 7 charachter, has to contain minimum  1 lowercase letter, 1 uppercase letter, 1 symbol and 1 number";
            return View("Index", user);
        }
        [HttpGet]
        public ActionResult Index()
        {
            user = (User)Session["user"];
            if (user == null) return View("Login");
            if (!user.IsAdmin)
            {
                ViewBag.loans = user.Loans.Count.ToString();
                ViewBag.allloans = (IEnumerable<Loan>)um.uam.GetAllLoan(user);
                ViewBag.allBoughtBooks = (IEnumerable<Purchase>)um.uam.GetAllPurchase(user);
            }
            ViewBag.adminLoans= uam.GettOngoingLoans();
            return View(user);
        }

        [HttpPost]
        public ActionResult Return(string id)
        {
            IEnumerable<Loan> loans = uam.GettOngoingLoans();
            loans.ToList().ForEach(l => uam.RefreshLoan(l.User.IDUser));
            uam.DeleteLoan(int.Parse(id));
            ViewBag.adminLoans = uam.GettOngoingLoans();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RefreshLoans()
        {
            IEnumerable<Loan> loans = uam.GettOngoingLoans();
            loans.ToList().ForEach(l => uam.RefreshLoan(l.User.IDUser));
            ViewBag.adminLoans = uam.GettOngoingLoans();
            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult DeleteAcc(int id)
        {
            User user = new User() { IDUser = id };
            um.Delete(user);
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult NewPassword()
        {
            user = (User)Session["user"];
            ViewBag.id = user.IDUser;
            return View();
        }


        [HttpPost]
        public ActionResult NewPassword(int id, ApplicationPassword passwords)
        {
            if (ModelState.IsValid && passwords.Password != null && passwords.ConfirmPassword != null)
            {
                um.ChangePassword(um.Get(id), passwords.ConfirmPassword);
                ViewBag.msg = "Password succesfully changed!";
                return View("Login");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Loans()
        {
            User user = (User)Session["user"];
            if (user is null) return RedirectToAction("Login");
            IEnumerable<Loan> loans = (IEnumerable<Loan>)um.uam.GetAllLoan(user);
            return View(loans);
        }

        [HttpGet]
        public ActionResult Purchases()
        {
            User user = (User)Session["user"];
            if (user is null) return RedirectToAction("Login");
            IEnumerable<Purchase> purchases = (IEnumerable<Purchase>)um.uam.GetAllPurchase(user);
            return View(purchases);
        }

        public ActionResult LogOut()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}