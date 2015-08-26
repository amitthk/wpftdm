using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using wpftdm.Data;
using wpftdm.Util;

namespace wpftdm
{
    public class MainWindowViewModel:BaseViewModel
    {
        private static string AppPath = Environment.CurrentDirectory;
        Data.IDataRepository _todoRepository;

        private ObservableCollection<Todo> _Todos;

        public ObservableCollection<Todo> Todos
        {
            get { return _Todos; }
            set
            {
                if ((null != value) && (_Todos != value))
                {
                    _Todos = value;
                    OnPropertyChanged("Todos");
                }
            }
        }

        private Todo _CurrentTodo;

        public Todo CurrentTodo
        {
            get { return _CurrentTodo; }
            set
            {
                if ((null != value) && (_CurrentTodo != value))
                {
                    _CurrentTodo = value;
                    //ExecUpdateTodo(value);
                    OnPropertyChanged("CurrentTodo");
                }
            }
        }

        public string IsTimerPaused { get {
            return ((RunTimer.Instance.Status == TimerStatus.paused) ? "Resume" : "Pause");
        }
        }

        private readonly ICommand _StartTimerCmd;

        public ICommand StartTimerCmd { get { return (_StartTimerCmd); } }

        private readonly ICommand _ResetTimerCmd;

        public ICommand ResetTimerCmd { get { return (_ResetTimerCmd); } }

        private readonly ICommand _PauseTimerCmd;

        public ICommand PauseTimerCmd { get { return (_PauseTimerCmd); } }

        private readonly ICommand _SaveTodoListCmd;

        public ICommand SaveTodoListCmd { get { return (_SaveTodoListCmd); } }

        private readonly ICommand _ExitAppCmd;

        public ICommand ExitAppCmd { get { return (_ExitAppCmd); } }

        private readonly ICommand _MinimizeToTrayCmd;

        public ICommand MinimizeToTrayCmd { get { return (_MinimizeToTrayCmd); } }

        private readonly ICommand _ShowAppSettingsCmd;

        public ICommand ShowAppSettingsCmd { get { return (_ShowAppSettingsCmd); } }

        private readonly ICommand _RowUpCmd;

        public ICommand RowUpCmd { get { return (_RowUpCmd); } }

        private readonly ICommand _RowDownCmd;

        public ICommand RowDownCmd { get { return (_RowDownCmd); } }

        private readonly ICommand _RowLeftCmd;

        public ICommand RowLeftCmd { get { return (_RowLeftCmd); } }

        private readonly ICommand _RowRightCmd;

        public ICommand RowRightCmd { get { return (_RowRightCmd); } }

        private readonly ICommand _UpdateRowWbsCmd;

        public ICommand UpdateRowWbsCmd { get { return (_UpdateRowWbsCmd); } }
        
        private readonly ICommand _ExportToExcelCmd;

        public ICommand ExportToExcelCmd { get { return (_ExportToExcelCmd); } }

        private readonly ICommand _ShowHelpCmd;

        public ICommand ShowHelpCmd { get { return (_ShowHelpCmd); } }

        private readonly ICommand _UpdateTodoCmd;

        public ICommand UpdateTodoCmd { get { return (_UpdateTodoCmd); } }



        private void ExecShowHelp(object obj)
        {
            //Todo: Add the functionality for ShowHelpCmd Here
            var helpPop = new wpftdm.Views.Help();
            helpPop.ShowDialog();
        }

        [DebuggerStepThrough]
        private bool CanShowHelp(object obj)
        {
            //Todo: Add the checking for CanShowHelp Here
            return (true);
        }

        private void ExecShowAppSettings(object obj)
        {
            //Todo: Add the functionality for ShowAppSettingsCmd Here
            var catOpts = new AppSettingsWindow();
            catOpts.ShowDialog();
        }

        [DebuggerStepThrough]
        private bool CanShowAppSettings(object obj)
        {
            //Todo: Add the checking for CanShowAppSettings Here
            return (true);
        }


        private void ExecExitApp(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }

        //These methods are checked again & again in loop, so we will step through them while debugging
        [DebuggerStepThrough]
        private bool CanMinimizeToTray(object obj)
        {
            return (true);
        }

