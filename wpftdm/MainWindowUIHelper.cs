using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm
{
    public class MainWindowUIHelper
    {
        public static void setWbs(ObservableCollection<Todo> items)
        {
            Action<Todo> SetPostion = null;
            SetPostion = parent =>
            {
                //Recursively call the SetChildren method for each child.
                var children = items.Where(x => x.ParentId == parent.Id).ToList();
                for (int tmp = 0; tmp <= children.Count - 1; tmp++)
                {
                    var child = children[tmp];
                    child.Wbs = parent.Wbs + ">";
                    items.Move(items.IndexOf(child), items.IndexOf(parent) + 1);
                    SetPostion(child);
                }
            };
            //Initialize the hierarchical list to root level items
            var roots = items.Where(x => x.ParentId == Guid.Empty).ToList();
            for (int tmp = 0; tmp <= roots.Count - 1; tmp++)
            {
                var x=roots[tmp];
                x.Wbs = ">";
                SetPostion(x);
            }
            items.OrderBy(x => x.Wbs);
            //return (items);
        }

        public static void setNewParent(Guid itemId, Guid parentId, ObservableCollection<Todo> _Todos)
        {
            var thisItm = _Todos.FirstOrDefault(x=>x.Id==itemId);

            if (parentId!= Guid.Empty)
            {
                var prnt = _Todos.FirstOrDefault(x => x.Id == parentId);
                thisItm.ParentId = prnt.Id;
                thisItm.Wbs = prnt.Wbs + ">";
                int newIdx = _Todos.IndexOf(prnt) + 1;
                if (newIdx < _Todos.Count)
                {
                    _Todos.Move(_Todos.IndexOf(thisItm), newIdx);
                }
                else
                {
                    _Todos.RemoveAt(_Todos.IndexOf(thisItm));
                    _Todos.Add(thisItm);
                }
            }
            else
            {
                thisItm.ParentId = Guid.Empty;
                thisItm.Wbs = ">";
            }

            //_Todos[itemIndex] = thisItm;
            updateChildren(thisItm, _Todos);


        }

        public static void updateChildren(Todo itm, ObservableCollection<Todo> lstTodo)
        {
            Action<Todo> UpdateChildren = null;
            UpdateChildren = parent =>
            {
                var children = lstTodo.Where(x => x.ParentId == parent.Id).ToList();
                for (int tmp = 0; tmp <= children.Count - 1;tmp++ )
                {
                    var child = children[tmp];
                    child.Wbs = parent.Wbs + ">";

                    int newIdx=lstTodo.IndexOf(parent)+1;

                    if (newIdx<lstTodo.Count)
	                {
                        lstTodo.Move(lstTodo.IndexOf(child), newIdx);
                    }
                    else
                    {
                        lstTodo.RemoveAt(lstTodo.IndexOf(child));
                        lstTodo.Add(child);
                    }

                    UpdateChildren(child);
                }
            };
            UpdateChildren(itm);
        }
    }
}
