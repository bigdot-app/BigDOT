//using ChartAndGraph;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace ChartAndGraph
//{
//    [Serializable]
//    class CandleChartData : IInternalCandleData
//    {
//        private event EventHandler DataChanged;
//        private bool mSuspendEvents = false;

//        [Serializable]
//        public struct CandleValue
//        {
//            public float Open;
//            public float High;
//            public float Low;
//            public float Close;
//            public DateTime Start;
//            public TimeSpan Duration;

//            public float Max
//            {
//                get
//                {
//                    return Mathf.Max(Open, Close);
//                }
//            }
//            public float Min
//            {
//                get
//                {
//                    return Mathf.Min(Open, Close);
//                }
//            }
//        }

//        [Serializable]
//        public struct CandleSettings
//        {
//            public float LineThickness;
//            public float CandleThickness;
//            public float OutlineThickness;
//            public Material Outline;
//            public Material Line;
//            public Material Fill;
//            public GameObject CandlePrefab;
//        }

//        [Serializable]
//        public class CategoryData
//        {
//            public string Name;
//            public List<CandleValue> Data = new List<CandleValue>();
//            public float? MaxX, MaxY, MinX, MinY;
//            public CandleSettings UpCandle = new CandleSettings();
//            public CandleSettings DownCandle = new CandleSettings();
//            public float Depth = 0f;
//        }

//        class CandleComparer : IComparer<CandleValue>
//        {
//            public int Compare(CandleValue x, CandleValue y)
//            {
//                if (x.Open < y.Open)
//                    return -1;
//                if (x.Open > y.Open)
//                    return 1;
//                return 0;

//            }
//        }

//        [Serializable]
//        class SerializedCategory
//        {
//            public string Name;
//            public CandleValue[] Data;
//            public float? MaxX, MaxY, MinX, MinY;
//            public CandleSettings UpCandle = new CandleSettings();
//            public CandleSettings DownCandle = new CandleSettings();
//            public float Depth = 0f;

//        }

//        CandleComparer mComparer = new CandleComparer();
//        Dictionary<string, CategoryData> mData = new Dictionary<string, CategoryData>();

//        [SerializeField]
//        SerializedCategory[] mSerializedData = new SerializedCategory[0];
//        private void RaiseDataChanged()
//        {
//            if (mSuspendEvents)
//                return;
//            if (DataChanged != null)
//                DataChanged(this, EventArgs.Empty);
//        }
//        /// <summary>
//        /// call this to suspend chart redrawing while updating the data of the chart
//        /// </summary>
//        public void StartBatch()
//        {
//            mSuspendEvents = true;
//        }
//        /// <summary>
//        /// call this after StartBatch , this will apply all the changed made between the StartBatch call to this call
//        /// </summary>
//        public void EndBatch()
//        {
//            mSuspendEvents = false;
//            RaiseDataChanged();
//        }

//        event EventHandler IInternalCandleData.InternalDataChanged
//        {
//            add
//            {
//                DataChanged += value;
//            }

//            remove
//            {
//                DataChanged -= value;
//            }
//        }

//        /// <summary>
//        /// rename a category. throws and exception on error
//        /// </summary>
//        /// <param name="prevName"></param>
//        /// <param name="newName"></param>
//        public void RenameCategory(string prevName, string newName)
//        {
//            if (prevName == newName)
//                return;
//            if (mData.ContainsKey(newName))
//                throw new ArgumentException(String.Format("A category named {0} already exists", newName));
//            CategoryData cat = mData[prevName];
//            mData.Remove(prevName);
//            cat.Name = newName;
//            mData.Add(newName, cat);
//            RaiseDataChanged();
//        }

//        /// <summary>
//        /// Adds a new category to the candle chart.
//        /// </summary>
//        /// <param name="category"></param>
//        /// <param name="material"></param>
//        /// <param name="innerFill"></param>
//        public void AddCategory(string category,CandleSettings up, CandleSettings down,float depth)
//        {
//            AddCategory3DCandle(category, up, down, 0f);
//        }

