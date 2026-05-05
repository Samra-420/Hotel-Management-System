using HotelManagementSystem.Models;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HotelManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly MongoDbService _db;

        public AccountController(MongoDbService db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _db.Users.Find(u => u.Username == username).FirstOrDefaultAsync();

            if (user == null)
            {
                if (username == "admin" && password == "admin123")
                {
                    var newUser = new User
                    {
                        Username = "admin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                        Role = "Admin"
                    };
                    await _db.Users.InsertOneAsync(newUser);
                    HttpContext.Session.SetString("Username", "admin");
                    HttpContext.Session.SetString("Role", "Admin");
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}