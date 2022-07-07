using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cshrp200_Study
{
    class VariablesAndWrite
    {
        static void Main1(string[] args)
        {
            int v1 = 100;
            double v2 = 1.234;

            //Console.WriteLine(v1,v2); -> 에러 발생
            //1. ToString() 문자열로 바꾸어 연결하여 출력 -> 문자열 하나를 출력하는 셈
            Console.WriteLine(v1.ToString() + "," + v2.ToString());
            //2. 문자열과 숫자를 + 로 연결하여 하나의 문자열 출력
            Console.WriteLine("v1 =" + v1 + "v2=" + v2);
            //3. 형식문자열 사용
            Console.WriteLine("v1 = {0},v2 = {1}", v1, v2);
            //4. 형식문자열 앞에 $를 사용하여 {}안에 변수명 직접 쓰기
            Console.WriteLine($"v1 = {v1},v2 = {v2}");
        }
    }
}
