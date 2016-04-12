using Common.Logging;
using System;

namespace Contoso.GenericBusHost.Actions
{
    /// <summary>
    /// DebugAction
    /// </summary>
    public class DebugAction : IAction
    {
        static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void Execute(ExecutingOptions options)
        {
            var host = new Host();
            host.SetArguments(options);
            try
            {
                host.DebugStart(new string[0]);
                var keepGoing = true;
                while (keepGoing)
                {
                    Console.WriteLine("Enter 'cls' to clear the screen, 'q' to exit");
                    var op = (Console.ReadLine() ?? string.Empty);
                    switch (op.ToLowerInvariant())
                    {
                        case "q":
                            keepGoing = false;
                            break;
                        case "cls":
                            Console.Clear();
                            break;
                    }

                }
                host.Stop();
            }
            catch (Exception e)
            {
                Log.Fatal("Host has crashed", e);
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }
}