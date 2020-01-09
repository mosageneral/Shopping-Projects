using BL.Repositories;
using Microsoft.EntityFrameworkCore;

using Models.AppDBModel;

namespace BL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private DBContext _ctx;
        public UnitOfWork(DBContext ctx)
        {
            _ctx = ctx;
            _ctx.ChangeTracker.LazyLoadingEnabled = true;
        }
       
        public TestRepository testRepository =>  new TestRepository(_ctx);

        public ProductRepository productRepository =>  new ProductRepository(_ctx);

        public CategoryRepository CategoryRepository =>  new CategoryRepository(_ctx);

        public void Dispose()
        {
            _ctx.Dispose();
        }

      

        public int Save()
        {
            return _ctx.SaveChanges();
        }
    }
}
