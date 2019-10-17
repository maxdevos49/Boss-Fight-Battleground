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
            using (MySqlConnection db = new MySqlConnection(connString))
            {
                try
                {
                    db.Open();
                }
                catch(Exception e)
                {
                    return Content(e.ToString());
                }
            }
            return Content($"Hello {model.Username}");
        }
    }
}