//        /// <summary>
//        /// add category to the candle chart
//        /// </summary>
//        /// <param name="category"></param>
//        /// <param name="up"></param>
//        /// <param name="down"></param>
//        /// <param name="depth"></param>
//        public void AddCategory3DCandle(string category, CandleSettings up, CandleSettings down, float depth)
//        {
//            if (mData.ContainsKey(category))
//                throw new ArgumentException(String.Format("A category named {0} already exists", category));
//            CategoryData data = new CategoryData();
//            mData.Add(category, data);
//            data.Name = category;
//            data.DownCandle = down;
//            data.UpCandle = up;
//            data.Depth = depth;
//            RaiseDataChanged();
//        }


//        /// <summary>
//        /// clears all candle data
//        /// </summary>
//        public void Clear()
//        {
//            mData.Clear();
//        }

//        /// <summary>
//        /// removed a category from the DataSource. returnes true on success , or false if the category does not exist
//        /// </summary>
//        /// <param name="category"></param>
//        /// <returns></returns>
//        public bool RemoveCategory(string category)
//        {
//            return mData.Remove(category);
//        }

//        public void SetDownCandle(string category,CandleSettings down)
//        {
//            if (mData.ContainsKey(category) == false)
//            {
//                Debug.LogWarning("Invalid category name. Make sure the category is present in the chart");
//                return;
//            }
//            CategoryData data = mData[category];
//            data.DownCandle = down;
//            RaiseDataChanged();
//        }
//        public void SetUpCandle(string category,CandleSettings up)
//        {
//            if (mData.ContainsKey(category) == false)
//            {
//                Debug.LogWarning("Invalid category name. Make sure the category is present in the chart");
//                return;
//            }
//            CategoryData data = mData[category];
//            data.UpCandle = up;
//            RaiseDataChanged();
//        }


//        /// <summary>
//        /// sets the depth for a 3d graph category
//        /// </summary>
//        /// <param name="category"></param>
//        /// <param name="depth"></param>
//        public void Set3DCategoryDepth(string category, float depth)
//        {
//            if (mData.ContainsKey(category) == false)
//            {
//                Debug.LogWarning("Invalid category name. Make sure the category is present in the chart");
//                return;
//            }
//            if (depth < 0)
//                depth = 0f;
//            CategoryData data = mData[category];
//            data.Depth = depth;
//            RaiseDataChanged();
//        }


//        /// <summary>
//        /// clears all the data for the selected category
//        /// </summary>
//        /// <param name="category"></param>
//        public void ClearCategory(string category)
//        {
//            if (mData.ContainsKey(category) == false)
//            {
//                Debug.LogWarning("Invalid category name. Make sure the category is present in the chart");
//                return;
//            }
//            mData[category].MaxX = null;
//            mData[category].MaxY = null;
//            mData[category].MinX = null;
//            mData[category].MinY = null;
//            mData[category].Data.Clear();
//            RaiseDataChanged();
//        }

//        /// <summary> 
//        /// adds a candle to the category. having the point x,y values as dates
//        /// <param name="category"></param>
//        /// <param name="x"></param>
//        /// <param name="candle"></param>
//        public void AddCandleToCategory(string category, DateTime x, CandleValue candle
//        { 
//            double xVal = ChartDateUtility.DateToValue(x);
//            AddCandleToCategory(category, (float)xVal, candle);
//        }

//        /// <summary>
//        /// adds a point to the category. The points are sorted by their x value automatically
//        /// </summary>
//        /// <param name="category"></param>
//        /// <param name="point"></param>
//        public void AddCandleToCategory(string category, float x, CandleValue candle)
//        {
//            if (mData.ContainsKey(category) == false)
//            {
//                Debug.LogWarning("Invalid category name. Make sure the category is present in the chart");
//                return;
//            }
//            Vector2 point = new Vector2(x, y);
//            CategoryData data = mData[category];
//            List<CandleValue> candles = data.Data;

//            double start = ChartDateUtility.DateToValue(candle.Start);
//            double end = ChartDateUtility.DateToValue(candle.Start + candle.Duration);

