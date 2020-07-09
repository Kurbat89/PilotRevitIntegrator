using System.ServiceProcess;

namespace PilotRevitShareListenerCore
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
               // new ShareListenerService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}