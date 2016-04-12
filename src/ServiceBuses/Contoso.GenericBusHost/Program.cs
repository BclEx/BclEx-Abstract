using CommandLine;
using Common.Logging;
using Contoso.GenericBusHost.Actions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Contoso.GenericBusHost
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        static readonly IDictionary<Action, IAction> _actions = new Dictionary<Action, IAction>
        {
            {Action.Debug, new DebugAction()},
            {Action.Server, new ServerAction()},
            {Action.Install, new InstallAction()},
            {Action.Uninstall, new UninstallAction()},
            {Action.Deploy, new DeployAction()}
        };
        static ILog _log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            var executingOptions = new ExecutingOptions();
            if (!Parser.ParseArguments(args, executingOptions))
            {
                Console.WriteLine("Invalid arguments:");
                Console.WriteLine("\t{0}",
                    string.Join(" ", args));
                Console.WriteLine();
                Console.WriteLine(Parser.ArgumentsUsage(typeof(ExecutingOptions)));
                return 1;
            }

            var action = (executingOptions.Action == Action.None ? (Environment.UserInteractive ? Action.Debug : Action.Server) : executingOptions.Action);
            executingOptions.Name = executingOptions.Name ?? Path.GetFileNameWithoutExtension(executingOptions.Assembly);
            try
            {
                _log.Debug("Executing action: " + action);
                _actions[action].Execute(executingOptions);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _log.Fatal("Host has crashed because of an error", e);
                // want to put the error in the error log
                if (action == Action.Server)
                    throw;
                return 2;
            }
        }
    }
}