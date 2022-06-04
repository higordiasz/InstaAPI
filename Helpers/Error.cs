using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI.Helpers.Errors
{
    public static class Error
    {
        public static void ErrWrite(this Instagram insta, Exception err, string methodname)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("Err Message: " + err.Message);
            Console.WriteLine("Err Stack: " + err.StackTrace);
            Console.WriteLine("Err Source: " + err.Source);
            Console.WriteLine("Err Data: " + err.Data);
            Console.WriteLine("Err Method: " + methodname);
            Console.WriteLine("\n\n");
        }
    }
}
