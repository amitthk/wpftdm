using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm.Data
{
    public class TodoRepository :IDataRepository
    {
        public Guid Add(Todo todo)
        {
          var guid =App.databaseInstance.SaveAsync(todo).Result;
          App.databaseInstance.FlushAsync();
          return ((Guid)guid);
        }

        public void Delete(Guid id)
        {
            App.databaseInstance.DeleteAsync(typeof(Todo), id);
            App.databaseInstance.FlushAsync();
        }

        public List<Todo> List()
        {
            var vals= from k in App.databaseInstance.Query<Todo, Guid>() orderby k.LazyValue.Value.CreateDt select k.LazyValue.Value;
            return (vals.ToList());
        }

        public bool Update(Todo todo)
        {
            App.databaseInstance.SaveAsync(todo);
            App.databaseInstance.FlushAsync();
            return true;
        }

        public Todo Get(Guid id)
        {
            var todo = App.databaseInstance.LoadAsync<Todo>(id).Result;
            if (todo != null)
            {
                return ((Todo)todo);
            }
            else
            {
                return null;
            }
        }
    }
}
