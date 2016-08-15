using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = string.Empty;
            ActiveDirectory.IdllActiveDirectory obj = new ActiveDirectory.DllActiveDirectory();

            code = obj.UnlockUser("1515899","252525" );
            Console.WriteLine("ques trae code: {0}", code);
             Console.ReadKey();
        }
    }
}
