using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using beltexam.Models;

namespace beltexam.Controllers
{
    public class HomeController : Controller
    {
        private BeltContext dbContext;

        public HomeController(BeltContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(User User)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(u => u.Email == User.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    User.Password = Hasher.HashPassword(User, User.Password);
                    dbContext.Add(User);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", User.UserId);
                    return RedirectToAction("Ideas");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginUser UserLog)
        {
            if (ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == UserLog.Email);
                if (userInDb == null)
                {
                    ModelState.AddModelError("UserLog.Email", "The email you entered did not match our records.");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(UserLog, userInDb.Password, UserLog.Password);
                if (result == 0)
                {
                    ModelState.AddModelError("UserLog.Password", "The password you entered did not match our records.");
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    return RedirectToAction("Ideas");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("Ideas")]
        public IActionResult Ideas()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            int UserId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            ViewBag.User = dbContext.Users.SingleOrDefault(user => user.UserId == UserId);
            ViewBag.Ideas = dbContext.Ideas.Include(like => like.Likes).Include(u => u.User).OrderByDescending(c=>c.Likes.Count).ToList();
            return View();
        }

        [HttpPost("NewIdea")]
        public IActionResult NewIdea(Idea idea)
        {
            int UserId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {

                idea.UserId = UserId;
                dbContext.Add(idea);
                dbContext.SaveChanges();
                return RedirectToAction("Ideas");
            }
            ViewBag.User = dbContext.Users.SingleOrDefault(user => user.UserId == UserId);
            ViewBag.Ideas = dbContext.Ideas.Include(like => like.Likes).Include(u => u.User).OrderByDescending(c=>c.Likes.Count).ToList();
            return View("Ideas", idea);
        }

        [HttpGet("Delete/{ideaId}")]
        public IActionResult Delete(int ideaId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            int userId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            Idea idea = dbContext.Ideas.FirstOrDefault(x => x.IdeaId == ideaId);
            if (idea.UserId != userId)
            {
                return RedirectToAction("Ideas");
            }
            List<Like> Deleteidea = dbContext.Likes.Where(x => x.IdeaId == ideaId).ToList();
            foreach (var like in Deleteidea)
            {
                dbContext.Likes.Remove(like);
            }
            dbContext.Remove(idea);
            dbContext.SaveChanges();
            return RedirectToAction("Ideas");
        }

        [HttpGet("Like/{ideaId}")]
        public IActionResult Like(int ideaId)
        {
            int UserId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            if (dbContext.Likes.Any(x => x.UserId == UserId && x.IdeaId == ideaId))
            {
                return RedirectToAction("Ideas");
            }
            Like like = new Like()
            {
                UserId = UserId,
                IdeaId = ideaId,
                
            };
            dbContext.Add(like);
            dbContext.SaveChanges();
            return RedirectToAction("Ideas");
        }

        [HttpGet("Users/{userId}")]
        public IActionResult UserNum(int userId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.User=dbContext.Users.SingleOrDefault(u => u.UserId == userId);
            ViewBag.IdeaCount=dbContext.Ideas.Where(i => i.UserId== userId).Count();
            ViewBag.LikeCount=dbContext.Likes.Where(l => l.UserId == userId).Count();
            return View();
        }

        [HttpGet("Ideas/{ideaId}")]
        public IActionResult IdeaNum(int ideaId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Idea = dbContext.Ideas.Include(u => u.User).SingleOrDefault(i => i.IdeaId == ideaId);
            ViewBag.Likers = dbContext.Likes.Include(u => u.User).Where(i => i.IdeaId == ideaId);
            return View();
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}