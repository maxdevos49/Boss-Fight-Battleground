using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using BFB.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace BFB.Web.Controllers
{
    public class RegisterFormController : Controller
    {
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(RegisterFormModel model, [FromServices] DatabaseConfig db)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            db.BFB_User.Add(new BFB_User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                IsVerified = false,
                IsBanned = false,
                IsActive = false,
                InsertedOn = currentDate,
                UpdatedOn = currentDate,
                UpdatedBy = currentDate,
                EmailToken = null
            });
            db.SaveChanges();
            return Content($"Hello {model.Username}");
        }
    }
}