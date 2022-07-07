using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//이름, 나이, 키를 입력하고 출력하는 프로그램
namespace Cshrp200_Study
{
    class A004_Variable
    {
        static void Main1(string[] args)
        {
            Console.Write("이름을 입력하세요: ");
            string name = Console.ReadLine();
            Console.Write("나이를 입력하세요: ");
            int age = int.Parse(Console.ReadLine());//입력받은 문자열을 정수로 할당
            Console.Write("키를 입력하세요(cm):");
            float height = float.Parse(Console.ReadLine()); //입력받은 문자열을 float로 바꾸어 할당

            Console.Write("안녕하세요, ");
            Console.Write(name);
            Console.WriteLine("님!");

            Console.Write("나이는 ");
            Console.Write(age);
            Console.Write("세, 키는 ");
            Console.Write(height);
            Console.Write("cm 이군뇨!");

        }
    }
}
