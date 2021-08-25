using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController :  BaseEntityController<Possition>
    {
        #region DECLARE
        //IPositionService _positionService;
        IBaseRepository<Possition> _baseRepository;
        IBaseService<Possition> _baseService;
        #endregion

        #region Constructor
        public PositionsController(IBaseService<Possition> baseService, IBaseRepository<Possition> baseRepository):base(baseService, baseRepository)
        {
            _baseService = baseService;
            _baseRepository = baseRepository;
        }
        #endregion
    }
}
