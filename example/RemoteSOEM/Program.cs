using System.Net;
using AUTD3Sharp;
using AUTD3Sharp.Gain;
using AUTD3Sharp.Modulation;
using AUTD3Sharp.Utils;
using AUTD3Sharp.Link;
using static AUTD3Sharp.Units;

using var autd = Controller.Open([new AUTD3(pos: Point3.Origin, rot: Quaternion.Identity)], new RemoteSOEM(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080)));

autd.Send((new Sine(freq: 150f * Hz, option: new SineOption()), new Focus(pos: autd.Center() + new Vector3(0f, 0f, 150f), option: new FocusOption())));

Console.ReadKey(true);

autd.Close();
