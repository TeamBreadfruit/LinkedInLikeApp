namespace LinkedIn.Data.Contracts
{
    using System;
    using System.Linq;

    public interface IRepository<T> : IDisposable where T : class 
    {
        IQueryable<T> All();

        T GetById(object id);

        T Add(T entity);

        T Update(T entity);

        void Delete(T entity);

        void Delete(object id);

        int SaveChanges();
    }
}
