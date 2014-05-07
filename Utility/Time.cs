using System;
using System.Diagnostics;

namespace SFML.Time
{
    public class Time
    {
        #region Variables
        private long _elapsedticks = 0;
        private static Time _zero = new Time(0);
        #endregion

        #region Properties
        public static Time Zero
        {
            get
            {
                return _zero;
            }
        }
        public long Ticks
        {
            get
            {
                return _elapsedticks;
            }
            set
            {
                _elapsedticks = value;
            }
        }
        public long Milliseconds
        {
            get
            {
                return (long)(Seconds * 1000d);
            }
            set
            {
                Seconds = (double)value / 1000d;
            }
        }
        public double Seconds
        {
            get
            {
                return (double)Ticks / (double)Frequency;
            }
            set
            {
                Ticks = (long)(value * (double)Frequency);
            }
        }
        public float Minutes
        {
            get
            {
                return (float)Seconds / 60f;
            }
            set
            {
                Seconds = (double)value * 60d;
            }
        }
        public static long Frequency
        {
            get
            {
                return Stopwatch.Frequency;
            }
        }
        #endregion

        #region Contructors/Destructors
        private Time(long TotalTicks)
        {
            _elapsedticks = TotalTicks;
        }
        public static Time FromTicks(long Ticks)
        {
            return new Time(Ticks);
        }
        public static Time FromMilliseconds(long Milliseconds)
        {
            return FromSeconds((double)Milliseconds / 1000d);
        }
        public static Time FromSeconds(double Seconds)
        {
            return FromTicks((long)(Seconds * (double)Frequency));
        }
        public static Time FromMinutes(float Minutes)
        {
            return FromSeconds((double)Minutes * 60d);
        }
        #endregion

        #region Functions
        public override bool Equals(object obj)
        {
            if (!(obj is Time)) return false;
            else return ((Time)obj)._elapsedticks == _elapsedticks;
        }
        #endregion

        #region Operators
        public static bool operator ==(Time a, Time b)
        {
            return (a._elapsedticks == b._elapsedticks);
        }
        public static bool operator !=(Time a, Time b)
        {
            return (a._elapsedticks != b._elapsedticks);
        }
        public static bool operator <(Time a, Time b)
        {
            return (a._elapsedticks < b._elapsedticks);
        }
        public static bool operator >(Time a, Time b)
        {
            return (a._elapsedticks > b._elapsedticks);
        }
        public static bool operator <=(Time a, Time b)
        {
            return (a._elapsedticks <= b._elapsedticks);
        }
        public static bool operator >=(Time a, Time b)
        {
            return (a._elapsedticks >= b._elapsedticks);
        }
        public static Time operator -(Time a)
        {
            return new Time(-a._elapsedticks);
        }
        public static Time operator +(Time a, Time b)
        {
            return new Time(a._elapsedticks + b._elapsedticks);
        }
        public static Time operator -(Time a, Time b)
        {
            return new Time(a._elapsedticks - b._elapsedticks);
        }
        public static Time operator *(Time a, Time b)
        {
            return new Time(a._elapsedticks * b._elapsedticks);
        }
        public static Time operator *(Time a, long b)
        {
            return new Time(a._elapsedticks * b);
        }
        public static Time operator *(Time a, double b)
        {
            return new Time((long)((double)a._elapsedticks * b));
        }
        public static Time operator *(Time a, float b)
        {
            return new Time((long)((double)a._elapsedticks * (double)b));
        }
        public static Time operator *(Time a, int b)
        {
            return new Time((long)((double)a._elapsedticks * (double)b));
        }
        public static Time operator /(Time a, Time b)
        {
            return new Time(a._elapsedticks / b._elapsedticks);
        }
        public static Time operator /(Time a, long b)
        {
            return new Time(a._elapsedticks / b);
        }
        public static Time operator /(Time a, double b)
        {
            return new Time((long)((double)a._elapsedticks / b));
        }
        public static Time operator /(Time a, float b)
        {
            return new Time((long)((double)a._elapsedticks / (double)b));
        }
        public static Time operator /(Time a, int b)
        {
            return new Time((long)((double)a._elapsedticks / (double)b));
        }
        #endregion
    }
}