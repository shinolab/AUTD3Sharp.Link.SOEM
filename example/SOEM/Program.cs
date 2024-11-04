using AUTD3Sharp;
using AUTD3Sharp.Gain;
using AUTD3Sharp.Modulation;
using AUTD3Sharp.Utils;
using AUTD3Sharp.Link;
using static AUTD3Sharp.Units;

System.Environment.SetEnvironmentVariable("RUST_LOG", "autd3=INFO");
Tracing.Init();

using var autd = Controller.Builder([new AUTD3(Vector3.Zero)])
    .Open(SOEM.Builder()
        .WithErrHandler((slave, status) =>
        {
            Console.Error.WriteLine($"slave [{slave}]: {status}");
            if (status == Status.Lost)
                // You can also wait for the link to recover, without exiting the process
                Environment.Exit(-1);
        }));

autd.Send((new Sine(150f * Hz), new Focus(autd.Center + new Vector3(0f, 0f, 150f))));

Console.ReadKey(true);

autd.Close();
