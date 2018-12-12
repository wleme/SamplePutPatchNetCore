using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PutPatchInNetCore.Models;

namespace PutPatchInNetCore.Data.Repositories
{
    public class CustomerRepoInMemory : ICustomerRepo
    {
        private readonly List<Customer> _customers;
        private int _id = 0;

        public CustomerRepoInMemory()
        {
            _customers = new List<Customer>();
        }

        public Task AddAsync(Customer model)
        {
            _id += 1;
            model.Id = _id;
            _customers.Add(model);
            return Task.CompletedTask;
        }

        public Task<ICollection<Customer>> GetAsync()
        {
            return Task.FromResult<ICollection<Customer>>(_customers);
        }

        public Task<Customer> GetAsync(int customerId)
        {
            return Task.FromResult<Customer>(_customers.Where(x => x.Id == customerId).FirstOrDefault());
        }

        public Task SaveAllAsync()
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Customer model)
        {
            var currentCustomer = _customers.Where(x => x.Id == model.Id).FirstOrDefault();
            currentCustomer = model;
            return Task.CompletedTask;
        }
    }
}