        private void ExecMinimizeToTrayCmd(object obj)
        {
            var win = (Window)obj;

            var notifyIcon = NotifyIconHelper.Instance.GetIcon(win);

            notifyIcon.Visible = true;
            win.Hide();
        }

        [DebuggerStepThrough]
        private bool CanExitApp(object obj)
        {
            //Todo: Add the checking for CanExitApp Here
            return (true);
        }

        private void ExecExportToExcel(object obj)
        {
            ExportToExcel<Todo,TodoList> s = new ExportToExcel<Todo, TodoList>();
            s.dataToPrint = Todos.ToList();
            s.GenerateReport();
        }

        [DebuggerStepThrough]
        private bool CanExportToExcel(object obj)
        {
            //Todo: Add the checking for CanExitApp Here
            return (true);
        }

        private void ExecSaveTodoList(object obj)
        {
            //Todo: Add the functionality for SaveTodoListCmd Here
            saveAllTodos();
        }

        private void saveAllTodos()
        {
            var old = _todoRepository.List();

            foreach (var item in Todos)
            {
                    if (_todoRepository.Get(item.Id) != null)
                    {
                        _todoRepository.Update(item);
                        item.IsDirty = false;
                    }
                    else
                    {
                        item.Wbs = ">";
                        var guid = _todoRepository.Add(item);
                        item.IsDirty = false;
                    }
            }

            var todel = old.Where(x => !(Todos.Select(y => y.Id).Contains(x.Id))).Select(z=>z.Id);

            foreach (var item in todel)
            {
                _todoRepository.Delete(item);
            }
        }

        private void ExecUpdateTodo(object obj)
        {
            //Todo: Add the functionality for UpdateTodoCmd Here
            if (obj != null)
            {
                var item = (Todo)obj;
                if (_todoRepository.Get(item.Id) != null)
                {
                    _todoRepository.Update(item);
                    item.IsDirty = false;
                }
                else
                {
                    item.Wbs = ">";
                    var guid = _todoRepository.Add(item);
                    item.IsDirty = false;
                }
                Todos[Todos.IndexOf(item)] = item;
            }
        }

        [DebuggerStepThrough]
        private bool CanUpdateTodo(object obj)
        {
            //Todo: Add the checking for CanUpdateTodo Here
            return (true);
        }


        [DebuggerStepThrough]
        private bool CanSaveTodoList(object obj)
        {
            //Todo: Add the checking for CanSaveTodoList Here
            return (true);
        }


        private void ExecPauseTimer(object obj)
        {
            //Todo: Add the functionality for PauseTimerCmd Here
            if (RunTimer.Instance.Status == TimerStatus.paused)
            {
                RunTimer.Instance.Resume();
            }
            else if (RunTimer.Instance.Status == TimerStatus.running)
            {
                RunTimer.Instance.Pause();
            }
            else
            {
                System.Windows.MessageBox.Show("Timer is not running right now. Select a task and click Start first!", "No Timer Running!", System.Windows.MessageBoxButton.OK);
                return;
            }
            OnPropertyChanged("IsTimerPaused");
        }

        [DebuggerStepThrough]
        private bool CanPauseTimer(object obj)
        {
            //Todo: Add the checking for CanPauseTimer Here
            return (true);
        }


        private void ExecReset(object obj)
        {
            //Todo: Add the functionality for ResetCmd Here
            RunTimer.Instance.Pause();
            System.Windows.MessageBoxResult confirmRunResult = System.Windows.MessageBox.Show("Are you sure you want to abandon this pomodoro?", "Abandon Pomodoro?", System.Windows.MessageBoxButton.OKCancel);
            if (confirmRunResult == System.Windows.MessageBoxResult.Cancel)
            {
                RunTimer.Instance.Resume();
                return;
            }


            if (CurrentTodo!=null)
            {
                CurrentTodo.TotalTime += RunTimer.Instance.ElapsedSecs;
                if (_todoRepository.Update(CurrentTodo))
                {
                    CurrentTodo.IsDirty = false;
                    Todos[Todos.IndexOf(CurrentTodo)] = CurrentTodo;
                }
            }

            RunTimer.Instance.RunTimeInSecs = AppSettings.Instance.PomodoroDurationMinutes * 60;
            RunTimer.Instance.RestTimeInSecs = AppSettings.Instance.RestDurationMinutes * 60;
            RunTimer.Instance.Stop();
        }

