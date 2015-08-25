using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm.ViewModels
{
    public class HelpViewModel :BaseViewModel
    {

        public HelpViewModel()
        {
            _ShortcutList = getAllShortcuts();
        }

        private ObservableCollection<TableEntry> getAllShortcuts()
        {
            List<TableEntry> lst = new List<TableEntry>();

            lst.Add(new TableEntry(0, 0, "Shift+Right"));
            lst.Add(new TableEntry(0, 1, "Indent Node (Make Child of above node)"));

            lst.Add(new TableEntry(1, 0, "Shift+Left"));
            lst.Add(new TableEntry(1, 1, "Un-Indent Node (Decrease Child Level selected node)"));

            lst.Add(new TableEntry(2, 0, "Ctrl+S"));
            lst.Add(new TableEntry(2, 1, "Saves the list"));

            lst.Add(new TableEntry(3, 0, "Drag + Drop a node"));
            lst.Add(new TableEntry(3, 1, "Makes the Dragged node a Child of target node)"));

            lst.Add(new TableEntry(4, 0, "Delete"));
            lst.Add(new TableEntry(4, 1, "Deletes selected node"));

            return new ObservableCollection<TableEntry>(lst);
        }

        private ObservableCollection<TableEntry> _ShortcutList;

        public ObservableCollection<TableEntry> ShortcutList
        {
            get { return _ShortcutList; }
            set
            {
                if ((null != value) && (_ShortcutList != value))
                {
                    _ShortcutList = value;
                    OnPropertyChanged("ShortcutList");
                }
            }
        }
    }

    public class TableEntry
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string Text { get; set; }

        public TableEntry(int row, int column, string text)
        {
            Row = row;
            Column = column;
            Text = text;
        }
    }
}
