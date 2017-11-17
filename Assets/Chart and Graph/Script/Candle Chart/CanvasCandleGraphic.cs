//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UnityEngine.UI;

//namespace ChartAndGraph
//{
//    class CanvasCandleGraphic : MaskableGraphic
//    {
//        CandleChartData.CandleSettings mCandleSettings;
//        CandleChartData.CandleValue? mCandle = null;

//        int mPart;
//        UIVertex[] mTmpVerts = new UIVertex[4];
//        public void ClearCandle()
//        {
//            mCandle = null;
//            SetAllDirty();
//            Rebuild(CanvasUpdate.PreRender);
//        }

//        public void SetCandle(int part,CandleChartData.CandleValue candle, CandleChartData.CandleSettings settings)
//        {
//            mPart = part;
//            mCandle = candle;
//            mCandleSettings = settings;
//            SetAllDirty();
//            Rebuild(CanvasUpdate.PreRender);
//        }

//        IEnumerable<UIVertex> getCandle()
//        {
//            UIVertex v = new UIVertex();
//            float max = Mathf.Max(mCandle.Value.Open, mCandle.Value.Close);
//            float min = Mathf.Min(mCandle.Value.Open, mCandle.Value.Close);
//            v.position = new Vector3(-mCandleSettings.CandleThickness, max, 0f);
//            v.uv0 = new Vector2(0f, 0f);
//            yield return v;
//            v.position = new Vector3(mCandleSettings.CandleThickness, max, 0f);
//            v.uv0 = new Vector2(1f, 0f);
//            yield return v;
//            v.position = new Vector3(mCandleSettings.CandleThickness, min, 0f);
//            v.uv0 = new Vector2(1f, 1f);
//            yield return v;
//            v.position = new Vector3(-mCandleSettings.CandleThickness, min, 0f);
//            v.uv0 = new Vector2(0f, 1f);
//            yield return v;
//        }

//        IEnumerable<UIVertex> getLine()
//        {
//            UIVertex v = new UIVertex();
//            float max = Mathf.Max(mCandle.Value.Open, mCandle.Value.Close);
//            float min = Mathf.Min(mCandle.Value.Open, mCandle.Value.Close);

//            v.position = new Vector3(-mCandleSettings.LineThickness, mCandle.Value.High, 0f);
//            v.uv0 = new Vector2(0f, 0f);
//            yield return v;
//            v.position = new Vector3(mCandleSettings.LineThickness, mCandle.Value.High, 0f);
//            v.uv0 = new Vector2(1f, 0f);
//            yield return v;
//            v.position = new Vector3(mCandleSettings.LineThickness, max, 0f);
//            v.uv0 = new Vector2(1f, 1f);
//            yield return v;
//            v.position = new Vector3(-mCandleSettings.LineThickness, max, 0f);
//            v.uv0 = new Vector2(0f, 1f);
//            yield return v;

//            v.position = new Vector3(-mCandleSettings.LineThickness, min, 0f);
//            v.uv0 = new Vector2(0f, 0f);
//            yield return v;
//            v.position = new Vector3(mCandleSettings.LineThickness, min, 0f);
//            v.uv0 = new Vector2(1f, 0f);
//            yield return v;
//            v.position = new Vector3(mCandleSettings.LineThickness, mCandle.Value.Low, 0f);
//            v.uv0 = new Vector2(1f, 1f);
//            yield return v;
//            v.position = new Vector3(-mCandleSettings.LineThickness, mCandle.Value.Low, 0f);
//            v.uv0 = new Vector2(0f, 1f);
//            yield return v;
//        }

//        IEnumerable<UIVertex> getVerices()
//        {
//            if (mCandle.HasValue == false)
//                return new UIVertex[0];
//            if(mPart == 0)
//                return getCandle();
//            return getLine();
//        }

//#if (!UNITY_5_2_0) && (!UNITY_5_2_1)
//        protected override void OnPopulateMesh(VertexHelper vh)
//        {
//            base.OnPopulateMesh(vh);
//            vh.Clear();
//            int vPos = 0;
//            foreach (UIVertex v in getVerices())
//            {
//                mTmpVerts[vPos++] = v;
//                if (vPos == 4)
//                {
//                    UIVertex tmp = mTmpVerts[2];
//                    mTmpVerts[2] = mTmpVerts[3];
//                    mTmpVerts[3] = tmp;
//                    vPos = 0;
//                    vh.AddUIVertexQuad(mTmpVerts);
//                }
//            }
//        }
//#endif
//#pragma warning disable 0672
//        protected override void OnPopulateMesh(Mesh m)
//        {
//            WorldSpaceChartMesh mesh = new WorldSpaceChartMesh(1);
//            int vPos = 0;
//            foreach (UIVertex v in getVerices())
//            {
//                mTmpVerts[vPos++] = v;
//                if (vPos == 4)
//                {
//                    vPos = 0;

//                    mesh.AddQuad(mTmpVerts[0], mTmpVerts[1], mTmpVerts[2], mTmpVerts[3]);
//                }
//            }
//            mesh.ApplyToMesh(m);
//        }
//#pragma warning restore 0672
//    }
//}