        [DebuggerStepThrough]
        private bool CanReset(object obj)
        {
            //Todo: Add the checking for CanReset Here
            return (true);
        }


        private void ExecStartTimer(object obj)
        {
            //Todo: Add the functionality for StartTimerCmd Here

            if (CurrentTodo==null)
            {
                System.Windows.MessageBox.Show("Please select a task from todo list first!", "Please select a task!", System.Windows.MessageBoxButton.OK);
                return;
            }
            RunTimer.Instance.Start();

        }

        [DebuggerStepThrough]
        private bool CanStartTimer(object obj)
        {
            //Todo: Add the checking for CanStartTimer Here
            return (true);
        }



        public MainWindowViewModel()
        {
            _StartTimerCmd = new RelayCommand(ExecStartTimer, CanStartTimer);
            _ResetTimerCmd = new RelayCommand(ExecReset, CanReset);
            _PauseTimerCmd = new RelayCommand(ExecPauseTimer, CanPauseTimer);
            _SaveTodoListCmd = new RelayCommand(ExecSaveTodoList, CanSaveTodoList);
            _ExitAppCmd = new RelayCommand(ExecExitApp, CanExitApp);
            _MinimizeToTrayCmd = new RelayCommand(ExecMinimizeToTrayCmd, CanMinimizeToTray);
            _ShowAppSettingsCmd = new RelayCommand(ExecShowAppSettings, CanShowAppSettings);
            _RowUpCmd = new RelayCommand(ExecRowUp, CanRowUp);
            _RowDownCmd = new RelayCommand(ExecRowDown, CanRowDown);
            _RowRightCmd = new RelayCommand(ExecRowRight, CanRowRight);
            _RowLeftCmd = new RelayCommand(ExecRowLeft, CanRowLeft);
            _UpdateRowWbsCmd = new RelayCommand(ExecUpdateRowWbs, CanUpdateRowWbs);
            _ExportToExcelCmd = new RelayCommand(ExecExportToExcel, CanExportToExcel);
            _ShowHelpCmd = new RelayCommand(ExecShowHelp, CanShowHelp);
            _UpdateTodoCmd = new RelayCommand(ExecUpdateTodo, CanUpdateTodo);

            _todoRepository = new Data.TodoRepository();
            loadAllTodos();
            RunTimer.Instance.RunTimeInSecs=(AppSettings.Instance.PomodoroDurationMinutes * 60);
        }

        private void loadAllTodos()
        {
            _Todos = new ObservableCollection<Todo>(_todoRepository.List()); ;
            MainWindowUIHelper.setWbs(_Todos);
            OnPropertyChanged("Todos");
        }



        private void ExecRowDown(object obj)
        {

            Todo thisItm = (Todo)obj;
            int idx = _Todos.IndexOf(thisItm);
            if (idx > 0)
            {
                int nextIndex = idx + 2;
                //The next item can be child of parent and then this will lead to stackoverflow exception

                //if (nextIndex < _Todos.Count())
                //{
                //    var parentId = _Todos[nextIndex].ParentId;
                //    MainWindowUIHelper.setNewParent(thisItm.Id, parentId, _Todos);
                //    OnPropertyChanged("Todos");

                //    thisItm = _Todos[idx];
                //    if (_todoRepository.Update(thisItm))
                //    {
                //        return;
                //    }
                //    else
                //    {
                //        var r = MessageBox.Show("Error saving updates to database!", "Error in saving data!");
                //    }
                //}
            }
        }

        [DebuggerStepThrough]
        private bool CanRowDown(object obj)
        {
            //Todo: Add the checking for CanRowDown Here
            return (true);
        }

        private void ExecRowUp(object obj)
        {
            //Todo: Add the functionality for RowUpCmd Here
            Todo thisItm = (Todo)obj;
            int idx = _Todos.IndexOf(thisItm);
            //if (idx > 0)
            //{
            //    int previousIndex = idx - 2;
            //    if (previousIndex >= 0)
            //    {
            //        var parentId = _Todos[previousIndex].Id;
            //        MainWindowUIHelper.setNewParent(thisItm.Id, parentId, _Todos);
            //        OnPropertyChanged("Todos");

            //        thisItm = _Todos[idx];
            //        if (_todoRepository.Update(thisItm))
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            var r = MessageBox.Show("Error saving updates to database!", "Error in saving data!");
            //        }
            //    }
            //}
        }

