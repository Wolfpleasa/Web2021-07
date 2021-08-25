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
    public class CustomerGroupService:BaseService<CustomerGroup>, ICustomerGroupService
    {
        #region DECLARE
        ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public CustomerGroupService(IBaseRepository<CustomerGroup> baseRepository) : base(baseRepository)
        {
            _serviceResult = new ServiceResult();       
        }
        #endregion
    }
}
