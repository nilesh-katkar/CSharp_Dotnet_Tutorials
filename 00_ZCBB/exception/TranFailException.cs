using System;

namespace _00_ZCBB.Exceptions
{
    public class TranFailException : Exception
    {
        public TranFailException(string message) : base(message) { }
    }
}
