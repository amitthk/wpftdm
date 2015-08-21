using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.Sterling.Core.Database;

namespace wpftdm.Data
{
    public class TodoDatabaseInstance : BaseDatabaseInstance
    {
        public const string DATAINDEX = "IndexData";

        protected override List<ITableDefinition> RegisterTables()
        {
            return new List<ITableDefinition>
                       {
                           CreateTableDefinition<Todo, Guid>(testModel => testModel.Id)
                               .WithIndex<Todo, string, Guid>(DATAINDEX, t => t.Title)
                               .WithIndex<Todo, DateTime, string, Guid>("IndexDateData",
                                                                            t => Tuple.Create(t.CreateDt, t.Title))                                                     
                       };
        }
    }    
}
