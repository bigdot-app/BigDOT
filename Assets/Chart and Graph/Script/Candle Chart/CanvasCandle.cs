//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace ChartAndGraph
//{
//    class CanvasCandle : MonoBehaviour, ICandleCreator
//    {
//        private CanvasCandleGraphic CreateCandleGraphic()
//        {
//            GameObject obj = ChartCommon.CreateChartItem();
//            CanvasCandleGraphic graphic = obj.AddComponent<CanvasCandleGraphic>();
//            obj.transform.SetParent(transform, false);
//            obj.transform.position = Vector3.zero;
//            obj.transform.rotation = Quaternion.identity;
//            obj.transform.localScale = new Vector3(1f, 1f, 1f);
//            return graphic;
//        }

//        public void Generate(CandleChart parent, CandleChartData.CandleValue value, CandleChartData.CandleSettings settings)
//        {
//            if(!(parent is ICanvas))
//            {
//                Debug.LogWarning("prefab is meant to be used with canvas candle chart only");
//                return;
//            }
//            CanvasCandleGraphic candle = CreateCandleGraphic();
//            candle.SetCandle(0, value, settings);
//            candle.material = settings.Fill;

//            CanvasCandleGraphic line = CreateCandleGraphic();
//            candle.SetCandle(1, value, settings);
//            candle.material = settings.Line;

//            GameObject obj = ChartCommon.CreateChartItem();
//            obj.transform.SetParent(transform, false);
//            obj.transform.position = Vector3.zero;
//            obj.transform.rotation = Quaternion.identity;
//            obj.transform.localScale = new Vector3(1f, 1f, 1f);

//            CanvasLines lines = obj.AddComponent<CanvasLines>();

//            float max = Mathf.Max(value.Open, value.Close);
//            float min = Mathf.Min(value.Open, value.Close);

//            CanvasLines.LineSegement segment = new CanvasLines.LineSegement(new Vector3[]
//            {
//                new Vector3(settings.LineThickness,value.High),
//                new Vector3(-settings.LineThickness,value.High),
//                new Vector3(-settings.LineThickness,max),
//                new Vector3(-settings.CandleThickness,max),
//                new Vector3(-settings.CandleThickness,min),
//                new Vector3(-settings.LineThickness,min),
//                new Vector3(-settings.LineThickness,value.Low),
//                new Vector3(settings.LineThickness,value.Low),
//                new Vector3(settings.LineThickness,min),
//                new Vector3(settings.CandleThickness,min),
//                new Vector3(settings.CandleThickness,max),
//                new Vector3(settings.LineThickness,max),
//                new Vector3(settings.LineThickness,value.High)
//            });

//            var lst = new List<CanvasLines.LineSegement>();
//            lst.Add(segment);
//            lines.Thickness = settings.OutlineThickness;
//            lines.SetLines(lst);
//            lines.material = settings.Outline;
//        }
//    }
//}
