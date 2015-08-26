using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm.Data
{
    public class TodoRepository :IDataRepository
    {
        private object todoUpdateLockObj = new object();

        public Guid Add(Todo todo)
        {
            lock (todoUpdateLockObj)
            {
                var guid = Task.Run(async () => { object td = await App.databaseInstance.SaveAsync(todo); return td; }).Result;
                //Task.Run(async () => { await App.databaseInstance.FlushAsync(); });
                return ((Guid)guid);
            }
        }

        public void Delete(Guid id)
        {
            lock (todoUpdateLockObj)
            {
                Task.Run(async () => { await App.databaseInstance.DeleteAsync(typeof(Todo), id); });
                //Task.Run(async () => { await App.databaseInstance.FlushAsync(); });
            }
        }

        public List<Todo> List()
        {
            var vals= from k in App.databaseInstance.Query<Todo, Guid>() orderby k.LazyValue.Value.CreateDt select k.LazyValue.Value;
            return (vals.ToList());
        }

        public bool Update(Todo todo)
        {
            lock (todoUpdateLockObj)
            {
                todo.ModifiedDt = DateTime.Now;
                Task.Run(async () => { await App.databaseInstance.SaveAsync(todo); });
                //Task.Run(async () => { await App.databaseInstance.FlushAsync(); });
            }
            return true;
        }

        public Todo Get(Guid id)
        {
            var todo =  Task.Run(async () => { object td = await App.databaseInstance.LoadAsync<Todo>(id); return td;}).Result;
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
