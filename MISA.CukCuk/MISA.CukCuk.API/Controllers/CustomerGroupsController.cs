using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerGroupsController : BaseEntityController<CustomerGroup>
    {
        #region DECLARE
        IBaseRepository<CustomerGroup> _baseRepository;
        IBaseService<CustomerGroup> _baseService;
        //ICustomerService _customerService;
        #endregion

        #region Constructor
        public CustomerGroupsController(ICustomerService customerService, IBaseService<CustomerGroup> baseService, IBaseRepository<CustomerGroup> baseRepository) : base(baseService, baseRepository)
        {
            _baseService = baseService;
            _baseRepository = baseRepository;
        }
        #endregion
    }
}
