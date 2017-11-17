using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChartAndGraph
{
    /// <summary>
    /// this class is used internally in order to draw lines, line fill and line points into a mesh
    /// </summary>
    public class CanvasLines : MaskableGraphic
    {
        public float Thickness = 2f;
        float innerTile = 1f;
        public float Tiling
        {
            get { return innerTile; }
            set
            {
                innerTile = value;
            }
        }
      //  bool mIsDown = false;
       // Vector2? mLastPosition;
        bool mFillRender = false;
        bool mPointRender = false;
        float mPointSize = 5f;
        Rect mFillRect;
        bool mStretchY;
        Material mCachedMaterial;
        SensitivityControl mControl;
        ChartItemEffect mHoverPrefab;
        ChartItemEffect mCurrentHover = null;
        List<ChartItemEffect> mHoverObjectes = new List<ChartItemEffect>();
        List<ChartItemEffect> mHoverFreeObjects = new List<ChartItemEffect>();
        List<LineSegement> mLines;
        bool mIsMouseIn = false;
        float mMinX, mMinY, mMaxX, mMaxY;
        int mPickedI = -1, mPickedJ = -1;
        Vector2 mLastMousePosition = new Vector2();
        GraphicRaycaster mCaster;

        //bool mUseCache = false;
   //     List<UIVertex> mCached = new List<UIVertex>();

        public event Action<int, Vector2> Hover;
        public event Action<int, Vector2> Click;
        public event Action Leave;



        Rect? ViewRect = null;

        WorldSpaceChartMesh mMesh = null;
        Rect? mUvRect;
        bool mForceMouseMove = false;
        public void SetViewRect(Rect r,Rect uvRect)
        {
            ViewRect = r;
            mUvRect = uvRect;
        }
        public int refrenceIndex = 0;
        public ChartItemEffect LockHoverObject(int index)
        {
            int count = mHoverFreeObjects.Count;
            ChartItemEffect effect = null;
            if (count > 0)
            {
                effect = mHoverFreeObjects[count - 1];
                mHoverFreeObjects.RemoveAt(count - 1);
            }
            else
            {
                if (mHoverPrefab == null)
                    return null;
                GameObject obj = GameObject.Instantiate(mHoverPrefab.gameObject);
                MaskableGraphic g = obj.GetComponent<MaskableGraphic>();
                if(g != null)
                    g.maskable = false;
                ChartCommon.EnsureComponent<ChartItem>(obj);
                obj.transform.SetParent(transform);
                effect = obj.GetComponent<ChartItemEffect>();
                effect.Deactivate += Effect_Deactivate;
            }
            effect.LineTag = index;
            mHoverObjectes.Add(effect);
            return effect;
        }

        private void Effect_Deactivate(ChartItemEffect obj)
        {
            mHoverObjectes.Remove(obj);
            mHoverFreeObjects.Add(obj);
        }

        public void SetHoverPrefab(ChartItemEffect prefab)
        {
            mHoverPrefab = prefab;
        }

        /// <summary>
        /// Sets point render mode
        /// </summary>
        /// <param name="pointSize"></param>
        public void MakePointRender(float pointSize)
        {
            mPointSize = pointSize;
            mPointRender = true;
        }

        /// <summary>
        /// sets inner fill render mode
        /// </summary>
        /// <param name="fillRect"></param>
        /// <param name="stretchY"></param>
        public void MakeFillRender(Rect fillRect, bool stretchY)
        {
            mFillRect = fillRect;
            mFillRender = true;
            mStretchY = stretchY;
        }

        UIVertex[] mTmpVerts = new UIVertex[4];
        /// <summary>
        /// holds line data and pre cacultates normal and speration
        /// </summary>
        internal struct Line
        {
            public Line(Vector3 from, Vector3 to, float halfThickness, bool hasNext, bool hasPrev) : this()
            {

                Vector3 diff = (to - from);
                float magDec = 0;
                if (hasNext)
                    magDec += halfThickness;
                if (hasPrev)
                    magDec += halfThickness;
                Mag = diff.magnitude - magDec * 2;
                Degenerated = false;
                if (Mag <= 0)
                    Degenerated = true;
                Dir = diff.normalized;
                Vector3 add = halfThickness * 2 * Dir;
                if (hasPrev)
                    from += add;
                if (hasNext)
                    to -= add;
                From = from;
                To = to;
                Normal = new Vector3(Dir.y, -Dir.x, Dir.z);
                P1 = From + Normal * halfThickness;
                P2 = from - Normal * halfThickness;
                P3 = to + Normal * halfThickness;
                P4 = to - Normal * halfThickness;
            }

            public bool Degenerated { get; private set; }
            public Vector3 P1 { get; private set; }
            public Vector3 P2 { get; private set; }
            public Vector3 P3 { get; private set; }
            public Vector3 P4 { get; private set; }

            public Vector3 From { get; private set; }
            public Vector3 To { get; private set; }
            public Vector3 Dir { get; private set; }
            public float Mag { get; private set; }
            public Vector3 Normal { get; private set; }
        }

        /// <summary>
        /// represents one line segemenet.
        /// </summary>
        internal class LineSegement
        {
            List<Vector4> mLines = new List<Vector4>();
            public LineSegement(IList<Vector3> lines)
            {
                mLines.AddRange(lines.Select(x=>new Vector4(x.x,x.y,x.z,-1f)));
            }
            public LineSegement(IList<Vector4> lines)
            {
                mLines.AddRange(lines);
            }
            public void ModifiyLines(List<Vector4> v)
            {
                mLines.Clear();
                mLines.AddRange(v);
            }
            public int PointCount
            {
                get
                {
                    if (mLines == null)
                        return 0;
                    return mLines.Count;
                }
            }

            public int LineCount
            {
                get
                {
                    if (mLines == null)
                        return 0;
                    if (mLines.Count < 2)
                        return 0;
                    return mLines.Count - 1;
                }
            }

            public Vector4 getPoint(int index)
            {
                Vector4 p = mLines[index];
                return p;
            }

            public void GetLine(int index, out Vector3 from, out Vector3 to)
            {
                from = mLines[index];
                to = mLines[index + 1];
            }

            public Line GetLine(int index, float halfThickness, bool hasPrev, bool hasNext)
            {
                Vector3 from = mLines[index];
                Vector3 to = mLines[index + 1];
                return new Line(from, to, halfThickness, false, false);
            }

        }

        public CanvasLines()
        {

        }


        internal void ModifyLines(List<Vector4> lines)
        {
            //mUseCache = false;
            if (mLines.Count == 0)
            {
                mLines.Add(new LineSegement(lines.ToArray()));
                return;
            }
            //bool regenerate = false;
            //if(mLines[0].)
            mLines[0].ModifiyLines(lines);
            mMinX = float.PositiveInfinity;
            mMinY = float.PositiveInfinity;
            mMaxX = float.NegativeInfinity;
            mMaxY = float.NegativeInfinity;
            mControl = GetComponentInParent<SensitivityControl>();

            if (mLines != null)
            {
                for (int i = 0; i < mLines.Count; i++)
                {
                    LineSegement seg = mLines[i];
                    int totalPoints = seg.PointCount;
                    for (int j = 0; j < totalPoints; j++)
                    {
                        Vector3 point = seg.getPoint(j);
                        mMinX = Mathf.Min(mMinX, point.x);
                        mMinY = Mathf.Min(mMinY, point.y);
                        mMaxX = Mathf.Max(mMaxX, point.x);
                        mMaxY = Mathf.Max(mMaxY, point.y);
                    }
                }
                //   lines.Add()
            }
            SetVerticesDirty();
            Rebuild(CanvasUpdate.PostLayout);
            mForceMouseMove = true;
        }
        /// <summary>
        /// sets the lines for this renderer
        /// </summary>
        /// <param name="lines"></param>
        internal void SetLines(List<LineSegement> lines)
        {
        //    mUseCache = false;
            mLines = lines;
            mMinX = float.PositiveInfinity;
            mMinY = float.PositiveInfinity;
            mMaxX = float.NegativeInfinity;
            mMaxY = float.NegativeInfinity;
            mControl = GetComponentInParent<SensitivityControl>();

            if (mLines != null)
            {
                for(int i=0; i<mLines.Count; i++)
                {
                    LineSegement seg = mLines[i];
                    int totalPoints = seg.PointCount;
                    for(int j=0; j<totalPoints; j++)
                    {
                        Vector3 point = seg.getPoint(j);
                        mMinX = Mathf.Min(mMinX, point.x);
                        mMinY = Mathf.Min(mMinY, point.y);
                        mMaxX = Mathf.Max(mMaxX, point.x);
                        mMaxY = Mathf.Max(mMaxY, point.y);
                    }
                }
            }

            SetAllDirty();
            mIsMouseIn = false;
            for (int i=0; i<mHoverObjectes.Count; i++)
            {
                mHoverObjectes[i].gameObject.SetActive(false);
                mHoverFreeObjects.Add(mHoverObjectes[i]);
            }
            mHoverObjectes.Clear();

            if(mCurrentHover != null)
            {
                mCurrentHover.gameObject.SetActive(false);
                mHoverFreeObjects.Add(mCurrentHover);
                mCurrentHover = null;
            }
            mPickedI = mPickedJ = -1;
            Rebuild(CanvasUpdate.PreRender);
        }

        protected override void UpdateMaterial()
        {
            base.UpdateMaterial();
            canvasRenderer.SetTexture(material.mainTexture);
        }

        void GetSide(Vector3 point, Vector3 dir,Vector3 normal,float dist,float size,float z,out Vector3 p1,out Vector3 p2)
        {
            point.z = z; 
            point += dir * dist;
            normal *= size;
            p1 = point + normal;
            p2 = point - normal;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ChartCommon.SafeDestroy(mCachedMaterial);
        }
        protected override void OnDisable()
        {
            base.OnDisable();

        }

        public override Material material
        {
            get
            {  
                return base.material;
            }
             
            set
            {
                ChartCommon.SafeDestroy(mCachedMaterial);
                if (value == null)
                { 
                    mCachedMaterial = null;
                    base.material = null;
                    return;
                }
                mCachedMaterial = new Material(value);
                mCachedMaterial.hideFlags = HideFlags.DontSave;
                if (mCachedMaterial.HasProperty("_ChartTiling"))
                    mCachedMaterial.SetFloat("_ChartTiling", Tiling);
                base.material = mCachedMaterial;
            }
        }

        protected void Update()
        {
            Material mat = material;
            if (mCachedMaterial != null && mat!=null && mCachedMaterial.HasProperty("_ChartTiling"))
            {
                if (mCachedMaterial != mat)
                    mCachedMaterial.CopyPropertiesFromMaterial(mat);
                mCachedMaterial.SetFloat("_ChartTiling", Tiling);
            }
            HandleMouseMove(mForceMouseMove);
            mForceMouseMove = false;
        }

        IEnumerable<UIVertex> getDotVeritces()
        {
            if (mLines == null)
                yield break;
            float z = 0f;
            float halfSize = mPointSize * 0.5f;
            for (int i = 0; i < mLines.Count; ++i)
            {
                LineSegement seg = mLines[i];
                int total = seg.PointCount;
                for (int j = 0; j < total; ++j)
                {
                    Vector4 magPoint = seg.getPoint(j);
                    if (magPoint.w == 0f)
                        continue;

                    Vector3 point = (Vector3)magPoint;
                    halfSize = mPointSize * 0.5f;
                    if (magPoint.w >= 0f)
                        halfSize = magPoint.w * 0.5f;
                    Vector3 p1 = point + new Vector3(-halfSize, -halfSize, 0f);
                    Vector3 p2 = point + new Vector3(halfSize, -halfSize, 0f);
                    Vector3 p3 = point + new Vector3(-halfSize, halfSize, 0f);
                    Vector3 p4 = point + new Vector3(halfSize, halfSize, 0f);
                    Vector2 uv1 = new Vector2(0f, 0f);
                    Vector2 uv2 = new Vector2(1f, 0f);
                    Vector2 uv3 = new Vector2(0f, 1f);
                    Vector2 uv4 = new Vector2(1f, 1f);

                    UIVertex v1 = ChartCommon.CreateVertex(p1, uv1, z);
                    UIVertex v2 = ChartCommon.CreateVertex(p2, uv2, z);
                    UIVertex v3 = ChartCommon.CreateVertex(p3, uv3, z);
                    UIVertex v4 = ChartCommon.CreateVertex(p4, uv4, z);

                    yield return v1;
                    yield return v2;
                    yield return v3;
                    yield return v4;
                }
            }
        }
        /*        void AddLineVertices()
                {
                    if (mLines.Count == 0)
                        return;
                    LineSegement seg = mLines[0];
                    int j = seg.LineCount - 1;
                    float halfThickness = Thickness * 0.5f;
                    float z = 0f;

                    for (int i = 0; i < mLines.Count; ++i)
                    {
                        LineSegement seg = mLines[i];
                        int totalLines = seg.LineCount;
                        Line? peek = null;
                        Line? prev = null;
                        float tileUv = 0f;
                        float totalUv = 0f;
                        for (int j = 0; j < totalLines; ++j)
                        {
                            Line line = seg.GetLine(j, halfThickness, false, false);
                            totalUv += line.Mag;
                        }
                        for (int j = 0; j < totalLines; ++j)
                        {
                            Line line;
                            bool hasNext = j + 1 < totalLines;
                            if (peek.HasValue)
                                line = peek.Value;
                            else
                                line = seg.GetLine(j, halfThickness, prev.HasValue, hasNext);
                            peek = null;
                            if (j + 1 < totalLines)
                                peek = seg.GetLine(j + 1, halfThickness, true, j + 2 < totalLines);

                            Vector3 p1 = line.P1;
                            Vector3 p2 = line.P2;
                            Vector3 p3 = line.P3;
                            Vector3 p4 = line.P4;

                            Vector2 uv1 = new Vector2(tileUv * Tiling, 0f);
                            Vector2 uv2 = new Vector2(tileUv * Tiling, 1f);
                            tileUv += line.Mag / totalUv;

                            Vector2 uv3 = new Vector2(tileUv * Tiling, 0f);
                            Vector2 uv4 = new Vector2(tileUv * Tiling, 1f);

                            UIVertex v1 = ChartCommon.CreateVertex(p1, uv1, z);
                            UIVertex v2 = ChartCommon.CreateVertex(p2, uv2, z);
                            UIVertex v3 = ChartCommon.CreateVertex(p3, uv3, z);
                            UIVertex v4 = ChartCommon.CreateVertex(p4, uv4, z);

                            yield return v1;
                            yield return v2;
                            yield return v3;
                            yield return v4;

                            if (peek.HasValue)
                            {
                                float myZ = z + 0.2f;
                                Vector3 a1, a2;
                                GetSide(line.To, line.Dir, line.Normal, halfThickness * 0.5f, halfThickness * 0.6f, v3.position.z, out a1, out a2);
                                yield return v3;
                                yield return v4;
                                yield return ChartCommon.CreateVertex(a1, v3.uv0, myZ);
                                yield return ChartCommon.CreateVertex(a2, v4.uv0, myZ);
                            }
                            if (prev.HasValue)
                            {
                                float myZ = z + 0.2f;
                                Vector3 a1, a2;
                                GetSide(line.From, -line.Dir, line.Normal, halfThickness * 0.5f, halfThickness * 0.6f, v1.position.z, out a1, out a2);
                                yield return ChartCommon.CreateVertex(a1, v1.uv0, myZ);
                                yield return ChartCommon.CreateVertex(a2, v2.uv0, myZ);
                                yield return v1;
                                yield return v2;
                            }
                            z -= 0.05f;
                            prev = line;
                        }
                    }

                }
                void AddDotVertices()
                {
                    if (mLines.Count == 0)
                        return;
                    LineSegement seg = mLines[0];
                    int j = seg.LineCount - 1;
                    float halfSize = mPointSize * 0.5f;
                    float z = 0f;
                    Vector4 magPoint = seg.getPoint(j);
                    Vector3 point = (Vector3)magPoint;
                    halfSize = mPointSize * 0.5f;
                    if (magPoint.w >= 0f)
                        halfSize = magPoint.w * 0.5f;
                    Vector3 p1 = point + new Vector3(-halfSize, -halfSize, 0f);
                    Vector3 p2 = point + new Vector3(halfSize, -halfSize, 0f);
                    Vector3 p3 = point + new Vector3(-halfSize, halfSize, 0f);
                    Vector3 p4 = point + new Vector3(halfSize, halfSize, 0f);
                    Vector2 uv1 = new Vector2(0f, 0f);
                    Vector2 uv2 = new Vector2(1f, 0f);
                    Vector2 uv3 = new Vector2(0f, 1f);
                    Vector2 uv4 = new Vector2(1f, 1f);

                    mCached.Add(ChartCommon.CreateVertex(p1, uv1, z));
                    mCached.Add(ChartCommon.CreateVertex(p2, uv2, z));
                    mCached.Add(ChartCommon.CreateVertex(p3, uv3, z));
                    mCached.Add(ChartCommon.CreateVertex(p4, uv4, z));
                }
                void AddFillVertices()
                {
                    if (mLines.Count == 0)
                        return;
                    float z = 0f;
                    LineSegement seg = mLines[0];
                    int j = seg.LineCount - 1;
                    Vector3 from;
                    Vector3 to;
                    seg.GetLine(j, out from, out to);
                    Vector3 fromBottom = from;
                    Vector3 toBottom = to;
                    fromBottom.y = mFillRect.yMin;
                    toBottom.y = mFillRect.yMin;

                    float fromV = 1f;
                    float toV = 1f;
                    if (mStretchY == false)
                    {
                        fromV = Mathf.Abs((from.y - mFillRect.yMin) / mFillRect.height);
                        toV = Mathf.Abs((to.y - mFillRect.yMin) / mFillRect.height);
                    }
                    float fromU = ((from.x - mFillRect.xMin) / mFillRect.width);
                    float toU = ((to.x - mFillRect.xMin) / mFillRect.width);
                    Vector2 uv1 = new Vector2(fromU, fromV);
                    Vector2 uv2 = new Vector2(toU, toV);
                    Vector2 uv3 = new Vector2(fromU, 0f);
                    Vector2 uv4 = new Vector2(toU, 0f);


                    mCached.Add(ChartCommon.CreateVertex(from, uv1, z));
                    mCached.Add(ChartCommon.CreateVertex(to, uv2, z));
                    mCached.Add(ChartCommon.CreateVertex(fromBottom, uv3, z));
                    mCached.Add(ChartCommon.CreateVertex(toBottom, uv4, z));
                }*/

        Vector2 TransformUv(Vector2 uv)
        {
            if (mUvRect.HasValue == false)
                return uv;
            Rect r = mUvRect.Value;
            float x = r.x + uv.x * r.width;
            float y = r.y + uv.y * r.height;
            return new Vector2(x, y);
        }

        IEnumerable<UIVertex> getFillVeritces()
        {
            if (mLines == null)
                yield break;
            float z = 0f;
            for (int i = 0; i < mLines.Count; ++i)
            {
                LineSegement seg = mLines[i];
                int totalLines = seg.LineCount;
                for (int j = 0; j < totalLines; ++j)
                {
                    Vector3 from;
                    Vector3 to;
                    seg.GetLine(j,out from, out to);

                    Vector2 toTrim = to;
                    Vector2 fromTrim = from;
                    TrimItem(mFillRect.xMin, mFillRect.yMin, mFillRect.xMax, mFillRect.yMin, true, false, ref fromTrim, ref toTrim);
                    to = new Vector3(toTrim.x, toTrim.y, to.z);
                    from = new Vector3(fromTrim.x, fromTrim.y, from.z);
                    Vector3 fromBottom = from;
                    Vector3 toBottom = to;

                    fromBottom.y = mFillRect.yMin;
                    toBottom.y = mFillRect.yMin;

                    float fromV = 1f;
                    float toV = 1f;

                    if (mStretchY == false)
                    {
                        fromV = Mathf.Abs((from.y - mFillRect.yMin) / mFillRect.height);
                        toV = Mathf.Abs((to.y - mFillRect.yMin) / mFillRect.height);
                    }

                    float fromU = ((from.x - mFillRect.xMin) / mFillRect.width);
                    float toU = ((to.x - mFillRect.xMin) / mFillRect.width);
                    Vector2 uv1 = TransformUv(new Vector2(fromU, fromV));
                    Vector2 uv2 = TransformUv(new Vector2(toU, toV));
                    Vector2 uv3 = TransformUv(new Vector2(fromU, 0f));
                    Vector2 uv4 = TransformUv(new Vector2(toU, 0f));

                  //  Vector2 uv1 = new Vector2(fromU, fromV);
                  //  Vector2 uv2 = new Vector2(toU, toV);
                  //  Vector2 uv3 = new Vector2(fromU, 0f);
                  //  Vector2 uv4 = new Vector2(toU, 0f);

                    UIVertex v1 = ChartCommon.CreateVertex(from, uv1, z);
                    UIVertex v2 = ChartCommon.CreateVertex(to, uv2, z);
                    UIVertex v3 = ChartCommon.CreateVertex(fromBottom, uv3, z);
                    UIVertex v4 = ChartCommon.CreateVertex(toBottom, uv4, z);

                    yield return v1;
                    yield return v2;
                    yield return v3;
                    yield return v4;
                }
            }
        }

        IEnumerable<UIVertex> getLineVertices()
        {
            if (mLines == null)
                yield break;
            float halfThickness = Thickness * 0.5f;
            float z = 0f;

            for (int i = 0; i < mLines.Count; ++i)
            {
                LineSegement seg = mLines[i];
                int totalLines = seg.LineCount;
                Line? peek = null;
                Line? prev = null;
                float tileUv = 0f;
                float totalUv = 0f;
                for (int j = 0; j < totalLines; ++j)
                {
                    Line line = seg.GetLine(j, halfThickness, false, false);
                    totalUv += line.Mag;
                }
                for (int j = 0; j < totalLines; ++j)
                {
                    Line line;
                    bool hasNext = j + 1 < totalLines;
                    if (peek.HasValue)
                        line = peek.Value;
                    else
                        line = seg.GetLine(j, halfThickness, prev.HasValue, hasNext);
                    peek = null;
                    if (j + 1 < totalLines)
                        peek = seg.GetLine(j + 1, halfThickness, true, j + 2 < totalLines);

                    Vector3 p1 = line.P1;
                    Vector3 p2 = line.P2;
                    Vector3 p3 = line.P3;
                    Vector3 p4 = line.P4;

                    Vector2 uv1 = new Vector2(tileUv * Tiling, 0f);
                    Vector2 uv2 = new Vector2(tileUv * Tiling, 1f);
                    tileUv += line.Mag / totalUv;

                    Vector2 uv3 = new Vector2(tileUv * Tiling, 0f);
                    Vector2 uv4 = new Vector2(tileUv * Tiling, 1f);

                    UIVertex v1 = ChartCommon.CreateVertex(p1, uv1, z);
                    UIVertex v2 = ChartCommon.CreateVertex(p2, uv2, z);
                    UIVertex v3 = ChartCommon.CreateVertex(p3, uv3, z);
                    UIVertex v4 = ChartCommon.CreateVertex(p4, uv4, z);

                    yield return v1;
                    yield return v2;
                    yield return v3;
                    yield return v4;

                    if (peek.HasValue)
                    {
                        float myZ = z + 0.2f;
                        Vector3 a1, a2;
                        GetSide(line.To, line.Dir, line.Normal, halfThickness * 0.5f, halfThickness * 0.6f, v3.position.z, out a1, out a2);
                        yield return v3;
                        yield return v4;
                        yield return ChartCommon.CreateVertex(a1, v3.uv0, myZ);
                        yield return ChartCommon.CreateVertex(a2, v4.uv0, myZ);
                    }
                    if (prev.HasValue)
                    {
                        float myZ = z + 0.2f;
                        Vector3 a1, a2;
                        GetSide(line.From, -line.Dir, line.Normal, halfThickness * 0.5f, halfThickness * 0.6f, v1.position.z, out a1, out a2);
                        yield return ChartCommon.CreateVertex(a1, v1.uv0, myZ);
                        yield return ChartCommon.CreateVertex(a2, v2.uv0, myZ);
                        yield return v1;
                        yield return v2;
                    }
                    z -= 0.05f;
                    prev = line;
                }
            }
        }

        IEnumerable<UIVertex> getVerices()
        {
        //    if (mUseCache == false || mCached.Count == 0)
        //    {
                IEnumerable<UIVertex> vertices; 
                if (mPointRender)
                    vertices = getDotVeritces();
                else
                {
                    if (mFillRender)
                        vertices = getFillVeritces();
                    else
                        vertices = getLineVertices();
                }
                //mCached.Clear();
          //      mCached.AddRange(vertices);
                //mUseCache = true;
        //    }
            return vertices;
        }

        #if (!UNITY_5_2_0) && (!UNITY_5_2_1)
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);
            vh.Clear();
            int vPos = 0;
            foreach (UIVertex v in getVerices())
            {
                mTmpVerts[vPos++] = v;
                if (vPos == 4)
                {
                    UIVertex tmp = mTmpVerts[2];
                    mTmpVerts[2] = mTmpVerts[3];
                    mTmpVerts[3] = tmp;
                    vPos = 0;
                    vh.AddUIVertexQuad(mTmpVerts);
                }
            }
        }
        #endif
