using System.Diagnostics;
using System.Linq;

namespace Globe.Platform.Host.Runners
{
    public static class RunnerFactory
    {
        public static IRunner Create(string[] args)
        {
            bool isConsoleRunnable = args.Contains("--console");
            bool isWindowsServiceRunnable = args.Contains("--service") && !Debugger.IsAttached;

            Logger.Log(string.Format("Console Runnable {0}, WindowsService Runnable {1}", isConsoleRunnable, isWindowsServiceRunnable));

            BaseService service;

            //if (isConsoleRunnable)
                service = new ConsoleService();
            //else if (isWindowsServiceRunnable)
            //    service = new WindowsService();
            //else
            //    service = new IISService();

            (service as IBuilder).Build();

            return service;
        }
    }
}
