using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChartAndGraph
{
    interface IInternalPieData
    {
        ChartSparseDataSource InternalDataSource { get; }
        void Update();
        void OnBeforeSerialize();
        void OnAfterDeserialize();
    }
}
