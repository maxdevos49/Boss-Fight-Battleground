using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BFB.Web.Models;
using BFB.Web.Services;
using BFB.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BFB.Web.Controllers
{
    public class LoginFormController : Controller
    {
        [HttpPost]
        public IActionResult Login(LoginFormModel model)
        {
            Boolean validUser = DatabaseService.ValidateUser(model.Username, model.Password);
            return validUser ? Content("Welcome " + model.Username) : Content("Invalid username or password");
        }
    }
}