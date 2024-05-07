using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursova.Data;
using Kursova.Models;
namespace DeliveryApp.Controllers
{
    public class UploadsAsortimentController : Controller
    {
        private readonly OrdersDBContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UploadsAsortimentController(OrdersDBContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Asortiment == null)
            {
                return NotFound();
            }

            var asortimet = await _context.Asortiment
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asortimet == null)
            {
                return NotFound();
            }

            return View(asortimet);
        }
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "NameCategory");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,Name,Description,Cost,CategoryId")] Asortimet asortimet, IFormFile imageFile)
        {
            try
            {
                if (imageFile != null)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "pictures");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    asortimet.Image = uniqueFileName;
                }
                Category category = await _context.Category.FindAsync(asortimet.CategoryId);

                Asortimet newAsortimet = new Asortimet
                {
                    Image = asortimet.Image,
                    Name = asortimet.Name,
                    Description = asortimet.Description,
                    Cost = asortimet.Cost,
                    CategoryId = asortimet.CategoryId,
                    Category = category,
                    CategoryName = category?.NameCategory,
                };
                _context.Add(newAsortimet);

                await _context.SaveChangesAsync();
                return RedirectToAction("Asortiment", "Asortiment");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Create action: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            ViewBag.Categories = new SelectList(_context.Category, "Id", "NameCategory");
            return View(asortimet);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Asortiment == null)
            {
                return NotFound();
            }

            var asortimet = await _context.Asortiment.FindAsync(id);
            if (asortimet == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "NameCategory", asortimet.CategoryId);
            return View(asortimet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Name,Description,Cost,CategoryId")] Asortimet asortimet, IFormFile imageFile)
        {
            if (id != asortimet.Id)
            {
                return NotFound();
            }

            Asortimet existingAsortimet = await _context.Asortiment.FindAsync(id);
            try
            {
                if (imageFile != null)
                {
                    if (!string.IsNullOrEmpty(existingAsortimet.Image))
                    {
                        var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, "pictures", existingAsortimet.Image);

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "pictures");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    existingAsortimet.Image = uniqueFileName;
                }
                else existingAsortimet.Image = existingAsortimet.Image;
                Category category = await _context.Category.FindAsync(asortimet.CategoryId);

                if (existingAsortimet == null)
                {
                    return NotFound();
                }

                existingAsortimet.Name = asortimet.Name;
                existingAsortimet.Description = asortimet.Description;
                existingAsortimet.Cost = asortimet.Cost;
                existingAsortimet.CategoryId = asortimet.CategoryId;
                existingAsortimet.Category = category;
                existingAsortimet.CategoryName = category?.NameCategory;

                _context.Update(existingAsortimet);
                await _context.SaveChangesAsync();

                return RedirectToAction("Asortiment", "Asortiment");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AsortimetExists(asortimet.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "NameCategory", asortimet.CategoryId);
            return View(asortimet);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Asortiment == null)
            {
                return NotFound();
            }

            var asortimet = await _context.Asortiment
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asortimet == null)
            {
                return NotFound();
            }

            return View(asortimet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asortimet = await _context.Asortiment
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (asortimet == null)
            {
                return NotFound();
            }

            var imageFileName = asortimet.Image;

            _context.Asortiment.Remove(asortimet);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(imageFileName))
            {
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "pictures", imageFileName);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            return RedirectToAction("Asortiment", "Asortiment");
        }
        private bool AsortimetExists(int id)
        {
            return (_context.Asortiment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
