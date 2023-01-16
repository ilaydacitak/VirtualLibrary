using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DenemeSWE.Models;

namespace DenemeSWE.Controllers
{
    public class UserController : Controller
    {
        IheBookEntities5 db = new IheBookEntities5();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(User user)
        {

            if (ModelState.IsValid)
            {
                    var check = db.User.FirstOrDefault(s => s.email == user.email);
                    if (check == null)
                    {
                        if(user.password.Length >= 8) { 
                            //user.password = GetMD5(user.password);
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.User.Add(user);
                            db.SaveChanges();
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            ViewData["SignupFlag"] = "Password must be at least 8 characters";
                            return View();

                        }
                    }
                    else
                    {
                        ViewData["SignupFlag"] = "Email already exists!!";
                        return View();
                    }
                
            }
            return View();
        }
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                    var check = db.User.Where(a => a.email.Equals(user.email) && a.password.Equals(user.password)).FirstOrDefault();
                    if (check != null)
                    {
                        Session["id"] = check.id.ToString();
                        Session["name"] = check.name.ToString();
                        return RedirectToAction("BookList", "Book");

                    }
                    else
                    {
                        ViewData["LoginFlag"] = "Wrong email or password!!";
                        return View();
                    }
            }
            return View();

        }
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
    }
}