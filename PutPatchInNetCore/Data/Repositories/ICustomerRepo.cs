using PutPatchInNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PutPatchInNetCore.Data.Repositories
{
    public interface ICustomerRepo
    {
        Task<ICollection<Customer>> GetAsync();
        Task AddAsync(Customer model);
        Task UpdateAsync(Customer model);
        Task<Customer> GetAsync(int customerId);
        Task SaveAllAsync();
    }
}
