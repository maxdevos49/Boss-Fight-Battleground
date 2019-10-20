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
            /*
            using (MySqlConnection db = new MySqlConnection(connString))
            {
                try
                {
                    db.Open();

                    string query = "INSERT INTO BFB_User (Username, Email, Password, IsVerified, IsBanned, IsActive, InsertedOn, UpdatedOn, UpdatedBy, EmailToken) VALUES(@username, @email, @pass, false, false, false, @time, @time, @time, null)";
                    MySqlCommand command = new MySqlCommand(query, db);

                    // Add parameters as values here rather than in the initial string to prevent SQL injection.
                    command.Parameters.AddWithValue("@username", model.Username);
                    command.Parameters.AddWithValue("@email", model.Email);
                    command.Parameters.AddWithValue("@pass", model.Password);
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@time", currentDate);

                    command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    return Content(e.ToString());
                }
                
            } */
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