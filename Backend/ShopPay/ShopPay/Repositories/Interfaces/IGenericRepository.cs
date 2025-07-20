namespace ShopPay.Repositories.Interfaces
{
    public interface IGenericRepository<T, TKey> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(TKey id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
