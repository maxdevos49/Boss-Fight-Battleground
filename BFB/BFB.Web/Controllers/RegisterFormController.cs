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

        private string connString;

        public RegisterFormController(IOptions<DatabaseConfig> config)
        {
            connString = config.Value.ConnectionString;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(RegisterFormModel model)
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
            return Content($"Hello {model.Username}");
        }
    }
}