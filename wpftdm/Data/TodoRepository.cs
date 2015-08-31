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
                App.DocumentSession.Store(todo);
                return (todo.Id);
            }
        }

        public void Delete(Guid id)
        {
            lock (todoUpdateLockObj)
            {
                if (App.DocumentSession.Load<Todo>(id) != null)
                App.DocumentSession.Delete<Todo>(id);
            }
        }

        public List<Todo> List()
        {
            var vals= from k in App.DocumentSession.Query<Todo>().Customize(x=>x.WaitForNonStaleResultsAsOfNow()) orderby k.CreateDt select k;
            return (vals.ToList());
        }

        public bool Update(Todo todo)
        {
            lock (todoUpdateLockObj)
            {
                todo.ModifiedDt = DateTime.Now;
                App.DocumentSession.Store(todo);
                //Task.Run(async () => { await App.DocumentSession.FlushAsync(); });
            }
            return true;
        }

        public Todo Get(Guid id)
        {
            var todo = App.DocumentSession.Load<Todo>(id);
            if (todo != null)
            {
                return ((Todo)todo);
            }
            else
            {
                return null;
            }
        }


        public void Delete(Todo todo)
        {
            lock (todoUpdateLockObj)
            {
                if (App.DocumentSession.Load<Todo>(todo.Id)!=null)
                {
                    App.DocumentSession.Delete<Todo>(todo);
                }
            }
        }
    }
}
