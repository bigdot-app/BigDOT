using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ChartAndGraph
{
    public class GraphChart : GraphChartBase, ICanvas
    {
        private Vector2 mLastSetSize = Vector2.zero;
        HashSet<string> mOccupiedCateogies = new HashSet<string>();
        Dictionary<string, Dictionary<int, BillboardText>> mTexts = new Dictionary<string, Dictionary<int, BillboardText>>();
        HashSet<BillboardText> mActiveTexts = new  HashSet<BillboardText>();
        Dictionary<string, CategoryObject> mCategoryObjects = new Dictionary<string, CategoryObject>();
        List<DoubleVector3> mTmpData = new List<DoubleVector3>();
        List<DoubleVector4> mClipped = new List<DoubleVector4>();
        List<Vector4> mTransformed = new List<Vector4>();
        List<int> mTmpToRemove = new List<int>();
        GameObject mFixPosition = null;
        GameObject mMask;
        private Vector2? mLastPosition;
        private GraphicRaycaster mCaster;
        private bool SupressRealtimeGeneration = false;
        private StringBuilder mRealtimeStringBuilder = new StringBuilder();
        public UnityEvent MousePan;

        [SerializeField]
        private bool horizontalPanning;

        public bool HorizontalPanning
        {
            get { return horizontalPanning; ; }
            set
            {
                horizontalPanning = value;
                Invalidate();
            }
        }

        [SerializeField]
        private bool verticalPanning;

        public bool VerticalPanning
        {
            get { return verticalPanning; ; }
            set
            {
                verticalPanning = value;
                Invalidate();
            }
        }

        class CategoryObject
        {
            public CanvasChartMesh mItemLabels;
            public CanvasLines mLines;
            public CanvasLines mDots;
            public CanvasLines mFill;
            public Dictionary<int, string> mCahced = new Dictionary<int, string>();
        }

        GameObject CreateRectMask(Rect viewRect)
        {
            GameObject obj = Instantiate(Resources.Load("Chart And Graph/RectMask") as GameObject);
            ChartCommon.HideObject(obj, hideHierarchy);
            obj.AddComponent<ChartItem>();
            obj.transform.SetParent(transform, false);
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = new Vector2(0f, 0f);
            rectTransform.pivot = new Vector2(0f, 1f);
            rectTransform.sizeDelta = viewRect.size;
            rectTransform.anchoredPosition = new Vector2(0f, viewRect.size.y);
            mMask = obj;
            Mask m = mMask.GetComponent<Mask>();
            if (m != null)
                m.enabled = Scrollable;
            return mMask;
        }

        private CanvasLines CreateDataObject(GraphData.CategoryData data, GameObject rectMask)
        {
            GameObject obj = new GameObject("Lines",typeof(RectTransform));
            ChartCommon.HideObject(obj, hideHierarchy);
            obj.AddComponent<ChartItem>();
            RectTransform t = obj.GetComponent<RectTransform>();
            obj.AddComponent<CanvasRenderer>();

            //  Canvas c = obj.AddComponent<Canvas>();

            //c.pixelPerfect = false;
           // obj.AddComponent<GraphicRaycaster>();

            CanvasLines lines = obj.AddComponent<CanvasLines>();
            lines.maskable = true;

            t.SetParent(rectMask.transform, false);
            t.localScale = new Vector3(1f, 1f, 1f);
            t.anchorMin = new Vector2(0f, 0f);
            t.anchorMax = new Vector2(0f, 0f);
            t.anchoredPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            return lines;
        }

        protected override void Update()
        {
            base.Update();
            HandleMouseDrag();
            RectTransform trans = GetComponent<RectTransform>();
            if (trans != null && trans.hasChanged)
            {
                if (mLastSetSize != trans.rect.size)
                {
                    Invalidate();
                }
            }
        }

        void AddBillboardText(string cat,int index,BillboardText text)
        {
            if(text.UIText != null)
                text.UIText.maskable = false;
            Dictionary<int, BillboardText> addTo;
            if(mTexts.TryGetValue(cat, out addTo) == false)
            {
                addTo = new Dictionary<int, BillboardText>(ChartCommon.DefaultIntComparer);
                mTexts.Add(cat, addTo);
            }
            addTo.Add(index,text);
        }

        protected override void ClearChart()
        {
            if(mMask != null)
            {
                Mask m = mMask.GetComponent<Mask>();
                m.enabled = false;
            }
            base.ClearChart();
            mTexts.Clear();
            mActiveTexts.Clear();
            mCategoryObjects.Clear();
        }

        double AddRadius(double radius, double mag, double min, double max)
        {
            double size = max - min;
            double factor = size / mag;
            return factor* radius;
        }

        public override void GenerateRealtime()
        {
            if (SupressRealtimeGeneration)
                return;
            base.GenerateRealtime();

            double minX = ((IInternalGraphData)Data).GetMinValue(0,false);
            double minY = ((IInternalGraphData)Data).GetMinValue(1, false);
            double maxX = ((IInternalGraphData)Data).GetMaxValue(0, false);
            double maxY = ((IInternalGraphData)Data).GetMaxValue(1, false);

            double xScroll = GetScrollOffset(0);
            double yScroll = GetScrollOffset(1);
            double xSize = maxX - minX;
            double ySize = maxY - minY;
            double xOut = minX + xScroll + xSize;
            double yOut = minY + yScroll + ySize;

            DoubleVector3 min = new DoubleVector3(xScroll + minX, yScroll + minY);
            DoubleVector3 max = new DoubleVector3(xOut, yOut);

            Rect viewRect = new Rect(0f, 0f, widthRatio, heightRatio);

            Transform parentT = transform;

            if (mFixPosition != null)
                parentT = mFixPosition.transform;

            foreach (Dictionary<int, BillboardText> d in mTexts.Values)
                d.Clear();

            foreach (GraphData.CategoryData data in ((IInternalGraphData)Data).Categories)
            {
                CategoryObject obj = null;

                if (mCategoryObjects.TryGetValue(data.Name, out obj) == false)
                    continue;

                mClipped.Clear();
                mTmpData.Clear();

                mTmpData.AddRange(data.getPoints());
                Rect uv;// = new Rect(0f, 0f, 1f, 1f);
                int refrenceIndex = ClipPoints(mTmpData, mClipped , out uv);
                //mClipped.AddRange(mTmpData);
                TransformPoints(mClipped, mTransformed, viewRect, min, max);
                mTmpToRemove.Clear();
                int range = refrenceIndex + mClipped.Count;
                foreach (int key in obj.mCahced.Keys)
                {
                    if (key < refrenceIndex || key > range)
                        mTmpToRemove.Add(key);
                }

                for(int i=0; i<mTmpToRemove.Count; i++)
                    obj.mCahced.Remove(mTmpToRemove[i]);

                if (mTmpData.Count == 0)
                    continue;
                    if (mItemLabels != null && mItemLabels.isActiveAndEnabled && obj.mItemLabels != null)
                {
                    Rect textRect = viewRect;
                    textRect.xMin -= 1f;
                    textRect.yMin -= 1f;
                    textRect.xMax += 1f;
                    textRect.yMax += 1f;

                    CanvasChartMesh m = obj.mItemLabels;
                    m.Clear();

                    for (int i = 0; i < mTransformed.Count; i++)
                    {
                        if (mTransformed[i].w == 0.0)
                            continue;
                        Vector3 labelPos = ((Vector3)mTransformed[i]) + new Vector3(mItemLabels.Location.Breadth, mItemLabels.Seperation, mItemLabels.Location.Depth);
                        if (mItemLabels.Alignment == ChartLabelAlignment.Base)
                            labelPos.y -= (float)mTransformed[i].y;
                        if (textRect.Contains((Vector2)(mTransformed[i])) == false)
                            continue;
                        string toSet = null;
                        int pointIndex = i + refrenceIndex;
                        if (obj.mCahced.TryGetValue(pointIndex, out toSet) == false)
                        {
                            DoubleVector3 pointValue = mTmpData[i + refrenceIndex];
                            string xFormat = StringFromAxisFormat(pointValue.x, mHorizontalAxis, mItemLabels.FractionDigits);
                            string yFormat = StringFromAxisFormat(pointValue.y, mVerticalAxis, mItemLabels.FractionDigits);
                            mItemLabels.TextFormat.Format(mRealtimeStringBuilder, String.Format("{0}:{1}", xFormat, yFormat), data.Name, "");
                            toSet = mRealtimeStringBuilder.ToString();
                            obj.mCahced[pointIndex] = toSet;
                        }
                        BillboardText billboard = m.AddText(this, mItemLabels.TextPrefab, parentT, mItemLabels.FontSize, mItemLabels.FontSharpness, toSet, labelPos.x, labelPos.y, labelPos.z, 0f, null);
                        AddBillboardText(data.Name, i + refrenceIndex, billboard);
                    }

                    m.DestoryRecycled();
                    if (m.TextObjects != null)
                    {
                        foreach (BillboardText text in m.TextObjects)
                        {
                            ((IInternalUse)this).InternalTextController.AddText(text);
                        }
                    }
                }

                if (obj.mDots != null)
                {
                    Rect pickRect = viewRect;
                    float halfSize = (float)(data.PointSize * 0.5f);
                    pickRect.xMin -= halfSize;
                    pickRect.yMin -= halfSize;
                    pickRect.xMax += halfSize;
                    pickRect.yMax += halfSize;
                    obj.mDots.SetViewRect(pickRect, uv);
                    obj.mDots.ModifyLines(mTransformed);
                    obj.mDots.refrenceIndex = refrenceIndex;
                    
                }

                if (obj.mLines != null)
                {
                    float tiling = 1f;
                    if (data.LineTiling.EnableTiling == true && data.LineTiling.TileFactor > 0f)
                    {
                        float length = 0f;
                        for (int i = 1; i < mTransformed.Count; i++)
                            length += (mTransformed[i - 1] - mTransformed[i]).magnitude;
                        tiling = length / data.LineTiling.TileFactor;
                    }
                    if (tiling <= 0.0001f)
                        tiling = 1f;
                    obj.mLines.Tiling = tiling;
                    obj.mLines.SetViewRect(viewRect, uv);
                    obj.mLines.ModifyLines(mTransformed);
                    obj.mLines.refrenceIndex = refrenceIndex;
                    
                }

                if (obj.mFill != null)
                {
                    obj.mFill.SetViewRect(viewRect, uv);
                    obj.mFill.ModifyLines(mTransformed);
                    obj.mFill.refrenceIndex = refrenceIndex;
                    
                }

            }
        }

        public override void InternalGenerateChart()
        {
            if (gameObject.activeInHierarchy == false)
                return;
            base.InternalGenerateChart();
            ClearChart();
            if (Data == null)
                return;
            GenerateAxis(true);
            double minX = ((IInternalGraphData)Data).GetMinValue(0, false);
            double minY = ((IInternalGraphData)Data).GetMinValue(1, false);
            double maxX =((IInternalGraphData)Data).GetMaxValue(0, false);
            double maxY = ((IInternalGraphData)Data).GetMaxValue(1, false);

            double xScroll = GetScrollOffset(0);
            double yScroll = GetScrollOffset(1);
            double xSize = maxX - minX;
            double ySize = maxY - minY;
            double xOut = minX + xScroll + xSize;
            double yOut = minY + yScroll + ySize;

            DoubleVector3 min = new DoubleVector3(xScroll + minX, yScroll + minY);
            DoubleVector3 max = new DoubleVector3(xOut, yOut);

            Rect viewRect = new Rect(0f, 0f, widthRatio, heightRatio);

            int index = 0;
            int total = ((IInternalGraphData)Data).TotalCategories + 1;
            bool edit = false;
            mTexts.Clear();
            mActiveTexts.Clear();

            GameObject mask = CreateRectMask(viewRect);

            foreach (GraphData.CategoryData data in ((IInternalGraphData)Data).Categories)
            {
                mClipped.Clear(); 
                DoubleVector3[] points = data.getPoints().ToArray();
                Rect uv;
                int refrenceIndex = ClipPoints(points, mClipped,out uv);
                TransformPoints(mClipped,mTransformed, viewRect, min, max);

                if (points.Length == 0 && ChartCommon.IsInEditMode)
                {
                    edit = true;
                    int tmpIndex = total - 1 - index;
                    float y1 = (((float)tmpIndex) / (float)total);
                    float y2 = (((float)tmpIndex + 1) / (float)total);
                    DoubleVector3 pos1 = interpolateInRect(viewRect, new DoubleVector3(0f, y1,-1f)).ToDoubleVector3();
                    DoubleVector3 pos2 = interpolateInRect(viewRect, new DoubleVector3(0.5f, y2,-1f)).ToDoubleVector3();
                    DoubleVector3 pos3 = interpolateInRect(viewRect, new DoubleVector3(1f, y1,-1f)).ToDoubleVector3();
                    points = new DoubleVector3[] { pos1, pos2, pos3 };
                    mTransformed.AddRange(points.Select(x=>(Vector4)x.ToVector3()));
                    index++;
                }

                List<CanvasLines.LineSegement> list = new List<CanvasLines.LineSegement>();
                list.Add(new CanvasLines.LineSegement(mTransformed));
                CategoryObject categoryObj = new CategoryObject();

                if (data.FillMaterial != null)
                {
                    CanvasLines fill = CreateDataObject(data, mask);
                    fill.material = data.FillMaterial;
                    fill.refrenceIndex = refrenceIndex;
                    fill.SetLines(list);
                    fill.SetViewRect(viewRect, uv);
                    fill.MakeFillRender(viewRect, data.StetchFill);
                    categoryObj.mFill = fill;
                }

                if (data.LineMaterial != null)
                {
                    CanvasLines lines = CreateDataObject(data, mask);

                    float tiling = 1f;
                    if (data.LineTiling.EnableTiling == true && data.LineTiling.TileFactor > 0f)
                    {
                        float length = 0f;
                        for (int i = 1; i < mTransformed.Count; i++)
                            length += (mTransformed[i - 1] - mTransformed[i]).magnitude;
                        tiling = length / data.LineTiling.TileFactor;
                    }
                    if (tiling <= 0.0001f)
                        tiling = 1f;
                    lines.SetViewRect(viewRect, uv);
                    lines.Thickness = (float)data.LineThickness;
                    lines.Tiling = tiling;
                    lines.refrenceIndex = refrenceIndex;
                    lines.material = data.LineMaterial;
                    lines.SetHoverPrefab(data.LineHoverPrefab);
                    lines.SetLines(list);
                    categoryObj.mLines = lines;

                }

                //if (data.PointMaterial != null)
                //{
                    CanvasLines dots = CreateDataObject(data, mask);
                    categoryObj.mDots = dots;
                    dots.material = data.PointMaterial;
                    dots.SetLines(list);
                    Rect pickRect = viewRect;
                    float halfSize = (float)data.PointSize * 0.5f;
                    pickRect.xMin -= halfSize;
                    pickRect.yMin -= halfSize;
                    pickRect.xMax += halfSize;
                    pickRect.yMax += halfSize;
                     dots.SetViewRect(pickRect, uv);
                    dots.refrenceIndex = refrenceIndex;
                    dots.SetHoverPrefab(data.PointHoverPrefab);

                    if (data.PointMaterial != null)
                        dots.MakePointRender((float)data.PointSize);
                    else
                        dots.MakePointRender(0f);

                    if (mItemLabels != null && mItemLabels.isActiveAndEnabled)
                    {
                        CanvasChartMesh m = new CanvasChartMesh(true);
                        m.RecycleText = true;
                        categoryObj.mItemLabels = m;
                        Rect textRect = viewRect;
                        textRect.xMin -= 1f;
                        textRect.yMin -= 1f;
                        textRect.xMax += 1f;
                        textRect.yMax += 1f;
                         for (int i = 0; i < mTransformed.Count; i++)
                        {
                            if (mTransformed[i].w == 0f)
                                continue;
                            Vector2 pointValue = mTransformed[i];
                            if (textRect.Contains(pointValue) == false)
                                continue;
                            if (edit == false)
                                pointValue = Data.GetPoint(data.Name, i + refrenceIndex).ToVector2();
                            string xFormat = StringFromAxisFormat(pointValue.x, mHorizontalAxis, mItemLabels.FractionDigits);
                            string yFormat = StringFromAxisFormat(pointValue.y, mVerticalAxis, mItemLabels.FractionDigits);
                            Vector3 labelPos = ((Vector3)mTransformed[i]) + new Vector3(mItemLabels.Location.Breadth, mItemLabels.Seperation, mItemLabels.Location.Depth);
                            if (mItemLabels.Alignment == ChartLabelAlignment.Base)
                                labelPos.y -= mTransformed[i].y;
                            string toSet = mItemLabels.TextFormat.Format(String.Format("{0}:{1}", xFormat, yFormat), data.Name, "");
                            BillboardText billboard = m.AddText(this, mItemLabels.TextPrefab, transform, mItemLabels.FontSize, mItemLabels.FontSharpness, toSet, labelPos.x, labelPos.y, labelPos.z, 0f, null);
                        //                          BillboardText billboard = ChartCommon.CreateBillboardText(null,mItemLabels.TextPrefab, transform, toSet, labelPos.x, labelPos.y, labelPos.z, 0f, null, hideHierarchy, mItemLabels.FontSize, mItemLabels.FontSharpness);
                            TextController.AddText(billboard);
                            AddBillboardText(data.Name,i +refrenceIndex, billboard);
                        }
                    }
                    string catName = data.Name;
                    dots.Hover += (idx,pos) => { Dots_Hover(catName, idx, pos); } ;
                    dots.Click += (idx, pos) => { Dots_Click(catName, idx, pos); };
                    dots.Leave += () => { Dots_Leave(catName); };
                //}
                mCategoryObjects[catName] = categoryObj;
            }
            TextController.transform.SetAsLastSibling();
            FitCanvas();
        }
        private void MouseDraged(Vector2 delta)
        {
            bool drag = false;
            SupressRealtimeGeneration = true;
            if (VerticalPanning)
            {
                float minY = (float)((IInternalGraphData)Data).GetMinValue(1, false);
                float maxY = (float)((IInternalGraphData)Data).GetMaxValue(1, false);
                float range = maxY - minY;
                VerticalScrolling -= (delta.y / heightRatio) * range;
                if (Mathf.Abs(delta.y) > 1f)
                    drag = true;
            }

            if (HorizontalPanning)
            {
                float minX = (float)((IInternalGraphData)Data).GetMinValue(0, false);
                float maxX = (float)((IInternalGraphData)Data).GetMaxValue(0, false);
                float range = maxX - minX;
                HorizontalScrolling -= (delta.x / widthRatio) * range;
                if (Mathf.Abs(delta.x) > 1f)
                    drag = true;
            }
            SupressRealtimeGeneration = false;
            if (drag)
            {
        //        Invalidate();
                GenerateRealtime();
                if (MousePan != null)
                    MousePan.Invoke();
            }
        }
        private void HandleMouseDrag()
        {

            if (verticalPanning == false && horizontalPanning == false)
                return;
            mCaster = GetComponentInParent<GraphicRaycaster>();
            if (mCaster == null)
                return;
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, mCaster.eventCamera, out mousePos);

            bool mouseIn = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition);
            if (Input.GetMouseButton(0) && mouseIn)
            {
                if (mLastPosition.HasValue)
                {
                    Vector2 delta = mousePos - mLastPosition.Value;
                    MouseDraged(delta);
                }
            }
            mLastPosition = mousePos;
        }

        private void Dots_Leave(string category)
        {
            foreach (BillboardText t in mActiveTexts)
            {
                if (t == null)
                    continue;
                if (t.UIText == null)
                    continue;
                foreach (ChartItemEffect effect in t.UIText.GetComponents<ChartItemEffect>())
                {
                    if(effect != null)
                        effect.TriggerOut(false);
                }
            }
            mActiveTexts.Clear();
            OnItemLeave(new GraphEventArgs(0,Vector3.zero,new DoubleVector2(0.0,0.0),-1f,category,"",""));
        }

        private void Dots_Click(string category,int idx, Vector2 pos)
        {
            DoubleVector3 point = Data.GetPoint(category, idx);
            Dictionary<int, BillboardText> catgoryTexts;
            BillboardText b;
            if (mTexts.TryGetValue(category, out catgoryTexts) == false)
                return;
            if (catgoryTexts.TryGetValue(idx, out b) == false)
                return;
            foreach (BillboardText t in mActiveTexts)
            {
                if (t == null)
                    continue;
                if (t.UIText == null || t.UIText == b.UIText)
                    continue;
                foreach (ChartItemEffect effect in t.UIText.GetComponents<ChartItemEffect>())
                {
                    if (effect != null)
                        effect.TriggerOut(false);
                }
            }

            mActiveTexts.Clear();

            Text tx = b.UIText;
            if (tx != null)
            {
                ChartItemEvents e = tx.GetComponent<ChartItemEvents>();
                if (e != null)
                {
                    e.OnSelected.Invoke(e.gameObject);
                    mActiveTexts.Add(b);
                }

                /*   if (t != null)
                {
                    foreach (ChartItemEffect effect in t.GetComponents<ChartItemEffect>())
                        effect.TriggerIn(false);
                }*/
            }

            string xString = StringFromAxisFormat(point.x, mHorizontalAxis);
            string yString = StringFromAxisFormat(point.y, mVerticalAxis);
            OnItemSelected(new GraphEventArgs(idx,pos, point.ToDoubleVector2(),(float)point.z, category,xString,yString));
        }

        private void Dots_Hover(string category, int idx, Vector2 pos)
        {

            DoubleVector3 point = Data.GetPoint(category, idx);
            Dictionary<int, BillboardText> catgoryTexts;
            BillboardText b;
            if (mTexts.TryGetValue(category, out catgoryTexts) == false)
                return;
            if (catgoryTexts.TryGetValue(idx, out b) == false)
                return;
            foreach (BillboardText t in mActiveTexts)
            {
                if (t == null)
                    continue;
                if (t.UIText == null || t.UIText == b.UIText)
                    continue;
                foreach (ChartItemEffect effect in t.UIText.GetComponents<ChartItemEffect>())
                {
                    if (effect != null)
                        effect.TriggerOut(false);
                }
            }
            
            mActiveTexts.Clear();

            Text tx = b.UIText;
            if (tx != null)
            {
                ChartItemEvents e = tx.GetComponent<ChartItemEvents>();
                if (e != null)
                {
                    e.OnMouseHover.Invoke(e.gameObject);
                    mActiveTexts.Add(b);
                }

                /*   if (t != null)
                {
                    foreach (ChartItemEffect effect in t.GetComponents<ChartItemEffect>())
                        effect.TriggerIn(false);
                }*/
            }
            string xString = StringFromAxisFormat(point.x, mHorizontalAxis);
            string yString = StringFromAxisFormat(point.y, mVerticalAxis);
            OnItemHoverted(new GraphEventArgs(idx,pos, point.ToDoubleVector2(),(float)point.z, category, xString, yString));
        }

        protected override void OnItemHoverted(object userData)
        {
            base.OnItemHoverted(userData);
            var args = userData as GraphEventArgs;
            mOccupiedCateogies.Add(args.Category);
        }

        protected override void OnItemSelected(object userData)
        {
            base.OnItemSelected(userData);
            var args = userData as GraphEventArgs;
            mOccupiedCateogies.Add(args.Category);
        }

        protected override void OnItemLeave(object userData)
        {
            //base.OnItemLeave(userData);
            GraphEventArgs args = userData as GraphEventArgs;
            if (args == null)
                return;

            string category = args.Category;
            mOccupiedCateogies.Remove(category);
            mOccupiedCateogies.RemoveWhere(x => !Data.HasCategory(x));
            if (mOccupiedCateogies.Count == 0)
            {
                if (NonHovered != null)
                    NonHovered.Invoke();
            }
        }

        void FitCanvas()
        {
            RectTransform trans = GetComponent<RectTransform>();
            GameObject fixPosition = new GameObject();
            mFixPosition = fixPosition;
            ChartCommon.HideObject(fixPosition, hideHierarchy);
            fixPosition.AddComponent<ChartItem>();
            fixPosition.transform.position = transform.position;
            while (gameObject.transform.childCount > 0)
                transform.GetChild(0).SetParent(fixPosition.transform, false);
            fixPosition.transform.SetParent(transform, false);
            fixPosition.transform.localScale = new Vector3(1f, 1f, 1f);
            float widthScale = trans.rect.size.x / WidthRatio;
            float heightScale = trans.rect.size.y / HeightRatio;
            float uniformScale = Math.Min(widthScale, heightScale);
            fixPosition.transform.localScale = new Vector3(uniformScale, uniformScale, uniformScale);
            fixPosition.transform.localPosition = new Vector3(-WidthRatio * uniformScale * 0.5f, -HeightRatio * uniformScale * 0.5f, 0f);
            mLastSetSize = trans.rect.size;
        }

    }
}
