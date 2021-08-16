using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IBaseRepository
    {
        List<MISAEntity> GetAll<MISAEntity>();

        MISAEntity GetById<MISAEntity>(Guid entityId);

        int Add<MISAEntity>(MISAEntity entity);

        int Edit<MISAEntity>(MISAEntity entity, Guid entityId);

        int Delete<MISAEntity>(Guid entityId);
    }
}
