using System;
using System.Collections.Generic;
using Kursova.Data;
using Kursova.Models;
using Kursova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Controllers
{
    [Authorize]
    public class AsortimentController : Controller
    {
        private readonly ILogger<AsortimentController> _logger;
        private readonly IAsortimentRepository _asortimentRepository;
        private readonly OrdersDBContext _dbContext;
        public AsortimentController(ILogger<AsortimentController> logger, IAsortimentRepository asortimentRepository, OrdersDBContext dbContext)
        {
            _logger = logger;
            _asortimentRepository = asortimentRepository;
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Asortiment(string sterm = "", int categoryId = 0)
        {           
            IEnumerable<Asortimet> asortiments = await _asortimentRepository.GetAsortiments(sterm, categoryId);
            IEnumerable<Category> categories = await _asortimentRepository.Categories();
            AsortimentDisplayModel asortimentDisplay = new AsortimentDisplayModel
            {
                Asortiments = asortiments,
                Categories = categories
            };
            return View(asortimentDisplay);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}