using ChartAndGraph.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ChartAndGraph
{
    [Serializable]
    public class GraphData : IInternalGraphData
    {
        private event EventHandler DataChanged;
        private event EventHandler RealtimeDataChanged;

        
        private bool mSuspendEvents = false;
        private bool mSliderUpdated = false;

        private List<DoubleVector3> mTmpDriv = new List<DoubleVector3>();

        [SerializeField]
        private double automaticVerticalViewGap = 0f;
        /// set this to true to automatically detect the horizontal size of the graph chart
        /// </summary>
        public double AutomaticVerticallViewGap
        {
            get { return automaticVerticalViewGap; }
            set
            {
                automaticVerticalViewGap = value;
                RestoreDataValues();
                RaiseDataChanged();
            }
        }

        /// <summary>
        /// set this to true to automatically detect the horizontal size of the graph chart
        /// </summary>
        [SerializeField]
        private bool automaticVerticallView = true;

        /// <summary>
        /// set this to true to automatically detect the horizontal size of the graph chart
        /// </summary>
        public bool AutomaticVerticallView
        {
            get { return automaticVerticallView; }
            set
            {
                automaticVerticallView = value;
                RestoreDataValues();
                RaiseDataChanged();
            }
        }

        [SerializeField]
        private double automaticcHorizontaViewGap = 0f;
        /// set this to true to automatically detect the horizontal size of the graph chart
        /// </summary>
        public double AutomaticcHorizontaViewGap
        {
            get { return automaticcHorizontaViewGap; }
            set
            {
                automaticcHorizontaViewGap = value;
                RestoreDataValues();
                RaiseDataChanged();
            }
        }

        /// <summary>
        /// set this to true to automatically detect the horizontal size of the graph chart
        /// </summary>
        [SerializeField]
        private bool automaticHorizontalView = true;

        /// <summary>
        /// set this to true to automatically detect the horizontal size of the graph chart
        /// </summary>
        public bool AutomaticHorizontalView
        {
            get { return automaticHorizontalView; }
            set
            {
                automaticHorizontalView = value;
                RestoreDataValues();
                RaiseDataChanged();
            }
        }

        public void RestoreDataValues()
        {
            if (automaticHorizontalView)
                RestoreDataValues(0);
            if (AutomaticVerticallView)
                RestoreDataValues(1);
        }

        void RestoreDataValues(int axis)
        {
            IInternalGraphData data = this;
            if (axis == 0)
            {
                double maxH = data.GetMaxValue(0, true);
                double minH = data.GetMinValue(0, true);
                horizontalViewOrigin = minH;
                horizontalViewSize = maxH - minH;
            }
            else
            {
                double maxV = data.GetMaxValue(1, true);
                double minV = data.GetMinValue(1, true);
                verticalViewOrigin = minV;
                verticalViewSize = maxV - minV;
            }
        }

        [SerializeField]
        private double horizontalViewSize = 100;

        /// <summary>
        /// Mannualy set the horizontal view size.
        /// </summary>
        public double HorizontalViewSize
        {
            get { return horizontalViewSize; }
            set
            {
                horizontalViewSize = value;
                RaiseDataChanged();
            }
        }

        
        [SerializeField]
        private double horizontalViewOrigin = 0;

        /// <summary>
        /// Mannualy set the horizontal view origin. 
        /// </summary>
        public double HorizontalViewOrigin
        {
            get { return horizontalViewOrigin; }
            set
            {
                horizontalViewOrigin = value;
                RaiseDataChanged();
            }
        }

        [SerializeField]
        private double verticalViewSize = 100;

        /// <summary>
        /// Mannualy set the horizontal view size.
        /// </summary>
        public double VerticalViewSize
        {
            get { return verticalViewSize; }
            set
            {
                verticalViewSize = value;
                RaiseDataChanged();
            }
        }


        [SerializeField]
        private double verticalViewOrigin = 0;

        /// <summary>
        /// Mannualy set the horizontal view origin. 
        /// </summary>
        public double VerticalViewOrigin
        {
            get { return verticalViewOrigin; }
            set
            {
                verticalViewOrigin = value;
                RaiseDataChanged();
            }
        }

        [Serializable]
        public class CategoryData
        {
            public string Name;
            public bool IsBezierCurve;
            public int SegmentsPerCurve = 10;
            public List<DoubleVector3> mTmpCurveData = new List<DoubleVector3>();
            public List<DoubleVector3> Data = new List<DoubleVector3>();
            public bool Regenerate = true;

            public List<DoubleVector3> getPoints()
            {
                if (IsBezierCurve == false)
                    return Data;
                if(Regenerate == false)
                    return mTmpCurveData;
                Regenerate = false;
                mTmpCurveData.Clear();
                if (Data.Count <= 0)
                    return mTmpCurveData;
                mTmpCurveData.Add(Data[0]);
                if (Data.Count < 4)
                    return mTmpCurveData;
                int endCount = Data.Count - 1;
                for (int i=0; i<endCount; i+=3)
                {
                    AddInnerCurve(Data[i], Data[i + 1], Data[i + 2], Data[i + 3]);
                    mTmpCurveData.Add(Data[i + 3]);
                }
                return mTmpCurveData;
            }

            public void AddInnerCurve(DoubleVector3 p1, DoubleVector3 c1,DoubleVector3 c2,DoubleVector3 p2)
            {
                for (int i=0; i<SegmentsPerCurve; i++)
                {
                    double blend = ((double)i) / (double)SegmentsPerCurve;
                    double invBlend = 1f - blend;
                    DoubleVector3 p = (invBlend * invBlend * invBlend * p1) + (3f * invBlend * invBlend * blend * c1) + (3f * blend * blend * invBlend * c2) + (blend * blend * blend * p2);
                    mTmpCurveData.Add(new DoubleVector3(p.x, p.y, 0f));
                }
            }

            public double? MaxX, MaxY, MinX, MinY, MaxRadius;
            public ChartItemEffect LineHoverPrefab; 
            public ChartItemEffect PointHoverPrefab;
            public Material LineMaterial;
            public MaterialTiling LineTiling;
            public double LineThickness = 1f;
            public Material FillMaterial;
            public bool StetchFill = false;
            public Material PointMaterial;
            public double PointSize = 5f;
            public PathGenerator LinePrefab;
            public FillPathGenerator FillPrefab;
            public GameObject DotPrefab;
            public double Depth = 0f;
        }

        class VectorComparer : IComparer<DoubleVector3>
        {
            public int Compare(DoubleVector3 x, DoubleVector3 y)
            {
                if (x.x < y.x)
                    return -1;
                if (x.x > y.x)
                    return 1;
                return 0;

            }
        }

        [Serializable]
        class SerializedCategory
        {
            public string Name;
            public bool IsBezierCurve;
            public int SegmentsPerCurve = 10;
            [NonCanvasAttribute]
            public double Depth;
            [HideInInspector]
            public DoubleVector3[] data;
            [HideInInspector]
            public double? MaxX, MaxY, MinX, MinY, MaxRadius;
            public ChartItemEffect LineHoverPrefab;
            public ChartItemEffect PointHoverPrefab;
            [NonCanvasAttribute]
            public PathGenerator LinePrefab;
            public Material Material;
            public MaterialTiling LineTiling;
            [NonCanvasAttribute]
            public FillPathGenerator FillPrefab;
            public Material InnerFill;
            public double LineThickness = 1f;
            public bool StetchFill = false;
            [NonCanvasAttribute]
            public GameObject DotPrefab;
            public Material PointMaterial;
            public double PointSize;
        }

        class Slider
        {
            public string category;
            public int from;
            public DoubleVector3 To;
            public DoubleVector3 current;
            public int index;
            public double startTime;
            public double totalTime;
        }
        void ModifyMinMax(CategoryData data, DoubleVector3 point)
        {
            if (data.MaxRadius.HasValue == false || data.MaxRadius.Value < point.z)
                data.MaxRadius = point.z;
            if (data.MaxX.HasValue == false || data.MaxX.Value < point.x)
                data.MaxX = point.x;
            if (data.MinX.HasValue == false || data.MinX.Value > point.x)
                data.MinX = point.x;
            if (data.MaxY.HasValue == false || data.MaxY.Value < point.y)
                data.MaxY = point.y;
            if (data.MinY.HasValue == false || data.MinY.Value > point.y)
                data.MinY = point.y;
        }
        public void Update()
        {
            mSliderUpdated = false;
            mSliders.RemoveAll(x =>
            {
                CategoryData data;
 
                if (mData.TryGetValue(x.category, out data) == false)
                    return true;
                if (data.IsBezierCurve)
                    return false;
                List<DoubleVector3> points = data.Data;

                if (x.from >= points.Count || x.index >= points.Count)
                    return true;

                DoubleVector3 from = points[x.from];
                DoubleVector3 to = x.To;
                double time = Time.time;
                time -= x.startTime;

                if (x.totalTime <= 0.0001f)
                    time = 1f;
                else
                {
                    time /= x.totalTime;
                    Math.Max(0.0, Math.Min(time, 1.0));
                }
                DoubleVector3 v = DoubleVector3.Lerp(from, to, time);
                x.current = v;
                points[x.index] = v;
                mSliderUpdated = true;
                if (time >= 1f)
                {
                    ModifyMinMax(data, v);
                    return true;
                }

                return false;
            });
            if (mSliderUpdated)
                RaiseRealtimeDataChanged();
        }

        List<Slider> mSliders = new List<Slider>();
        VectorComparer mComparer = new VectorComparer();
        Dictionary<string, CategoryData> mData = new Dictionary<string, CategoryData>();

        private void RaiseRealtimeDataChanged()
        {
            if (mSuspendEvents)
                return;
            if (RealtimeDataChanged != null)
                RealtimeDataChanged(this, EventArgs.Empty);
        }

        [SerializeField]
        SerializedCategory[] mSerializedData = new SerializedCategory[0];
        private void RaiseDataChanged()
        {
            if (mSuspendEvents)
                return;
            if (DataChanged != null)
                DataChanged(this, EventArgs.Empty);
        }
        /// <summary>
        /// call this to suspend chart redrawing while updating the data of the chart
        /// </summary>
        public void StartBatch()
        {
            mSuspendEvents = true;
        }
        /// <summary>
        /// call this after StartBatch , this will apply all the changed made between the StartBatch call to this call
        /// </summary>
        public void EndBatch()
        {
            mSuspendEvents = false;
            RaiseDataChanged();
        }


        public void ClearAndMakeBezierCurve(string category)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }
            CategoryData data = mData[category];
            data.IsBezierCurve = true;
            ClearCategory(category);
        }

        public void ClearAndMakeLinear(string category)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }
            CategoryData data = mData[category];
            data.IsBezierCurve = false;
            ClearCategory(category);
        }
        event EventHandler IInternalGraphData.InternalRealTimeDataChanged
        {
            add
            {
                RealtimeDataChanged += value;
            }

            remove
            {
                RealtimeDataChanged -= value;
            }
        }
        event EventHandler IInternalGraphData.InternalDataChanged
        {
            add
            {
                DataChanged += value;
            }

            remove
            {
                DataChanged -= value;
            }
        }
        /// <summary>
        /// rename a category. throws and exception on error
        /// </summary>
        /// <param name="prevName"></param>
        /// <param name="newName"></param>
        public void RenameCategory(string prevName, string newName)
        {
            if (prevName == newName)
                return;
            if (mData.ContainsKey(newName))
                throw new ArgumentException(String.Format("A category named {0} already exists", newName));
            CategoryData cat = mData[prevName];
            mData.Remove(prevName);
            cat.Name = newName;
            mData.Add(newName, cat);
            RaiseDataChanged();
        }

        /// <summary>
        /// Adds a new category to the graph chart. each category corrosponds to a graph line. 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="material"></param>
        /// <param name="innerFill"></param>
        public void AddCategory(string category, Material lineMaterial, double lineThickness, MaterialTiling lineTiling, Material innerFill, bool strechFill, Material pointMaterial, double pointSize)
        {
            if (mData.ContainsKey(category))
                throw new ArgumentException(String.Format("A category named {0} already exists", category));
            CategoryData data = new CategoryData();
            mData.Add(category, data);
            data.Name = category;
            data.LineMaterial = lineMaterial;
            data.LineHoverPrefab = null;
            data.PointHoverPrefab = null;
            data.FillMaterial = innerFill;
            data.LineThickness = lineThickness;
            data.LineTiling = lineTiling;
            data.StetchFill = strechFill;
            data.PointMaterial = pointMaterial;
            data.PointSize = pointSize;
            RaiseDataChanged();
        }

        public void Set2DCategoryPrefabs(string category, ChartItemEffect lineHover, ChartItemEffect pointHover)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }
            CategoryData data = mData[category];
            data.LineHoverPrefab = lineHover;
            data.PointHoverPrefab = pointHover;
        }

        public void AddCategory3DGraph(string category, PathGenerator linePrefab, Material lineMaterial, double lineThickness, MaterialTiling lineTiling, FillPathGenerator fillPrefab, Material innerFill, bool strechFill, GameObject pointPrefab, Material pointMaterial, double pointSize, double depth,bool isCurve,int segmentsPerCurve)
        {
            if (mData.ContainsKey(category))
                throw new ArgumentException(String.Format("A category named {0} already exists", category));
            if (depth < 0f)
                depth = 0f;
            CategoryData data = new CategoryData();
            mData.Add(category, data);
            data.Name = category;
            data.LineMaterial = lineMaterial;
            data.FillMaterial = innerFill;
            data.LineThickness = lineThickness;
            data.LineTiling = lineTiling;
            data.StetchFill = strechFill;
            data.PointMaterial = pointMaterial;
            data.PointSize = pointSize;
            data.LinePrefab = linePrefab;
            data.FillPrefab = fillPrefab;
            data.DotPrefab = pointPrefab;
            data.Depth = depth;
            data.IsBezierCurve = isCurve;
            data.SegmentsPerCurve = segmentsPerCurve;
            RaiseDataChanged();
        }

        /// <summary>
        /// sets the line style for the category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="lineMaterial"></param>
        /// <param name="lineThickness"></param>
        /// <param name="lineTiling"></param>
        public void SetCategoryLine(string category, Material lineMaterial, double lineThickness, MaterialTiling lineTiling)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }
            CategoryData data = mData[category];
            data.LineMaterial = lineMaterial;
            data.LineThickness = lineThickness;
            data.LineTiling = lineTiling;
            RaiseDataChanged();
        }

        /// <summary>
        /// clears all graph data
        /// </summary>
        public void Clear()
        {
            mSliders.Clear();
            mData.Clear();
        }

        public bool HasCategory(string category)
        {
            return mData.ContainsKey(category);
        }

        /// <summary>
        /// removed a category from the DataSource. returnes true on success , or false if the category does not exist
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool RemoveCategory(string category)
        {
            mSliders.RemoveAll(x => (x.category == category));
            return mData.Remove(category);
        }

        /// <summary>
        /// sets the point style for the selected category. set material to null for no points
        /// </summary>
        /// <param name="category"></param>
        /// <param name="pointMaterial"></param>
        /// <param name="pointSize"></param>
        public void SetCategoryPoint(string category, Material pointMaterial, double pointSize)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }

            CategoryData data = mData[category];
            data.PointMaterial = pointMaterial;
            data.PointSize = pointSize;
            RaiseDataChanged();
        }

        /// <summary>
        /// sets the prefabs for a 3d graph category,
        /// </summary>
        /// <param name="category"></param>
        /// <param name="linePrefab"></param>
        /// <param name="fillPrefab"></param>
        /// <param name="dotPrefab"></param>
        public void Set3DCategoryPrefabs(string category, PathGenerator linePrefab, FillPathGenerator fillPrefab, GameObject dotPrefab)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }
            CategoryData data = mData[category];
            data.LinePrefab = linePrefab;
            data.DotPrefab = dotPrefab;
            data.FillPrefab = fillPrefab;
            RaiseDataChanged();
        }

        /// <summary>
        /// sets the depth for a 3d graph category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="depth"></param>
        public void Set3DCategoryDepth(string category, double depth)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }
            if (depth < 0)
                depth = 0f;
            CategoryData data = mData[category];
            data.Depth = depth;
            RaiseDataChanged();
        }

        /// <summary>
        /// sets the fill style for the selected category.set the material to null for no fill
        /// </summary>
        /// <param name="category"></param>
        /// <param name="fillMaterial"></param>
        /// <param name="strechFill"></param>
        public void SetCategoryFill(string category, Material fillMaterial, bool strechFill)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }
            CategoryData data = mData[category];
            data.FillMaterial = fillMaterial;
            data.StetchFill = strechFill;
            RaiseDataChanged();
        }

        /// <summary>
        /// clears all the data for the selected category
        /// </summary>
        /// <param name="category"></param>
        public void ClearCategory(string category)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }
            mSliders.RemoveAll(x => (x.category == category));
            mData[category].MaxX = null;
            mData[category].MaxY = null;
            mData[category].MinX = null;
            mData[category].MinY = null;
            mData[category].MaxRadius = null;
            mData[category].Data.Clear();
            mData[category].Regenerate = true;
            RaiseDataChanged();
        }

        /// <summary> 
        /// adds a point to the category. having the point x,y values as dates
        /// <param name="category"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddPointToCategory(string category, DateTime x, DateTime y, double pointSize = -1f)
        {
            double xVal = ChartDateUtility.DateToValue(x);
            double yVal = ChartDateUtility.DateToValue(y);
            AddPointToCategory(category, (double)xVal, (double)yVal, pointSize);
        }
        public DoubleVector3 GetPoint(string category, int index)
        {
            CategoryData data = mData[category];
            List<DoubleVector3> points = data.getPoints();
            if (points.Count == 0)
                return DoubleVector3.zero;
            if (index < 0)
                return points[0];
            if (index >= points.Count)
                return points[points.Count - 1];
            return points[index];

        }

        /// <summary>
        /// adds a point to the category. having the point x value as date
        /// <param name="category"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddPointToCategory(string category, DateTime x, double y, double pointSize = -1f)
        {
            double xVal = ChartDateUtility.DateToValue(x);
            AddPointToCategory(category, (double)xVal, y, pointSize);
        }

        /// <summary>
        /// adds a point to the category. having the point y value as date
        /// </summary>
        /// <param name="category"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddPointToCategory(string category, double x, DateTime y, double pointSize = -1f)
        {
            double yVal = ChartDateUtility.DateToValue(y);
            AddPointToCategory(category, x, (double)yVal, pointSize);

        }

        public void AddPointToCategoryRealtime(string category, DateTime x, DateTime y, double slideTime = 0f, double pointSize = -1f)
        {
            double xVal = ChartDateUtility.DateToValue(x);
            double yVal = ChartDateUtility.DateToValue(y);
            AddPointToCategoryRealtime(category, (double)xVal, (double)yVal, slideTime,pointSize);
        }

        public void AddPointToCategoryRealtime(string category, DateTime x, double y, double slideTime = 0f, double pointSize = -1f)
        {
            double xVal = ChartDateUtility.DateToValue(x);
            AddPointToCategoryRealtime(category, (double)xVal, y, slideTime,pointSize);
        }

        public void AddPointToCategoryRealtime(string category, double x, DateTime y, double slideTime = 0f, double pointSize = -1f)
        {
            double yVal = ChartDateUtility.DateToValue(y);
            AddPointToCategoryRealtime(category, x, (double)yVal, slideTime, pointSize);
        }  

        public void AddPointToCategoryRealtime(string category, double x, double y,double slideTime =0f, double pointSize = -1f)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }

            CategoryData data = mData[category];
            
            if (data.IsBezierCurve == true)
            {
                Debug.LogWarning("Category is Bezier curve. use AddCurveToCategory instead ");
                return;
            }

            DoubleVector3 point = new DoubleVector3(x, y, pointSize);
            List<DoubleVector3> points = data.Data;

            if (points.Count > 0)
            {
                if (points[points.Count - 1].x > point.x)
                {
                    Debug.LogWarning("realtime points can only be added at the end of the graph");
                    return;
                }
            }


            if (slideTime <= 0f || points.Count == 0)
            {
                points.Add(point);
                ModifyMinMax(data, point);
            }
            else
            {
                Slider s = new Slider();
                s.category = category;
                s.from = points.Count - 1;
                s.index = points.Count;
                s.startTime = Time.time;
                s.totalTime = slideTime;
                s.To = point;
                mSliders.Add(s);
                s.current = points[points.Count - 1];
                points.Add(s.current);
            }
            RaiseRealtimeDataChanged();
        }

        public void SetCurveInitialPoint(string category, DateTime x, double y, double pointSize = -1f)
        {
            SetCurveInitialPoint(category,ChartDateUtility.DateToValue(x), y, pointSize);
        }

        public void SetCurveInitialPoint(string category, DateTime x, DateTime y, double pointSize = -1f)
        {
            SetCurveInitialPoint(category, ChartDateUtility.DateToValue(x), ChartDateUtility.DateToValue(y), pointSize);
        }

        public void SetCurveInitialPoint(string category, double x, DateTime y, double pointSize = -1f)
        {
            SetCurveInitialPoint(category, x,ChartDateUtility.DateToValue(y), pointSize);
        }

        public void SetCurveInitialPoint(string category, double x, double y,double pointSize = -1f)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }

            CategoryData data = mData[category];
            if (data.IsBezierCurve == false)
            {
                Debug.LogWarning("Category is not Bezier curve. use AddPointToCategory instead ");
                return;
            }

            if(data.Data.Count > 0)
            {
                Debug.LogWarning("Initial point already set for this category, call is ignored. Call ClearCategory to create a new curve");
                return;
            }
            data.Regenerate = true;
            if (data.MaxRadius.HasValue == false || data.MaxRadius.Value < pointSize)
                data.MaxRadius = pointSize;
            if (data.MaxX.HasValue == false || data.MaxX.Value < x)
                data.MaxX = x;
            if (data.MinX.HasValue == false || data.MinX.Value > x)
                data.MinX = x;
            if (data.MaxY.HasValue == false || data.MaxY.Value < y)
                data.MaxY = y;
            if (data.MinY.HasValue == false || data.MinY.Value > y)
                data.MinY = y;

            DoubleVector3 sizedPoint = new DoubleVector3(x, y, pointSize);
            data.Data.Add(sizedPoint);
            RaiseDataChanged();
        }

        private double min3(double a,double b,double c)
        {
            return Math.Min(a, Math.Min(b, c));
        }

        private double max3(double a, double b, double c)
        {
            return Math.Max(a, Math.Max(b, c));
        }

        private DoubleVector2 max3(DoubleVector2 a, DoubleVector2 b, DoubleVector2 c)
        {
            return new DoubleVector2(max3(a.x, b.x, c.x), max3(a.y, b.y, c.y));
        }

        private DoubleVector2 min3(DoubleVector2 a, DoubleVector2 b, DoubleVector2 c)
        {
            return new DoubleVector2(min3(a.x, b.x, c.x), min3(a.y, b.y, c.y));
        }

        public void MakeCurveCategorySmooth(string category)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }

            CategoryData data = mData[category];
            if (data.IsBezierCurve == false)
            {
                Debug.LogWarning("Category is not Bezier curve. use AddPointToCategory instead ");
                return;
            }
            List<DoubleVector3> points = data.Data;
            data.Regenerate = true;
            mTmpDriv.Clear();
            for(int i=0; i< points.Count; i+=3)
            {
                DoubleVector3 prev = points[Mathf.Max(i - 3, 0)];
                DoubleVector3 next = points[Mathf.Min(i + 3, points.Count - 1)];
                DoubleVector3 diff = next - prev;
                mTmpDriv.Add(diff * 0.25f);
            }

            for (int i = 3; i < points.Count; i+=3)
            {
                int driv = i / 3;
                DoubleVector3 ct1 = points[i - 3] + (DoubleVector3)mTmpDriv[driv - 1];
                DoubleVector3 ct2 = points[i] - (DoubleVector3)mTmpDriv[driv];
                points[i - 2] = ct1;
                points[i - 1] = ct2;
            }
            RaiseDataChanged();
        }

        public void AddLinearCurveToCategory(string category, DoubleVector2 toPoint, double pointSize = -1f)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }

            CategoryData data = mData[category];
            if (data.IsBezierCurve == false)
            {
                Debug.LogWarning("Category is not Bezier curve. use AddPointToCategory instead ");
                return;
            }

            if (data.Data.Count == 0)
            {
                Debug.LogWarning("Initial not set for this category, call is ignored. Call SetCurveInitialPoint to create a new curve");
                return;
            }

            List<DoubleVector3> points = data.Data;
            DoubleVector3 last = points[points.Count - 1];
            DoubleVector3 c1 = DoubleVector3.Lerp(last, toPoint.ToDoubleVector3(), 1f / 3f);
            DoubleVector3 c2 = DoubleVector3.Lerp(last, toPoint.ToDoubleVector3(), 2f / 3f);
            AddCurveToCategory(category, c1.ToDoubleVector2(), c2.ToDoubleVector2(), toPoint, pointSize);
        }

        public void AddCurveToCategory(string category, DoubleVector2 controlPointA, DoubleVector2 controlPointB , DoubleVector2 toPoint,double pointSize = -1f)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }

            CategoryData data = mData[category];
            if (data.IsBezierCurve == false)
            {
                Debug.LogWarning("Category is not Bezier curve. use AddPointToCategory instead ");
                return;
            }

            if (data.Data.Count == 0)
            {
                Debug.LogWarning("Initial not set for this category, call is ignored. Call SetCurveInitialPoint to create a new curve");
                return;
            }

            List<DoubleVector3> points = data.Data;
            if (points.Count > 0 && points[points.Count - 1].x > toPoint.x)
            {
                Debug.LogWarning("Curves must be added sequentialy according to the x axis. toPoint.x is smaller then the previous point x value");
                return;
            }
            data.Regenerate = true;
            DoubleVector2 min = min3(controlPointA, controlPointB, toPoint);
            DoubleVector2 max = max3(controlPointA, controlPointB, toPoint);

            if (data.MaxRadius.HasValue == false || data.MaxRadius.Value < pointSize)
                data.MaxRadius = pointSize;
            if (data.MaxX.HasValue == false || data.MaxX.Value < max.x)
                data.MaxX = max.x;
            if (data.MinX.HasValue == false || data.MinX.Value > min.x)
                data.MinX = min.x;
            if (data.MaxY.HasValue == false || data.MaxY.Value < max.y)
                data.MaxY = max.y;
            if (data.MinY.HasValue == false || data.MinY.Value > min.y)
                data.MinY = min.y;

            points.Add(controlPointA.ToDoubleVector3());
            points.Add(controlPointB.ToDoubleVector3());
            points.Add(new DoubleVector3(toPoint.x,toPoint.y,pointSize));
            
            RaiseDataChanged();
        }

        /// <summary>
        /// adds a point to the category. The points are sorted by their x value automatically
        /// </summary>
        /// <param name="category"></param>
        /// <param name="point"></param>
        public void AddPointToCategory(string category, double x,double y, double pointSize = -1f)
        {
            if (mData.ContainsKey(category) == false)
            {
                Debug.LogWarning("Invalid category name. Make sure the category is present in the graph");
                return;
            }

            CategoryData data = mData[category];
            if(data.IsBezierCurve == true)
            {
                Debug.LogWarning("Category is Bezier curve. use AddCurveToCategory instead ");
                return;
            }
            DoubleVector3 point = new DoubleVector3(x, y,pointSize);
            
            List<DoubleVector3> points = data.Data;

            if (data.MaxRadius.HasValue == false || data.MaxRadius.Value < pointSize)
                data.MaxRadius = pointSize;
            if (data.MaxX.HasValue == false || data.MaxX.Value < point.x)
                data.MaxX = point.x;
            if (data.MinX.HasValue == false || data.MinX.Value > point.x)
                data.MinX = point.x;
            if (data.MaxY.HasValue == false || data.MaxY.Value < point.y)
                data.MaxY = point.y;
            if (data.MinY.HasValue == false || data.MinY.Value > point.y)
                data.MinY = point.y;

            if (points.Count > 0)
            {
                if (points[points.Count - 1].x <= point.x)
                {
                    points.Add(point);
                    return;
                }
            }
         //   points.Add(point);
            int search = points.BinarySearch(point, mComparer);
            if (search < 0)
                search = ~search;
            points.Insert(search, point);               
            RaiseDataChanged();
        }
        
        
        double IInternalGraphData.GetMaxValue(int axis, bool dataValue)
        {
            if (dataValue == false)
            {
                if (axis == 0 && automaticHorizontalView == false)
                    return HorizontalViewOrigin + Math.Max(0.001f, horizontalViewSize);
                if (axis == 1 && AutomaticVerticallView == false)
                    return VerticalViewOrigin + Math.Max(0.001f, verticalViewSize);
            }
            double? res = null;
            double add = 0;
            foreach (CategoryData cat in mData.Values)
            {
                if (cat.MaxRadius.HasValue && add < cat.MaxRadius)
                    add = cat.MaxRadius.Value;
                if (axis == 0)
                {
                    if (res.HasValue == false || (cat.MaxX.HasValue && res.Value < cat.MaxX))
                        res = cat.MaxX;
                }
                else
                {
                    if (res.HasValue == false || (cat.MaxY.HasValue &&  res.Value < cat.MaxY))
                        res = cat.MaxY;
                }
            }
            foreach (Slider s in mSliders)
            {
                if (axis == 0)
                {
                    if (res.HasValue == false ||  res.Value < s.current.x)
                        res = s.current.x;
                }
                else
                {
                    if (res.HasValue == false || res.Value < s.current.y)
                        res = s.current.y;
                }
            }
            if (res.HasValue == false)
                return 10f;
            double gap = (axis == 0) ? automaticcHorizontaViewGap : automaticVerticalViewGap;
            return res.Value + add + gap;
        }

        double IInternalGraphData.GetMinValue(int axis, bool dataValue) 
        {
            if (dataValue == false)
            {
                if (axis == 0 && automaticHorizontalView == false)
                    return horizontalViewOrigin;
                if (axis == 1 && AutomaticVerticallView == false)
                    return verticalViewOrigin;
            }                
            double? res = null;
            double add = 0f;

            foreach (CategoryData cat in mData.Values)
            {
                if (cat.MaxRadius.HasValue && add < cat.MaxRadius)
                    add = cat.MaxRadius.Value;

                if (axis == 0)
                {
                    if (res.HasValue == false || (cat.MinX.HasValue && res.Value > cat.MinX))
                        res = cat.MinX;
                }
                else
                {
                    if (res.HasValue == false || (cat.MinY.HasValue && res.Value > cat.MinY))
                        res = cat.MinY;
                }
            }
            foreach (Slider s in mSliders)
            {
                if (axis == 0)
                {
                    if (res.HasValue == false || res.Value > s.current.x)
                        res = s.current.x;
                }
                else
                {
                    if (res.HasValue == false || res.Value > s.current.y)
                        res = s.current.y;
                }
            }
        //    Debug.Log(res.HasValue);
            if (res.HasValue == false)
                return 0.0;
            double max = ((IInternalGraphData)this).GetMaxValue(axis, dataValue);
           // Debug.Log(max); 
            if (max == res.Value)
            {
                if (res.Value == 0.0)
                    return -10.0f;
                return 0.0;
            }
            double gap = (axis == 0) ? automaticcHorizontaViewGap : automaticVerticalViewGap;
            return res.Value - add - gap;
        }

        void IInternalGraphData.OnAfterDeserialize()
        {
            if (mSerializedData == null)
                return;
            mData.Clear();
            mSuspendEvents = true;
            for (int i = 0; i < mSerializedData.Length; i++)
            {
                SerializedCategory cat = mSerializedData[i];
                if (cat.Depth < 0)
                    cat.Depth = 0f;
                string name = cat.Name;
                AddCategory3DGraph(name,cat.LinePrefab, cat.Material, cat.LineThickness, cat.LineTiling,cat.FillPrefab, cat.InnerFill,cat.StetchFill,cat.DotPrefab,cat.PointMaterial,cat.PointSize,cat.Depth,cat.IsBezierCurve,cat.SegmentsPerCurve);
                Set2DCategoryPrefabs(name, cat.LineHoverPrefab, cat.PointHoverPrefab);
                CategoryData data = mData[name];
                if (data.Data == null)
                    data.Data = new List<DoubleVector3>();
                else
                    data.Data.Clear();
                if(cat.data != null)
                    data.Data.AddRange(cat.data);
                data.MaxX = cat.MaxX;
                data.MaxY = cat.MaxY;
                data.MinX = cat.MinX;
                data.MinY = cat.MinY;
                data.MaxRadius = cat.MaxRadius;
            }
            mSuspendEvents = false;
        }

        void IInternalGraphData.OnBeforeSerialize()
        {
            List<SerializedCategory> serialized = new List<SerializedCategory>();
            foreach (KeyValuePair<string, CategoryData> pair in mData)
            {
                SerializedCategory cat = new SerializedCategory();
                cat.Name = pair.Key;
                cat.MaxX = pair.Value.MaxX;
                cat.MinX = pair.Value.MinX;
                cat.MaxY = pair.Value.MaxY;
                cat.MaxRadius = pair.Value.MaxRadius;
                cat.MinY = pair.Value.MinY;
                cat.LineThickness = pair.Value.LineThickness;
                cat.StetchFill = pair.Value.StetchFill;
                cat.Material = pair.Value.LineMaterial;
                cat.LineHoverPrefab = pair.Value.LineHoverPrefab;
                cat.PointHoverPrefab = pair.Value.PointHoverPrefab;
                cat.LineTiling = pair.Value.LineTiling;
                cat.InnerFill = pair.Value.FillMaterial;
                cat.data = pair.Value.Data.ToArray();
                cat.PointSize = pair.Value.PointSize;
                cat.IsBezierCurve = pair.Value.IsBezierCurve;
                cat.SegmentsPerCurve = pair.Value.SegmentsPerCurve;
                cat.PointMaterial = pair.Value.PointMaterial;
                cat.LinePrefab = pair.Value.LinePrefab;
                cat.Depth = pair.Value.Depth;
                cat.DotPrefab = pair.Value.DotPrefab;
                cat.FillPrefab = pair.Value.FillPrefab;
                if (cat.Depth < 0)
                    cat.Depth = 0f;
                serialized.Add(cat);
            }
            mSerializedData = serialized.ToArray();
        }
        int IInternalGraphData.TotalCategories
        {
            get { return mData.Count; }
        }
        IEnumerable<CategoryData> IInternalGraphData.Categories
        {
            get
            {
                return mData.Values;
            }
        }
    }
}
