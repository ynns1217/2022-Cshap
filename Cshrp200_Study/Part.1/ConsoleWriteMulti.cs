using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cshrp200_Study
{
    class ConsoleWriteMulti
    {
        static void Main1(string[] args)
        {
            Console.WriteLine("10이하의 소수 : {0}, {1},{2},{3}", 2, 3, 5, 7);

            string primes;
            //String.Format() 메소드를 이용하여 Console.WriteLine()과 같은 방법으로 문자열을 만들 수 있다.
            primes = String.Format("10이하의 소수: {0},{1},{2},{3}", 2, 4, 5, 7);
            Console.WriteLine(primes);

        }
    }
}
