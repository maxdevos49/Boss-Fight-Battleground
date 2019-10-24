using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using BFB.Web.Models;
using BFB.Web.Services;
using BFB.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace BFB.Web.Controllers
{
    public class RegisterFormController : Controller
    {
        private readonly BFBContext _db;
            
        public RegisterFormController(BFBContext db)
        {
            _db = db;
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(RegisterFormModel model)
        {
            DatabaseService.AddUserEntry(_db,model.Username, model.Email, model.Password);
            return Content($"Hello {model.Username}");
        }
    }
}