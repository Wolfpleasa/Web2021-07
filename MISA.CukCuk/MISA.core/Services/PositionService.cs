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
    public class PositionService:BaseService<Possition> , IPositionService
    {
        public PositionService(IBaseRepository<Possition> baseRepository):base(baseRepository)
        {

        }
    }
}
