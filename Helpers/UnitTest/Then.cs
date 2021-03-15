using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers.UnitTest
{
    public class Then<TContext>
    {
        public Then<TContext> And => this;
        public TContext Context { get; private set; }

        public Then(TContext context)
        {
            this.Context = context;
        }
    }
}
