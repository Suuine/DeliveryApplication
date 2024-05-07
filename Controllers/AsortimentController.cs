using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Kursova.Controllers
{
    public class AsortimentController : Controller
    {
        public IActionResult Asortiment()
        {
            return View();
        }
    }
}