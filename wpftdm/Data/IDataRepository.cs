using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wpftdm.Data
{
    interface IDataRepository
    {
        Guid Add(Todo todo);
        void Delete(Guid id);
        void Delete(Todo id);
        List<Todo> List();
        bool Update(Todo todo);
        Todo Get(Guid id);
    }
}
