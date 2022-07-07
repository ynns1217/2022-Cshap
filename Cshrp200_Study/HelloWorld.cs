using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cshrp200_Study
{
    //콘솔에 Hello World!라고 출력
    class HelloWorld
    {
        static void Main1(string[] args)
        {
            Console.Write("Hello");     //줄바꿈 없음
            Console.WriteLine("World!");        //줄바꿈
            Console.Write("이름을 입력하세요 : ");      //줄바꿈 없이 출력

            string name = Console.ReadLine();       //이름을 입력받습니다.
            Console.Write(name);
            Console.WriteLine("님!");
        }


    }
}
