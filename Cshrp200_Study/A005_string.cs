using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cshrp200_Study
{
    class A005_string
    {
        static void Main1(string[] args)
        {
            string a = "hello";
            string b = "h";

            b = b + "ello";
            Console.WriteLine(a == b);      //True 출력
            Console.WriteLine("b = " + b);

            int x = 10;
            string c = b + '!' + " " + x;

            Console.WriteLine("c = " + c);
        }
    }
}
