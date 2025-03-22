using prueba.Data;

namespace prueba.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext Context { get; }
        Task<bool> Complete();
    }
}