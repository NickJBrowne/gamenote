using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers.UnitTest
{
    public class When<TContext>
    {
        public When<TContext> And => this;
        public TContext Context { get; private set; }
        public When(TContext context)
        {
            this.Context = context;
        }

        public Then<TContext> Then()
            => new Then<TContext>(Context);
    }
}
