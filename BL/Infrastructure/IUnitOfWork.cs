using BL.Repositories;
using System;

namespace BL.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        TestRepository testRepository { get; }
        ProductRepository productRepository { get; }
        CategoryRepository CategoryRepository { get; }



        int Save();

    }
}
