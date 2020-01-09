using BL.Infrastructure;

using Models.AppDBModel;
using Models.Models;

using System.Linq;

namespace BL.Repositories
{
    public interface ICategoryRepository
    { }

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DBContext ctx) : base(ctx)
        { }

      
    }
}
