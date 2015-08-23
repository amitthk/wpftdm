using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
                    int newIdx = items.IndexOf(parent) + 1;
                    int oldChildIdx = items.IndexOf(child);

                    if (newIdx < items.Count)
                    {
                        items.Remove(child);
                        
                        if (newIdx>oldChildIdx) {
                        	newIdx-=1;
                        }
                        
                        items.Insert(newIdx, child);
                    }
                    else if (newIdx==items.Count)
                    {
                        items.Remove(child);
                        items.Add(child);
                    }

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
                int newIdx = _Todos.IndexOf(prnt)+1;
                if (newIdx <= _Todos.Count)
                {
                    _Todos.Remove(thisItm);
                    _Todos.Insert(newIdx, thisItm);
                }
                else
                {
                    _Todos.Remove(thisItm);
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
            	Debug.Write("\n UpdateChildren: parent "+parent.Title+" index: " +lstTodo.IndexOf(parent));
                var children = lstTodo.Where(x => x.ParentId == parent.Id).ToList();
                for (int tmp = 0; tmp <= children.Count - 1;tmp++ )
                {
                    var child = children[tmp];
                    child.Wbs = parent.Wbs + ">";
                	Debug.Write(",  child "+child.Title+" index: "+lstTodo.IndexOf(child) +"\n");

                int oldChildIdx = lstTodo.IndexOf(child);
                    int newIdx = lstTodo.IndexOf(parent)+1;
                    //Count is already one short
                    if (newIdx < lstTodo.Count)
                    {
                    	
                        lstTodo.Remove(child);
                        
                        if(newIdx>oldChildIdx){
                        	newIdx-=1;
                        }
                        
                        lstTodo.Insert(newIdx, child);
                        Debug.Write(string.Format("\n Move {0} from {1} => {2} \n",child.Title, oldChildIdx,newIdx));
                    }
                    else
                    {
                        lstTodo.Remove(child);
                        lstTodo.Add(child);
                        Debug.Write(string.Format("\n Added {0} from {1} (end of the list) => {2} \n",child.Title, oldChildIdx,newIdx));
                    }
                    
                    Debug.Write(string.Format("\n New Indexes: Parent {0} -> Index {1}, Child {2} -> Index {3} \n",parent.Title,lstTodo.IndexOf(parent), child.Title, lstTodo.IndexOf(child)));
                    
                    UpdateChildren(child);
                }
            };
            Debug.Write("\n Calling UpdateChildren on :"+itm.Title +"\n");
            UpdateChildren(itm);
        }
    }
}
