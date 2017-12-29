using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Common
{
    public class Guard
    {
        public static string EnsureParam(string arg)
        {
            arg = arg?.Trim();
            return string.IsNullOrWhiteSpace(arg) ? string.Empty : arg;
        }
    }
}
