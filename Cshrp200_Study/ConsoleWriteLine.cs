using System;

namespace Cshrp200_Study
{
    class ConsoleWriteLine
    {
        //Console.Write() / Console.WriteLine() 메소드는 하나의 변수나 값을 
        //출력할 때는 어떤 값이라도 출력이 가능합니다.
        static void Main1(string[] args)
        {
            bool b = true;
            char c = 'A';
            decimal d = 1.234m; //m는 decimal 형의 접미사
            double e = 1.23456779;
            float f = 1.23456789f; //f는 float 형의 접미사
            int i = 1234;
            string s = "Hello";

            Console.WriteLine(b);
            Console.WriteLine(c);
            Console.WriteLine(d);
            Console.WriteLine(e);
            Console.WriteLine(f);
            Console.WriteLine(i);
            Console.WriteLine(s);
        }
    }
}
