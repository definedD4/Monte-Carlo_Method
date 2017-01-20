using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Exceptions
{

    [Serializable]
    public class TableLoadException : Exception
    {
        public TableLoadException() { }
        public TableLoadException(string message) : base(message) { }
        public TableLoadException(string message, Exception inner) : base(message, inner) { }
        protected TableLoadException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
