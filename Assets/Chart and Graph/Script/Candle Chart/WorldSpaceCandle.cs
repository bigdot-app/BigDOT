//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace ChartAndGraph
//{
//    class WorldSpaceCandle : MonoBehaviour, ICandleCreator
//    {
//        public GameObject Prefab;

//        GameObject CreatePrefab(float centerX,float fromY, float width,float toY)
//        {
//            GameObject obj = GameObject.Instantiate(Prefab);
//            ChartCommon.EnsureComponent<ChartItem>(obj);
//            obj.transform.SetParent(transform, false);
//            float centerY = (fromY + toY) * 0.5f;
//            float height = Mathf.Abs(fromY - toY);
//            obj.transform.position = new Vector3(centerX,centerY,0f);
//            obj.transform.rotation = Quaternion.identity;
//            obj.transform.localScale = new Vector3(width*2f, height, 1f);
//            return obj;
//        }
        
//        void SetMaterial(GameObject obj, Material mat)
//        {
//            Renderer rend = obj.GetComponent<Renderer>();
//            if (rend != null)
//                rend.material = mat;
//        }
//        public void Generate(CandleChart parent, CandleChartData.CandleValue value, CandleChartData.CandleSettings settings)
//        {
//            if ((parent is ICanvas))
//            {
//                Debug.LogWarning("prefab is meant not meant to be used with canvas candle chart");
//                return;
//            }

//            float max = value.Max;
//            float min = value.Min;

//            GameObject upper = CreatePrefab(0f, value.High,settings.LineThickness,max);
//            GameObject lower = CreatePrefab(0f, value.Low, settings.LineThickness, min);
//            GameObject candle = CreatePrefab(0f, min, settings.LineThickness, max);

//            SetMaterial(upper, settings.Line);
//            SetMaterial(lower, settings.Line);
//            SetMaterial(candle, settings.Fill);
//        }
//    }
//}
