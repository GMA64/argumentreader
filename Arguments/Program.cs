using ArgumentMarshalerLib;
using ArgumentsLib;
using System;

namespace Argument
{
    class Program
    {
        static void Main(string[] args)
        {
            Arguments parameter;

            try
            {
                parameter = new Arguments(@".\Marshaler", "bool,booltwo,int#,double##,string*", args);
                Console.WriteLine("Arguments");
            }
            catch (ArgumentsException ex)
            {
                Console.WriteLine(ex.ErrorMessage());
                return;
            }

            Console.WriteLine(parameter.GetValue<bool>("bool"));
            Console.WriteLine(parameter.GetValue<bool>("booltwo"));
            Console.WriteLine(parameter.GetValue<int>("int"));
            Console.WriteLine(parameter.GetValue<double>("double"));
            Console.WriteLine(parameter.GetValue<string>("string"));

            Console.ReadKey();
        }
    }
}