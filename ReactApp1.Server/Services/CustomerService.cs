using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.DTOs;
using ReactApp1.Server.Interface;
using ReactApp1.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApp1.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<CustomerDTO> GetAll()
        {
            return _customerRepository.GetAll()
                .Select(c => new CustomerDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    NIP = c.NIP,
                    PhoneNumber = c.PhoneNumber,
                    ContactPerson = c.ContactPerson,
                    Interventions = c.Interventions.Select(i => new InterventionDTO
                    {
                        Id = i.Id,
                    }).ToList(),
                    Addresses = c.Addresses.Select(a => new AddressDTO
                    {
                        Id = a.Id,
                    }).ToList()
                })
                .ToList();
        }

        public CustomerDTO? Get(int id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null) return null;

            return new CustomerDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                NIP = customer.NIP,
                PhoneNumber = customer.PhoneNumber,
                ContactPerson = customer.ContactPerson,
                Interventions = customer.Interventions.Select(i => new InterventionDTO
                {
                    Id = i.Id,
                }).ToList(),
                Addresses = customer.Addresses.Select(a => new AddressDTO
                {
                    Id = a.Id,
                }).ToList()
            };
        }
    }
}
