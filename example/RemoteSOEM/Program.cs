using System.Net;
using AUTD3Sharp;
using AUTD3Sharp.Gain;
using AUTD3Sharp.Modulation;
using AUTD3Sharp.Utils;
using AUTD3Sharp.Link;
using static AUTD3Sharp.Units;

using var autd = Controller.Builder([new AUTD3(Point3.Origin)])
    .Open(RemoteSOEM.Builder(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080)));


autd.Send((new Sine(150f * Hz), new Focus(autd.Center + new Vector3(0f, 0f, 150f))));

Console.ReadKey(true);

autd.Close();
