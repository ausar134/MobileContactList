using System;
using System.Collections.Generic;
using System.Text;

namespace ContactBook 
{
   public static class ExtensionMethods
    {
        public static bool IsNotValid(this string source)
        {
            return false;
        }

        public static bool NotValidMessageError(this string source)
        {
            return source.NotValidMessageError();
        }
    }
}
