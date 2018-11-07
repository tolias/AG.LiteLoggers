using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Loggers
{
    public interface IStringAppender
    {
        void Append(string str);
    }
}
