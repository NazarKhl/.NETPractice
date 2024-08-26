using ReactApp1.Server.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactApp1.Server.Interface
{
    public interface ICustomerService
    {
        List<CustomerDTO> GetAll();
        CustomerDTO? Get(int id);
    }
}
