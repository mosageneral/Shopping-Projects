using BL.Infrastructure;

using Models.AppDBModel;
using Models.Models;

using System.Linq;

namespace BL.Repositories
{
    public interface IProductRepositoryRepository
    { }

    public class ProductRepository : Repository<Products>, IProductRepositoryRepository
    {
        public ProductRepository(DBContext ctx) : base(ctx)
        { }

       
    }
}
