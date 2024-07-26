using Reservation.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Core.Contract.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        Task AddedAsync(T entity);  

        Task<IEnumerable<Seat>> GetSeatsByUserId(int userId);
        void UpdateAll(IEnumerable<T> entity);

    }
}
