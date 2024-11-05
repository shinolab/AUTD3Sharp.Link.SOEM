namespace AUTD3Sharp
{
    namespace Link
    {
        public enum TimerStrategy : byte
        {
            SpinSleep = 0,
            StdSleep = 1,
            SpinWait = 2
        }

        public enum ProcessPriority : byte
        {
            Idle = 0,
            BelowNormal = 1,
            Normal = 2,
            AboveNormal = 3,
            High = 4,
            Realtime = 5,
        }
    }
}
