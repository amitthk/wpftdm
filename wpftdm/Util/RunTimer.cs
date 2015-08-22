using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm.Util
{
    public enum TimerStatus { stopped, running, paused, resting, completed }
    public class RunTimer : BaseViewModel, IDisposable
    {
        private int _Hrs;

        public int Hrs
        {
            get { return _Hrs; }
            set
            {
                if (_Hrs != value)
                {
                    _Hrs = value;
                    OnPropertyChanged("Hrs");
                }
            }
        }
        private int _Mins;

        public int Mins
        {
            get { return _Mins; }
            set
            {
                if (_Mins != value)
                {
                    _Mins = value;
                    OnPropertyChanged("Mins");
                }
            }
        }

        private int _Secs;

        public int Secs
        {
            get { return _Secs; }
            set
            {
                if (_Secs != value)
                {
                    _Secs = value;
                    OnPropertyChanged("Secs");
                }
            }
        }

        private TimerStatus _Status;

        public TimerStatus Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private bool _PlayTickerSound=true;

        public bool PlayTickerSound
        {
            get { return _PlayTickerSound; }
            set
            {
                if (_PlayTickerSound != value)
                {
                    _PlayTickerSound = value;
                    OnPropertyChanged("PlayTickerSound");
                }
            }
        }

        private bool _PlayCompletionSound=true;

        public bool PlayCompletionSound
        {
            get { return _PlayCompletionSound; }
            set
            {
                if ((null != value) && (_PlayCompletionSound != value))
                {
                    _PlayCompletionSound = value;
                    OnPropertyChanged("PlayCompletionSound");
                }
            }
        }

        private bool _ContinuePlay;

        public bool ContinuePlay
        {
            get { return _ContinuePlay; }
            set
            {
                if (_ContinuePlay != value)
                {
                    _ContinuePlay = value;
                    OnPropertyChanged("ContinuePlay");
                }
            }
        }

        private int _ElapsedSecs;

        public int ElapsedSecs
        {
            get { return _ElapsedSecs; }
            private set
            {
                if (_ElapsedSecs != value)
                {
                    _ElapsedSecs = value;
                    OnPropertyChanged("ElapsedSecs");
                }
            }
        }

        private int _RunTimeInSecs = AppConstants.PomodoroDurationMinutesDefault*60;

        public int RunTimeInSecs
        {
            get { return _RunTimeInSecs; }
            set
            {
                if (_RunTimeInSecs != value)
                {
                    _RunTimeInSecs = value;
                    setTimeInSeconds(value);
                    OnPropertyChanged("RunTimeInSecs");
                }
            }
        }

        private int _RestTimeInSecs=AppConstants.RestDurationMinutesDefault*60;

        public int RestTimeInSecs
        {
            get { return _RestTimeInSecs; }
            set
            {
                if (_RestTimeInSecs != value)
                {
                    _RestTimeInSecs = value;
                    OnPropertyChanged("RestTimeInSecs");
                }
            }
        }

        private string _TickerSoundPath=AppConstants.TickerSoundDefaultPath;

        public string TickerSoundPath
        {
            get { return _TickerSoundPath; }
            set
            {
                if ((null != value) && (_TickerSoundPath != value))
                {
                    _TickerSoundPath = value;
                    OnPropertyChanged("TickerSoundPath");
                }
            }
        }

        private string _CompletionSoundPath=AppConstants.CompletionSoundDefaultPath;

        public string CompletionSoundPath
        {
            get { return _CompletionSoundPath; }
            set
            {
                if ((null != value) && (_CompletionSoundPath != value))
                {
                    _CompletionSoundPath = value;
                    OnPropertyChanged("CompletionSoundPath");
                }
            }
        }

        private static System.Windows.Threading.DispatcherTimer _timerClock;

        private static RunTimer _instance;

        public static RunTimer Instance
        {
            get {
                if (_instance==null)
                {
                    _instance = new RunTimer();
                }
                return _instance; }
        }


        private RunTimer()
        {
            Hrs = Mins = Secs = 0;
            initTimer();
        }

        private void setTimeInSeconds(int seconds){
            Hrs = seconds / 3600;
            Mins= (seconds -(Hrs*3600))/60;
            Secs = seconds - (Hrs*3600) - (Mins*60);
        }

        private void initTimer(){
                _timerClock = new System.Windows.Threading.DispatcherTimer();
                //Creates a timerClock and enables it
                _timerClock.Interval = new TimeSpan(0, 0, 1);
                _timerClock.Tick += new EventHandler(TimerClock_Tick);
        }

        public void TimerClock_Tick(object sender, EventArgs e)
        {
            Decrement();
            if ((Status == TimerStatus.running)&&(PlayTickerSound))
            {
                string fullPathToSound = TickerSoundPath;
                SoundPlayer player = new SoundPlayer(fullPathToSound);
                player.Play();

            }
        }

        public void Decrement()
        {
            //timer has completed
            if ((Hrs==0)&&(Mins==0)&&(Secs==0))
            {
                if (Status==TimerStatus.running)
                {
                    if (PlayCompletionSound)
                    {
                        string fullPathToSound = CompletionSoundPath;
                        SoundPlayer player = new SoundPlayer(fullPathToSound);
                        player.Play();
                    }
                    ResetToRest();
                }
                else if(Status==TimerStatus.resting)
                {
                    if (PlayCompletionSound)
                    {
                        string fullPathToSound = CompletionSoundPath;
                        SoundPlayer player = new SoundPlayer(fullPathToSound);
                        player.Play();
                    }

                    if (ContinuePlay)
                    {
                        ResetToRun();
                    }
                    else
                    {
                        Stop();
                    }
                }
            }

            // Else continue counting.
            if (Secs < 1)
            {
                Secs = 59;
                if (Mins == 0)
                {
                    Mins = 59;
                    if (Hrs != 0)
                        Hrs -= 1;

                }
                else
                {
                    Mins -= 1;
                }
            }
            else
            {
                Secs = Secs - 1;
                ElapsedSecs += 1;
            }

        }

        public TimeSpan GetElapsed()
        {
            TimeSpan ts = TimeSpan.FromSeconds(ElapsedSecs);
            return (ts);
        }

        public void Start()
        {
            setTimeInSeconds(RunTimeInSecs);
            _timerClock.Start();
            Status = TimerStatus.running;
        }

        public void Pause()
        {
            _timerClock.Stop();
            Status = TimerStatus.paused;
        }

        public void Resume()
        {
            _timerClock.Start();
            Status = TimerStatus.running;
        }

        public void Complete()
        {
            _timerClock.Stop();
            setTimeInSeconds(RestTimeInSecs);
            Status = TimerStatus.completed;
        }

        public void Stop()
        {
            _timerClock.Stop();
            setTimeInSeconds(RunTimeInSecs);
            Status = TimerStatus.stopped;
        }

        private void ResetToRun()
        {
            _timerClock.Stop();
            setTimeInSeconds(RunTimeInSecs);
            Status = TimerStatus.running;
            _timerClock.Start();
        }

        private void ResetToRest()
        {
            _timerClock.Stop();
            setTimeInSeconds(RestTimeInSecs);
            Status = TimerStatus.resting;
            _timerClock.Start();
        }

        public void Dispose()
        {
            _instance = null;
            _timerClock.Stop();
            _timerClock = null;
            GC.Collect();
        }
    }
}
