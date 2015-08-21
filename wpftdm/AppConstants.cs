using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm
{
    public class AppConstants
    {
        public static string AppBasePath { get { return (Environment.CurrentDirectory); } }

        public static int PomodoroDurationMinutesDefault { get { return (25); } }
        public static int RestDurationMinutesDefault { get { return (5); } }

        public static string AppDataPath { get { return (System.IO.Path.Combine(AppConstants.AppBasePath, "Db")); } }
    }
}
