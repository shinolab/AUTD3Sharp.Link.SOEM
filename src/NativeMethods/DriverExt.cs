using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("tests")]
namespace AUTD3Sharp
{
    namespace NativeMethods
    {
        public enum Status : byte
        {
            Error = 0,
            Lost = 1,
            StateChanged = 2
        }
    }

    namespace Link
    {
        public enum SyncMode : byte
        {
            DC = 0,
            FreeRun = 1
        }

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
            Realtime = 5
        }
    }
}
