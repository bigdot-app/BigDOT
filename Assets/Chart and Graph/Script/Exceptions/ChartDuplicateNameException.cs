using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChartAndGraph.Exceptions
{
    class ChartDuplicateItemException : ChartException
    {
        public ChartDuplicateItemException(String message)
            : base(message)
        {

        }
    }
}
