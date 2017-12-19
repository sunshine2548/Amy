using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Common
{
    public class CreateOrderRef
    {
        public static string Create()
        {
            return DateTime.Now.Ticks + new Random().Next(100000, 999999).ToString();
        }
    }
}
