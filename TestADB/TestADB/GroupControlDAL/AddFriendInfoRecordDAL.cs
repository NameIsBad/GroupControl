using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using Dapper;
using DapperExtensions;

using GroupControl.IDAL;
using GroupControl.Model;
using GroupControl.Common;

namespace GroupControl.DAL
{
    public class AddFriendInfoRecordDAL:BaseDAL<AddFriendInfoRecord, BaseViewModel>, IAddFriendInfoRecordDAL
    {

    }
}
