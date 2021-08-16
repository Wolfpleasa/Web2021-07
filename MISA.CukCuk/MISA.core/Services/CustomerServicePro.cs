using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class CustomerServicePro : BaseService<Customer>, ICustomerService
    {
        ServiceResult _serviceResult;

        public CustomerServicePro(IBaseRepository baseRepository) : base(baseRepository)
        {
            _serviceResult = new ServiceResult();
            //_customerRepository = customerRepository;
        }


        //public ServiceResult Add(MISAEntity entity)
        //{
        //    throw new NotImplementedException();
        //}



        //public ServiceResult Edit(MISAEntity entity, Guid entityId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
