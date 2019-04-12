using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginReg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {
        private LoginRegContext dbContext;

        public HomeController(LoginRegContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == newUser.Email);
                if (userInDb != null)
                {
                    ModelState.AddModelError("Email", "This email already taken");
                    return View("Index");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Add(newUser);
                dbContext.SaveChanges();
                var userToLogIn = dbContext.Users.FirstOrDefault(u => u.Email == newUser.Email);
                HttpContext.Session.SetInt32("UserId", userToLogIn.UserId);
                return RedirectToAction("Success");
            }
            else
                return View("Index");
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }

                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();

                // varify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);

                // result can be compared to 0 for failure
                if (result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    return RedirectToAction("Success");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [Route("success")]
        [HttpGet]
        public IActionResult Success()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Index");

            int? userId = HttpContext.Session.GetInt32("UserId");
            User oneUser = dbContext.Users.FirstOrDefault(user =>
            user.UserId == userId);
            UserTransaction newUserTrans = new UserTransaction();
            newUserTrans.user = oneUser;

            List<Transaction> TransactionsWithUsers = dbContext.Transactions
                .Include(transaction => transaction.Creator)
                .OrderByDescending(t=>t.CreatedAt)
                .ToList();

            return View("Success", newUserTrans);
        }

        [Route("/transaction")]
        [HttpPost]
        public IActionResult MakeTransaction(Transaction newTransaction)
        {
            if (ModelState.IsValid)
            {
            User oneUser = dbContext.Users.FirstOrDefault(user =>
            user.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            if(oneUser.Balance - newTransaction.Amount < 0 || newTransaction.Amount == null)
            {
                Console.WriteLine("LEss zero");
                return RedirectToAction("Success");
            }
            oneUser.Balance +=newTransaction.Amount;
                newTransaction.UserId = (int)HttpContext.Session.GetInt32("UserId");
                dbContext.Add(newTransaction);
                dbContext.SaveChanges();
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Success");
            }
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
