using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stut6.Service
{
    public class MyException:Exception
    {
        public MyException(string message) : base(message)
        {
            Console.WriteLine(message);
        }
    }
}
