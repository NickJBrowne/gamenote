using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers.UnitTest
{
    public class Given<TContext>
    {
        public Given<TContext> And => this;

        public TContext Context { get; private set; }

        public Given(TContext context)
        {
            Context = context;
        }

        public When<TContext> When()
            => new When<TContext>(Context);
    }
}