﻿// <auto-generated/>

#if UNITY_2020_2_OR_NEWER
#nullable enable
#endif

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AUTD3Sharp.Link {
    public partial class SOEMBuilder
    {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithIfname(string value)
        {
            Ifname = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithBufSize(uint value)
        {
            BufSize = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithSendCycle(global::AUTD3Sharp.Duration value)
        {
            SendCycle = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithSync0Cycle(global::AUTD3Sharp.Duration value)
        {
            Sync0Cycle = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithTimerStrategy(global::AUTD3Sharp.Link.TimerStrategy value)
        {
            TimerStrategy = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithSyncMode(global::AUTD3Sharp.SyncMode value)
        {
            SyncMode = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithSyncTolerance(global::AUTD3Sharp.Duration value)
        {
            SyncTolerance = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithSyncTimeout(global::AUTD3Sharp.Duration value)
        {
            SyncTimeout = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithStateCheckInterval(global::AUTD3Sharp.Duration value)
        {
            StateCheckInterval = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithThreadPriority(global::AUTD3Sharp.NativeMethods.ThreadPriorityPtr value)
        {
            ThreadPriority = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithProcessPriority(global::AUTD3Sharp.Link.ProcessPriority value)
        {
            ProcessPriority = value;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage]
        public SOEMBuilder WithErrHandler(global::System.Action<int, global::AUTD3Sharp.Link.Status> value)
        {
            ErrHandler = value;
            return this;
        }
    }   
}