#pragma warning disable 0672

        protected override void OnPopulateMesh(Mesh m)
        {
            if (mMesh == null)
                mMesh = new WorldSpaceChartMesh(1);
            else
                mMesh.Clear();
            int vPos = 0;
            foreach (UIVertex v in getVerices())
            {
                mTmpVerts[vPos++] = v;
                if(vPos == 4)
                {
                    vPos = 0;
                    mMesh.AddQuad(mTmpVerts[0], mTmpVerts[1], mTmpVerts[2], mTmpVerts[3]);
                }
            }

            mMesh.ApplyToMesh(m);
        }
        
        void PickLine(Vector3 mouse, out int segment, out int line)
        {
            float minDist = Mathf.Infinity;
            segment = -1;
            line = -1;

            if (mLines == null)
            {
                return;
            }
            for (int i = 0; i < mLines.Count; ++i)
            {
                LineSegement seg = mLines[i];
                int total = seg.LineCount;
                for (int j = 0; j < total; ++j)
                {
                    Vector3 from;
                    Vector3 to;
                    seg.GetLine(j, out from, out to);
                    float dist = ChartCommon.SegmentPointSqrDistance(from,to,mouse);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        segment = i;
                        line = j;
                    }
                }
            }
            float sensitivity = 10f;
            if (mControl != null)
                sensitivity = mControl.Sensitivity;
            float thresh = (Thickness + sensitivity);

            if ((ViewRect.HasValue && !ViewRect.Value.Contains(mouse)) ||  minDist > thresh * thresh)
            {
                segment = -1;
                line = -1;
            }
        }

        void Pick(Vector3 mouse, out int i, out int j)
        {
            if (mPointRender)
                PickDot(mouse, out i, out j);
            else
                PickLine(mouse, out i, out j);
            if(j>=0)
                j += refrenceIndex;
        }

        void PickDot(Vector3 mouse, out int segment , out int point)
        {
            float minDist = Mathf.Infinity;
            segment = -1;
            point = -1;
            if (mLines == null)
                return;
            float mag = mPointSize;
            for (int i = 0; i < mLines.Count; ++i)
            {
                LineSegement seg = mLines[i];
                int total = seg.PointCount;
                for (int j = 0; j < total; ++j)
                {
                    Vector4 p = seg.getPoint(j);
                    if (p.w == 0f)
                        continue;
                    float dist = (mouse - ((Vector3)p)).sqrMagnitude;
                    if(dist < minDist)
                    {
                        mag = p.w;
                        if (mag < 0f)
                            mag = mPointSize;
                        minDist = dist;
                        segment = i;
                        point = j;
                    }
                    
                }
            }

            float sensitivity = 10f;

            if (mControl != null)
                sensitivity = mControl.Sensitivity;
            
            float thresh = mag + sensitivity;
            if ((ViewRect.HasValue && !ViewRect.Value.Contains(mouse)) || minDist > thresh * thresh)
            {
                segment = -1;
                point = -1;
            }
        }

        void SetUpAllHoverObjects()
        {
            if (mHoverObjectes == null)
                return;
            for (int i = 0; i < mHoverObjectes.Count; i++)
                SetUpHoverObject(mHoverObjectes[i]);
        }

        void SetUpHoverObject(ChartItemEffect hover)
        {
            if (hover == null)
                return;
            if (mLines == null || mLines.Count == 0)
                return;
            int index = hover.LineTag - refrenceIndex;
            if (index < 0)
                return;

            if (mPointRender)
            {
                if (index >= mLines[0].PointCount)
                    return;
                Vector4 point = mLines[0].getPoint(index);
                RectTransform transform = hover.GetComponent<RectTransform>();
                transform.localScale = new Vector3(1f, 1f, 1f);
                float size = mPointSize;
                if (point.w >= 0f)
                    size = point.w;
                transform.sizeDelta = new Vector2(size, size);
                transform.anchoredPosition3D = new Vector3(point.x,point.y,0f);
            }
            else
            {
                if (index >= mLines[0].LineCount)
                    return;
                Vector3 from;
                Vector3 to;
                mLines[0].GetLine(index, out from, out to);

                if (ViewRect.HasValue)
                {
                    Vector2 vFrom = from;
                    Vector2 vTo = to;
                    TrimLine(ViewRect.Value, ref vFrom, ref vTo);
                    from = new Vector3(vFrom.x, vFrom.y, from.z);
                    to = new Vector3(vTo.x, vTo.y, to.z);
                }

                Vector3 dir = (to - from);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                RectTransform transform = hover.GetComponent<RectTransform>();
                transform.sizeDelta = new Vector2(dir.magnitude,Thickness);
                transform.localScale = new Vector3(1f, 1f, 1f);
                transform.localRotation = Quaternion.Euler(0f,0f,angle);
                Vector3 point = (from + to) * 0.5f;
                transform.anchoredPosition3D = new Vector3(point.x, point.y, 0f);
            }
        }

        void TrimItem(float x1, float y1 ,float x2,float y2,bool xAxis,bool oposite, ref Vector2 from, ref Vector2 to)
        {
            Vector2 seg1 = new Vector2(x1, y1);
            Vector2 seg2 = new Vector2(x2, y2);
            Vector2 point;
            if (ChartCommon.SegmentIntersection(seg1, seg2, from, to, out point) == false)
                return;
            if(xAxis)
            {
                if ((to.y > from.y) ^ oposite)
                    from = point;
                else
                    to = point;
                return;
            }
            if ((to.x > from.x) ^ oposite)
                from = point;
            else
                to = point;
        }

        void TrimLine(Rect r,ref Vector2 from, ref Vector2 to)
        {
            TrimItem(r.xMin, r.yMin, r.xMax, r.yMin, true, false, ref from, ref to);
            TrimItem(r.xMin, r.yMax, r.xMax, r.yMax, true, true, ref from, ref to);
            TrimItem(r.xMin, r.yMin, r.xMin, r.yMax, false, false, ref from, ref to);
            TrimItem(r.xMax, r.yMin, r.xMax, r.yMax, false, true, ref from, ref to);
        }
        void TriggerOut(ChartItemEffect hover)
        {
            hover.TriggerOut(true);
            ChartMaterialController control = hover.GetComponent<ChartMaterialController>();
            if(control)
                control.TriggerOff();
        }

        void TriggerIn(ChartItemEffect hover)
        {

            hover.TriggerIn(false);
            ChartMaterialController control = hover.GetComponent<ChartMaterialController>();
            if (control)
                control.TriggerOn();
        }

        void DoMouse(Vector3 mouse, bool leave,bool force)
        {
            if (mLines == null)
                return;

            int prevI = mPickedI;
            int prevJ = mPickedJ;

            if (leave)
            {
                
                mPickedI = -1;
                mPickedJ = -1;
            }
            else
                Pick(mouse, out mPickedI, out mPickedJ);

            if(prevI != mPickedI || prevJ != mPickedJ)
            {

                if (mCurrentHover != null)
                {
                    TriggerOut(mCurrentHover);
                    if (mPickedI == -1 && mPickedJ == -1)
                    {
                        if (Leave != null)
                            Leave();
                    }
                    mCurrentHover = null;
                }

                if (mPickedI != -1 && mPickedJ != -1)
                {
                    mCurrentHover = LockHoverObject(mPickedJ);
                    if (mCurrentHover == null)
                        return;
                    if (Hover != null)
                        Hover(mPickedJ, mouse);
                    mCurrentHover.gameObject.SetActive(true);
                    SetUpHoverObject(mCurrentHover);
                    TriggerIn(mCurrentHover);
                }
            }

        }

        public void HandleMouseMove()
        {
            HandleMouseMove(false);
        }

        public void HandleMouseMove(bool force)
        {
            mCaster = GetComponentInParent<GraphicRaycaster>();
            if (mCaster == null)
                return;
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, mCaster.eventCamera, out mousePos);
             //DoMouse(mousePos, false);
            float sensitivity = 10f;
            if (mControl != null)
                sensitivity = mControl.Sensitivity;
            float thresh = Mathf.Max(Thickness, mPointSize) + sensitivity;

            
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseDown();
            }
            else
            {
        //        mIsDown = false;
            }

            if (force)
            {
                SetUpAllHoverObjects();
            }

            if (mousePos.x < mMinX - thresh || mousePos.y < mMinY- thresh || mousePos.x > mMaxX+ thresh || mousePos.y > mMaxY+ thresh)
                  {
                      if(mIsMouseIn)
                      {
                          mIsMouseIn = false;
                          DoMouse(mousePos, true,force);
                      }
                      return;
                  }
                  else
                  {
                      if (mIsMouseIn == false)
                      {
                          mIsMouseIn = true;
                          mLastMousePosition = mousePos;
                          DoMouse(mousePos, false, force);
                      }
                      else
                      {
                          if(((mLastMousePosition - mousePos).sqrMagnitude > 1) || force)
                          {
                              mLastMousePosition = mousePos;
                              DoMouse(mousePos, false, force); 
                          }
                      }
                  }

        }
        private void HandleMouseDown()
        {
            if (mPickedI != -1 && mPickedJ != -1)
            {
                if (Click != null)
                {
                    mCaster = GetComponentInParent<GraphicRaycaster>();
                    if (mCaster == null)
                        return;
                    Vector2 mousePos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, mCaster.eventCamera, out mousePos);
                    Vector3 pos = transform.InverseTransformPoint(mousePos);
                    Click(mPickedJ, pos);
                }
            }
            else
            {
             //   mIsDown = true;
            }
        }

#pragma warning restore 0672

    }
}
