using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class PagingResult<MISAEntity>
    {
        public int TotalRecord { get; set; }

        public int TotalPageNumber { get; set; }

        public List<MISAEntity> Entities { get; set; } = new List<MISAEntity>();
    }
}
