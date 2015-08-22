using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm
{
    public class AppSettingsInfo
    {
        public int PomodoroDurationMinutes { get; set; }
        public int RestDurationMinutes { get; set; }
        public string CompletionSoundPath { get; set; }
        public string TickerSoundPath { get; set; }
    }
}
