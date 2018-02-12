using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Model
{
    public interface IHasIdentityCol<T>
    {
        T GetIdentityColValue();
    }
}
