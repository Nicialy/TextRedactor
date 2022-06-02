using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arhiv
{
    public class ArhivException: Exception
    {
        
        public ArhivException(string message)
            : base(message) { }

        public ArhivException(string message, Exception inner)
            : base(message, inner) { }
    }
}
