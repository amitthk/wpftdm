using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm.Util
{
    class TodoComparer : IComparer<Todo>
    {
        // allow us to look up parent Todos by GUID
        IDictionary<Guid, Todo> TodoLookup;

        public TodoComparer(IEnumerable<Todo> list)
        {
            TodoLookup = list.ToDictionary(Todo => Todo.Id);
            foreach (var Todo in list)
                SetLevel(Todo);
        }

        int SetLevel(Todo Todo)
        {
            if (Todo.Level == 0 && (Todo.ParentId!=Guid.Empty))
                Todo.Level = 1 + TodoLookup[Todo.ParentId].Level;
            return Todo.Level;
        }

        public int Compare(Todo x, Todo y)
        {
            // see if x is a child of y
            while (x.Level > y.Level)
            {
                if (x.ParentId == y.Id)
                    return 1;
                x = TodoLookup[x.ParentId];
            }
            // see if y is a child of x
            while (y.Level > x.Level)
            {
                if (y.ParentId == x.Id)
                    return -1;
                y = TodoLookup[y.ParentId];
            }
            // x and y are not parent-child, so find common ancestor
            while (x.ParentId != y.ParentId)
            {
                x = TodoLookup[x.ParentId];
                y = TodoLookup[y.ParentId];
            }
            // compare createDate of children of common ancestor
            return x.CreateDt.CompareTo(y.CreateDt);
        }
    }
}
