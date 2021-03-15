using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers.UnitTest
{
    public abstract class BaseGiven<T>
        where T : class, new()
    {
        protected virtual Given<T> Given()
            => new Given<T>(new T());
    }
}