using DeliveryApp.Data;
using DeliveryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Repository
{
    public class AsortimentRepository: IAsortimentRepository
    {
        private readonly OrdersDBContext _context;

        public AsortimentRepository(OrdersDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> Categories()
        {
            return await _context.Category.ToArrayAsync();
        }
        public async Task<IEnumerable<Asortimet>> GetAsortiments(string sTerm="", int categoryId = 0)
        {
            sTerm=sTerm.ToLower();
            IEnumerable<Asortimet> asortiments = await (from asortiment in _context.Asortiment
                               join category in _context.Category
                               on asortiment.CategoryId equals category.Id
                               where string.IsNullOrWhiteSpace(sTerm) || (asortiment!=null && asortiment.Name.ToLower().StartsWith(sTerm))

                               select new Asortimet
                               {
                                   Id = asortiment.Id,
                                   Name = asortiment.Name,
                                   Image = asortiment.Image,
                                   Description = asortiment.Description,
                                   Cost = asortiment.Cost,
                                   CategoryId = asortiment.CategoryId,
                                   CategoryName = category.NameCategory
                               }
                              ).ToListAsync();
            if(categoryId > 0 )
            {
                asortiments = asortiments.Where(a => a.CategoryId == categoryId).ToList();
            }
            return asortiments;
        }
    }
}
