using GGM.Application;
using System.IO;

namespace DemoService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GGMApplication.Run(
                applicationClass: typeof(Program)
              , configPath: Path.Combine(Directory.GetCurrentDirectory(), "../../application.cfg")
              , args: args
              , serviceTypes: typeof(DemoService.Service.DemoService));

        }
    }
}