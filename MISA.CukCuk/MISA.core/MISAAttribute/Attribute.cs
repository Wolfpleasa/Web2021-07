using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.MISAAttribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MISARequired : Attribute 
    { 
            
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MISAEmail : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MISAPhoneNumber : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MISAIsNumber : Attribute
    {

    }
}
