using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivaldi.Resources;

namespace Vivaldi.Models.Encrypt
{
    public class AES
    {
        public static string Key
        {
            get { return Resource.Key; }
        }

        public static string Iv
        {
            get { return Resource.Iv; }
        }
    }
}
