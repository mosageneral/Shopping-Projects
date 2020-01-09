using BL.Infrastructure;

using Models.AppDBModel;
using Models.Models;

using System.Linq;

namespace BL.Repositories
{
    public interface ITestRepository
    { }

    public class TestRepository : Repository<Test>, ITestRepository
    {
        public TestRepository(DBContext ctx) : base(ctx)
        { }

       
    }
}
