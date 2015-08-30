﻿using Raven.Imports.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm
{
    public class Todo : BaseViewModel
    {
        private string _Title;

        public string Title
        {
            get { return _Title; }
            set
            {
                if ((null != value) && (_Title != value))
                {
                    _Title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        private int _Estimated;

        public int Estimated
        {
            get { return _Estimated; }
            set
            {
                if ((null != value) && (_Estimated != value))
                {
                    _Estimated = value;
                    OnPropertyChanged("Estimated");
                }
            }
        }

        private int _Executed;

        public int Executed
        {
            get { return _Executed; }
            set
            {
                if ((null != value) && (_Executed != value))
                {
                    _Executed = value;
                    OnPropertyChanged("Executed");
                }
            }
        }

        private int _TotalTime;

        public int TotalTime
        {
            get { return _TotalTime; }
            set
            {
                if ((null != value) && (_TotalTime != value))
                {
                    _TotalTime = value;
                    OnPropertyChanged("TotalTime");
                }
            }
        }

        private DateTime _CreateDt;

        public DateTime CreateDt
        {
            get { return _CreateDt; }
            set
            {
                if ((null != value) && (_CreateDt != value))
                {
                    _CreateDt = value;
                    OnPropertyChanged("CreateDt");
                }
            }
        }

        private DateTime _ModifiedDt;

        public DateTime ModifiedDt
        {
            get { return _ModifiedDt; }
            set
            {
                if ((null != value) && (_ModifiedDt != value))
                {
                    _ModifiedDt = value;
                    OnPropertyChanged("ModifiedDt");
                }
            }
        }

        private Guid _ParentId;

        public Guid ParentId
        {
            get { return _ParentId; }
            set
            {
                if ((null != value) && (_ParentId != value))
                {
                    _ParentId = value;
                    OnPropertyChanged("ParentId");
                }
            }
        }

        private string _Wbs=string.Empty;

        [JsonIgnore]     
        public string Wbs
        {
            get { return _Wbs; }
            set
            {
                if ((null != value) && (_Wbs != value))
                {
                    _Wbs = value;
                    OnPropertyChanged("Wbs");
                }
            }
        }

        private int _Level;

        public int Level
        {
            get { return _Level; }
            set
            {
                if ((null != value) && (_Level != value))
                {
                    _Level = value;
                    OnPropertyChanged("Level");
                }
            }
        }

        public Todo():base()
        {
            //_Id = Guid.NewGuid();
            _CreateDt = DateTime.Now;
        }

    }
    
    public class TodoList :List<Todo>{}
}
