using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Imagine.BookManager.EntityFramework.Repositories
{
    public abstract class BookManagerRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<BookManagerDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected BookManagerRepositoryBase(IDbContextProvider<BookManagerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class BookManagerRepositoryBase<TEntity> : BookManagerRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected BookManagerRepositoryBase(IDbContextProvider<BookManagerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