        [DebuggerStepThrough]
        private bool CanRowUp(object obj)
        {
            //Todo: Add the checking for CanRowUp Here
            return (true);
        }

        private void ExecRowRight(object obj)
        {
            //saveAllTodos();
            Todo thisItm = (Todo)obj;
            int idx = _Todos.IndexOf(thisItm);
            if (idx > 0)
            {
                int parentIndex = idx - 1;
                if (parentIndex>=0)
                {
                    var parentId = _Todos[parentIndex].Id;
                    MainWindowUIHelper.setNewParent(thisItm.Id, parentId, _Todos);
                    OnPropertyChanged("Todos");

                    thisItm = _Todos.FirstOrDefault(x => x.Id == thisItm.Id); ;
                    if (_todoRepository.Update(thisItm))
                    {
                        thisItm.IsDirty = false;
                        OnPropertyChanged("CurrentTodo");
                        return;
                    }
                    else
                    {
                       var r= MessageBox.Show("Error saving updates to database!","Error in saving data!");
                    }
                }
            }
        }

        [DebuggerStepThrough]
        private bool CanRowRight(object obj)
        {
            //Todo: Add the checking for CanRowRight Here
            return (true);
        }


        private void ExecRowLeft(object obj)
        {
            //saveAllTodos();
            Todo thisItm = (Todo)obj;
            int idx = _Todos.IndexOf(thisItm);
            if (idx > 0 )
            {
             if(thisItm.ParentId != Guid.Empty)
                {
                 Todo parentItm = _Todos.FirstOrDefault(x => x.Id == thisItm.ParentId);

                    MainWindowUIHelper.setNewParent(thisItm.Id, parentItm.ParentId, _Todos);
                    OnPropertyChanged("Todos");
                    thisItm = _Todos.FirstOrDefault(x => x.Id == thisItm.Id); ;

                 //Update database
                    if (_todoRepository.Update(thisItm))
                    {
                        thisItm.IsDirty = false;
                        OnPropertyChanged("CurrentTodo");
                        return;
                    }
                    else
                    {
                        var r = MessageBox.Show("Error saving updates to database!", "Error in saving data!");
                    }
                }
            }
        }

        [DebuggerStepThrough]
        private bool CanRowLeft(object obj)
        {
            //Todo: Add the checking for CanRowLeft Here
            return (true);
        }

        private void ExecUpdateRowWbs(object obj)
        {
            //Todo: Add the functionality for UpdateRowWbsCmd Here
            Tuple<object, object> vals = (Tuple<object, object>)obj;
            Todo thisItm = (Todo)vals.Item1;
            Todo targetItm = (Todo)vals.Item2;
            int idx = _Todos.IndexOf(thisItm);
            if ((targetItm!=null)&&(idx >= 0))
            {
                //Item is already inserted to target by the DragDropBehavior
                thisItm.ParentId = targetItm.Id;
                thisItm.Wbs = targetItm.Wbs + ">";

                Debug.Write(string.Format("\n\n****Now Moving child {0} under parent {1} ****",thisItm.Title,targetItm.Title));
                Debug.Write("\nThis item index: "+Todos.IndexOf(thisItm));
                Debug.Write("\nParent item index: "+Todos.IndexOf(targetItm));

                MainWindowUIHelper.updateChildren(thisItm, Todos);
                
                Debug.Write("*****Done Moving Child****\n\n");
                OnPropertyChanged("Todos");

                thisItm = _Todos.FirstOrDefault(x=>x.Id==thisItm.Id);
                //Update database
                if (_todoRepository.Update(thisItm))
                {
                    thisItm.IsDirty = false;
                    return;
                }
                else
                {
                    var r = MessageBox.Show("Error saving updates to database!", "Error in saving data!");
                }
            }
        }

        [DebuggerStepThrough]
        private bool CanUpdateRowWbs(object obj)
        {
            //Todo: Add the checking for CanUpdateRowWbs Here
            return (true);
        }

    }
}