//            float candleMax = Mathf.Max(Mathf.Max(candle.Open, candle.Close), Math.Max(candle.High, candle.Low));
//            float candleMin = Mathf.Min(Mathf.Min(candle.Open, candle.Close), Math.Min(candle.High, candle.Low));

//            if (data.MaxX.HasValue == false || data.MaxX.Value < end)
//                data.MaxX = (float)end;
//            if (data.MinX.HasValue == false || data.MinX.Value > start)
//                data.MinX = (float)start;
//            if (data.MaxY.HasValue == false || data.MaxY.Value < candleMax)
//                data.MaxY = candleMax;
//            if (data.MinY.HasValue == false || data.MinY.Value > candleMin)
//                data.MinY = candleMin;

//            if (candles.Count > 0)
//            {
//                if (candles[candles.Count - 1].Start < candle.Start)
//                {
//                    candles.Add(candle);
//                    return;
//                }
//            }

//            int search = candles.BinarySearch(candle, mComparer);
//            if (search < 0)
//                search = ~search;
//            candles.Insert(search, candle);
//            RaiseDataChanged();
//        }

//        double IInternalCandleData.GetMaxValue(int axis)
//        {
//            double? res = null;
//            foreach (CategoryData cat in mData.Values)
//            {
//                if (axis == 0)
//                {
//                    if (res.HasValue == false || res.Value < cat.MaxX)
//                        res = cat.MaxX;
//                }
//                else
//                {
//                    if (res.HasValue == false || res.Value < cat.MaxY)
//                        res = cat.MaxY;
//                }
//            }
//            if (res.HasValue == false)
//                return (ChartCommon.IsInEditMode) ? 10.0 : 0.0;
//            return res.Value;
//        }

//        double IInternalCandleData.GetMinValue(int axis)
//        {
//            double? res = null;
//            foreach (CategoryData cat in mData.Values)
//            {
//                if (axis == 0)
//                {
//                    if (res.HasValue == false || res.Value > cat.MinX)
//                        res = cat.MinX;
//                }
//                else
//                {
//                    if (res.HasValue == false || res.Value > cat.MinY)
//                        res = cat.MinY;
//                }
//            }
//            if (res.HasValue == false)
//                return 0.0;
//            return res.Value;
//        }

//        void IInternalCandleData.OnAfterDeserialize()
//        {
//            if (mSerializedData == null)
//                return;
//            mData.Clear();
//            mSuspendEvents = true;
//            for (int i = 0; i < mSerializedData.Length; i++)
//            {
//                SerializedCategory cat = mSerializedData[i];
//                if (cat.Depth < 0)
//                    cat.Depth = 0f;
//                string name = cat.Name;
//                AddCategory3DCandle(name, cat.UpCandle, cat.DownCandle, cat.Depth);
//                CategoryData data = mData[name];
//                data.Data.AddRange(cat.Data);
//                data.MaxX = cat.MaxX;
//                data.MaxY = cat.MaxY;
//                data.MinX = cat.MinX;
//                data.MinY = cat.MinY;
//            }
//            mSuspendEvents = false;
//        }

//        void IInternalCandleData.OnBeforeSerialize()
//        {
//            List<SerializedCategory> serialized = new List<SerializedCategory>();
//            foreach (KeyValuePair<string, CategoryData> pair in mData)
//            {
//                SerializedCategory cat = new SerializedCategory();
//                cat.Name = pair.Key;
//                cat.MaxX = pair.Value.MaxX;
//                cat.MinX = pair.Value.MinX;
//                cat.MaxY = pair.Value.MaxY;
//                cat.MinY = pair.Value.MinY;
//                cat.UpCandle = pair.Value.UpCandle;
//                cat.DownCandle = pair.Value.DownCandle;
//                cat.Depth = pair.Value.Depth;
//                if (cat.Depth < 0)
//                    cat.Depth = 0f;
//                serialized.Add(cat);
//            }
//            mSerializedData = serialized.ToArray();
//        }

//        int IInternalCandleData.TotalCategories
//        {
//            get { return mData.Count; }
//        }

//        IEnumerable<CategoryData> IInternalCandleData.Categories
//        {
//            get
//            {
//                return mData.Values;
//            }
//        }
//    }
//}
