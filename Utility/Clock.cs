using System;
using System.Diagnostics;

namespace SFML.Time
{
    public class Clock
    {
        #region Variables
        private Stopwatch _timer = Stopwatch.StartNew();
        #endregion

        #region Properties
        public Time ElapsedTime
        {
            get
            {
                return Time.FromTicks(_timer.ElapsedTicks);
            }
        }
        #endregion

        #region Functions
        public Time Restart()
        {
            Time tm = Time.FromTicks(_timer.ElapsedTicks);
            _timer.Reset();
            _timer.Start();
            return tm;
        }
        #endregion
    }
}