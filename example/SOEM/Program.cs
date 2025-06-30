using AUTD3Sharp;
using AUTD3Sharp.Gain;
using AUTD3Sharp.Modulation;
using AUTD3Sharp.Utils;
using AUTD3Sharp.Link;
using static AUTD3Sharp.Units;

System.Environment.SetEnvironmentVariable("RUST_LOG", "autd3=INFO");
SOEM.Tracing.Init();

using var autd = Controller.Open([new AUTD3(pos: Point3.Origin, rot: Quaternion.Identity)], new SOEM(
        (slave, status) =>
        {
                Console.Error.WriteLine($"slave [{slave}]: {status}");
                if (status == Status.Lost)
                        // You can also wait for the link to recover, without exiting the process
                        System.Environment.Exit(-1);
        }, option: new SOEMOption()));

autd.Send((new Sine(freq: 150f * Hz, option: new SineOption()), new Focus(pos: autd.Center() + new Vector3(0f, 0f, 150f), option: new FocusOption())));

Console.ReadKey(true);

autd.Close();
