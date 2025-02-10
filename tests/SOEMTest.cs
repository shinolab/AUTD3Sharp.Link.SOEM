using System.Net;

namespace tests;

public class SOEMTest
{
    [Fact, Trait("require", "soem")]
    public void TestThreadPriority()
    {
        _ = AUTD3Sharp.Link.ThreadPriority.Max;
        _ = AUTD3Sharp.Link.ThreadPriority.Min;
        _ = AUTD3Sharp.Link.ThreadPriority.Crossplatform(0);
        _ = AUTD3Sharp.Link.ThreadPriority.Crossplatform(99);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = AUTD3Sharp.Link.ThreadPriority.Crossplatform(100));
    }

    [Fact, Trait("require", "soem")]
    public void TestStatus()
    {
        var lost = Status.Lost;
        var stateChanged = Status.StateChanged;
        var error = Status.Error;

        Assert.True(Status.Lost == lost);
        Assert.True(Status.StateChanged != lost);
        Assert.True(Status.Error != lost);
        Assert.False(lost.Equals(null));
        Assert.True(lost.Equals((object?)lost));
        Assert.False(lost.Equals((object?)null));

        Assert.True(Status.Error == error);
        Assert.True(Status.Lost != error);
        Assert.True(Status.StateChanged != error);

        Assert.True(Status.Error != stateChanged);
        Assert.True(Status.Lost != stateChanged);
        Assert.True(Status.StateChanged == stateChanged);

        Assert.Equal("", lost.ToString());
    }

    [Fact, Trait("require", "soem")]
    public void TestSOEMOption()
    {
        Assert.True(AUTD3Sharp.NativeMethods.NativeMethodsLinkSOEM.AUTDLinkSOEMIsDefault(new SOEMOption().ToNative()));
    }

    [Fact, Trait("require", "soem")]
    public void TestSOEM()
    {
        _ = new SOEM((_, _) => { }, new SOEMOption());
    }

    [Fact, Trait("require", "soem")]
    public void TestRemoteSOEM()
    {
        _ = new RemoteSOEM(new IPEndPoint(IPAddress.Parse("172.0.0.1"), 8080));
    }
}