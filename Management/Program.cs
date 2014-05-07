using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conform.Management;
namespace Conform
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                GameApplication game = new GameApplication();
                game.run();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error has occured in the program");
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
