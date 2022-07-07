using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cshrp200_Study
{
    class Assignment
    {
        static void Mai1(string[] args)
        {
            int i;      //4bite
            double x;   //8bite

            i = 5;
            x = 3.141592;
            Console.WriteLine("i = " + i + ",x = " + x);

            x = i;  //암시적 형변환
            i = (int)x; //캐스트가 필요
            Console.WriteLine("i = " + i + ",x = " + x);
        }
    }
}
