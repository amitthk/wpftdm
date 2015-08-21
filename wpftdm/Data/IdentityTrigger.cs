using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.Sterling.Core;
using Wintellect.Sterling.Core.Database;

namespace wpftdm.Data
{
    public class IdentityTrigger<T> : BaseSterlingTrigger<T, Guid>
  where T :  IBaseModel, new()
    {
        private static Guid _idx = Guid.Empty;

        public IdentityTrigger(ISterlingDatabaseInstance database)
        {
            // If a record exists, set it to the highest value plus 1
            if (database.Query<T, Guid>().Any())
            {
                _idx = Guid.NewGuid();
            }
        }

        public override bool BeforeSave(T instance)
        {
            if (instance.Id ==Guid.Empty)
            {
                instance.Id = Guid.NewGuid();
            }

            return true;
        }

        public override void AfterSave(T instance)
        {
            return;
        }

        public override bool BeforeDelete(Guid key)
        {
            return true;
        }
    }
}
