using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ChartAndGraph
{
    public class LegenedData
    {
        public class LegenedItem
        {
            public string Name;
            public Material Material;
        }

        List<LegenedItem> mItems = new List<LegenedItem>();

        public void AddLegenedItem(LegenedItem item)
        {
            mItems.Add(item);
        }

        public IEnumerable<LegenedItem> Items
        {
            get { return mItems; }
        }
    }
}
