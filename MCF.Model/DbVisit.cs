using MCF.Data.Orm;
using MCF.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCF.Model
{
    public class DbVisit
    {
        public static AbsOrmDbEntity db = null;
        public static AbsOrmDbEntity GetDbEntity()
        {
            if (db == null)
            {
                db = new OrmDbEntity(DataManager.GetConnectionString("ConnectionStrings"));
                return db;
            }
            else
            {
                return db;
            }
        }
    }
}
