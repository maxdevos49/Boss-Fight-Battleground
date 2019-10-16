using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BFB.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BFB.Web.Controllers
{
    public class RegisterFormController : Controller
    {
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(RegisterFormModel model)
        {
            return Content($"Hello {model.Username}");
        }
    }
}