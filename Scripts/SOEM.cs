using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AUTD3Sharp.NativeMethods;
using AUTD3Sharp.Driver.FPGA.Common;

#if UNITY_2020_2_OR_NEWER
#nullable enable
#endif

namespace AUTD3Sharp.Link
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false,
        ThrowOnUnmappableChar = true)]
    internal delegate void ErrHandlerDelegate(IntPtr context, uint slave, NativeMethods.Status status);

    public class SOEMOption
    {
        public string Ifname { get; init; } = string.Empty;
        public uint BufSize { get; init; } = 32;
        public Duration SendCycle { get; init; } = Duration.FromMillis(1);
        public Duration Sync0Cycle { get; init; } = Duration.FromMillis(1);
        public TimerStrategy TimerStrategy { get; init; } = TimerStrategy.SpinSleep;
        public SyncMode SyncMode { get; init; } = SyncMode.DC;
        public Duration SyncTolerance { get; init; } = Duration.FromMicros(1);
        public Duration SyncTimeout { get; init; } = Duration.FromSecs(10);
        public Duration StateCheckInterval { get; init; } = Duration.FromMillis(100);
        public ThreadPriority ThreadPriority { get; init; } = ThreadPriority.Max;
        public ProcessPriority ProcessPriority { get; init; } = ProcessPriority.High;

        internal NativeMethods.SOEMOption ToNative()
        {
            unsafe
            {
                var ifnameBytes = Ffi.ToNullTerminatedUtf8(Ifname);
                fixed (byte* pIfname = &ifnameBytes[0])
                {
                    return new NativeMethods.SOEMOption
                    {
                        ifname = pIfname,
                        buf_size = BufSize,
                        send_cycle = SendCycle,
                        sync0_cycle = Sync0Cycle,
                        timer_strategy = TimerStrategy,
                        sync_mode = SyncMode,
                        sync_tolerance = SyncTolerance,
                        sync_timeout = SyncTimeout,
                        state_check_interval = StateCheckInterval,
                        thread_priority = ThreadPriority.Ptr,
                        process_priority = ProcessPriority
                    };
                }
            }
        }
    }

    public sealed class SOEM : Driver.Link
    {
        private readonly ErrHandlerDelegate _errHandler;
        private readonly SOEMOption _option;

        [ExcludeFromCodeCoverage]
        public SOEM(Action<int, Status> errHandler, SOEMOption option)
        {
            _errHandler = (_, slave, status) =>
            {
                var msgBytes = new byte[128];
                unsafe
                {
#pragma warning disable CA1806
                    fixed (byte* p = &msgBytes[0]) NativeMethodsLinkSOEM.AUTDLinkSOEMStatusGetMsg(status, p);
#pragma warning restore CA1806
                }
                errHandler((int)slave, new Status(status, System.Text.Encoding.UTF8.GetString(msgBytes).TrimEnd('\0')));
            };
            _option = option;
        }

        [ExcludeFromCodeCoverage]
        public override LinkPtr Resolve() => NativeMethodsLinkSOEM.AUTDLinkSOEM(
                            new ConstPtr { Item1 = Marshal.GetFunctionPointerForDelegate(_errHandler) },
                            new ConstPtr { Item1 = IntPtr.Zero },
                            _option.ToNative()).Validate();

        [ExcludeFromCodeCoverage]
        private static EtherCATAdapter GetAdapter(EthernetAdaptersPtr handle, uint i)
        {
            unsafe
            {
                var sbDesc = new byte[128];
                var sbName = new byte[128];
                fixed (byte* dp = &sbDesc[0])
                fixed (byte* np = &sbName[0]) NativeMethodsLinkSOEM.AUTDAdapterGetAdapter(handle, i, dp, np);
                return new EtherCATAdapter(System.Text.Encoding.UTF8.GetString(sbDesc), System.Text.Encoding.UTF8.GetString(sbName));
            }
        }

        [ExcludeFromCodeCoverage]
        public static IEnumerable<EtherCATAdapter> EnumerateAdapters()
        {
            var handle = NativeMethodsLinkSOEM.AUTDAdapterPointer();
            var len = NativeMethodsLinkSOEM.AUTDAdapterGetSize(handle);
            for (uint i = 0; i < len; i++) yield return GetAdapter(handle, i);
            NativeMethodsLinkSOEM.AUTDAdapterPointerDelete(handle);
        }

        public static class Tracing
        {
#if UNITY_2020_2_OR_NEWER
            public static void Init(string path)
            {
                AUTD3Sharp.Tracing.ExtTracing += (pathBytes) => 
                {
                    unsafe
                    {
                        fixed (byte* pathPtr = &pathBytes[0])
                        {
                            NativeMethodsLinkSOEM.AUTDLinkSOEMTracingInitWithFile(pathPtr);
                        }
                    }
                };
            }
#else
#pragma warning disable CA2255
            [ModuleInitializer]
            public static void Init()
            {
                AUTD3Sharp.Tracing.ExtTracing += NativeMethodsLinkSOEM.AUTDLinkSOEMTracingInit;
            }
#pragma warning restore CA2255
#endif
        }
    }

    public sealed class RemoteSOEM : Driver.Link
    {
        private readonly IPEndPoint _ip;

        public RemoteSOEM(IPEndPoint ip)
        {
            _ip = ip;
        }

        [ExcludeFromCodeCoverage]
        public override LinkPtr Resolve()
        {
            var ipBytes = Ffi.ToNullTerminatedUtf8(_ip.Address.ToString());
            unsafe
            {
                fixed (byte* ipPtr = &ipBytes[0])
                    return NativeMethodsLinkSOEM.AUTDLinkRemoteSOEM(ipPtr).Validate();
            }
        }
    }

    [ExcludeFromCodeCoverage]
    public readonly struct EtherCATAdapter : IEquatable<EtherCATAdapter>
    {
        public string Desc { get; }
        public string Name { get; }

        internal EtherCATAdapter(string desc, string name)
        {
            Desc = desc;
            Name = name;
        }

        public override string ToString() => $"{Desc}, {Name}";
        public bool Equals(EtherCATAdapter other) => Desc.Equals(other.Desc) && Name.Equals(other.Name);
        public static bool operator ==(EtherCATAdapter left, EtherCATAdapter right) => left.Equals(right);
        public static bool operator !=(EtherCATAdapter left, EtherCATAdapter right) => !left.Equals(right);
        public override bool Equals(object? obj) => obj is EtherCATAdapter adapter && Equals(adapter);
        public override int GetHashCode() => Desc.GetHashCode() ^ Name.GetHashCode();
    }

    public class ThreadPriority
    {
        internal readonly ThreadPriorityPtr Ptr;

        private ThreadPriority(ThreadPriorityPtr ptr)
        {
            Ptr = ptr;
        }

        public static ThreadPriority Min = new(NativeMethodsLinkSOEM.AUTDLinkSOEMThreadPriorityMin());
        public static ThreadPriority Max = new(NativeMethodsLinkSOEM.AUTDLinkSOEMThreadPriorityMax());

        public static ThreadPriority Crossplatform(byte value)
        {
            if (value > 99) throw new ArgumentOutOfRangeException(nameof(value), "value must be between 0 and 99");
            return new ThreadPriority(NativeMethodsLinkSOEM.AUTDLinkSOEMThreadPriorityCrossplatform(value));
        }
    }

    public class Status : IEquatable<Status>
    {
        private readonly NativeMethods.Status _inner;
        private readonly string _msg;

        public Status(NativeMethods.Status status, string msg)
        {
            _inner = status;
            _msg = msg;
        }

        public static Status Lost => new(NativeMethods.Status.Lost, "");
        public static Status StateChanged => new(NativeMethods.Status.StateChanged, "");
        public static Status Error => new(NativeMethods.Status.Error, "");

        public override string ToString() => _msg;

        public static bool operator ==(Status left, Status right) => left.Equals(right);
        public static bool operator !=(Status left, Status right) => !left.Equals(right);
        public bool Equals(Status? other) => other is not null && _inner.Equals(other._inner);
        public override bool Equals(object? obj) => obj is Status other && Equals(other);
        [ExcludeFromCodeCoverage] public override int GetHashCode() => _inner.GetHashCode();
    }
}

#if UNITY_2020_2_OR_NEWER
#nullable restore
#endif
