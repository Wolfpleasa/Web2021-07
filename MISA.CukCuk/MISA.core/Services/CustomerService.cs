using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class CustomerService : BaseService<Customer>,ICustomerService
    {
        ICustomerRepository _customerRepository;
        ServiceResult _serviceResult;

        public CustomerService(ICustomerRepository customerRepository, IBaseRepository<Customer> baseRepository):base(baseRepository)
        {
            _serviceResult = new ServiceResult();
            _customerRepository = customerRepository;
        }
  
    }
}
