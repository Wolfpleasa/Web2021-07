using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MISA.Core.Entities;
using System.Data;
using MySqlConnector;
using Dapper;
using System.Text.RegularExpressions;
using MISA.Core.Interfaces.Services;
using MISA.Core.Interfaces.Repository;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : BaseEntityController<Customer>
    {
        #region DECLARE
        //IDepartmentService _departmentService;
        IBaseRepository<Customer> _baseRepository;
        IBaseService<Customer> _baseService;
        #endregion

        #region Constructor
        public CustomersController(IBaseService<Customer> baseService, IBaseRepository<Customer> baseRepository) : base(baseService, baseRepository)
        {
            _baseService = baseService;
            _baseRepository = baseRepository;
        }
        #endregion

    }
}
