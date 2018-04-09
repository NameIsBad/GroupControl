using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using DapperExtensions;

using GroupControl.IDAL;
using GroupControl.Model;
using GroupControl.Common;

namespace GroupControl.DAL
{
    public class ContractsInfoDAL : BaseDAL<ContractsInfo, ContractsInfoViewModel>, IContractsInfoDAL
    {

    }
}
