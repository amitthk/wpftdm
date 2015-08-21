using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wpftdm.Data
{
    public abstract class IBaseModel
    {
        private Guid _Id=Guid.Empty;

        public Guid Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public IBaseModel()
        {
            _Id = Guid.NewGuid();
        }
    }
}
