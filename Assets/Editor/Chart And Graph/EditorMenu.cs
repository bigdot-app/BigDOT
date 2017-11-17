using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ChartAndGraph
{
    class EditorMenu
    {
        private static void InstanciateCanvas(string path)
        {
            Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
            if (canvases == null || canvases.Length == 0)
            {
                EditorUtility.DisplayDialog("No canvas in scene", "Please add a canvas to the scene and try again", "Ok");
                return;
            }
            Canvas canvas = null;
            foreach(Canvas c in canvases)
            {
                if(c.transform.parent == null)
                {
                    canvas = c;
                    break;
                }
            }

            if (canvas == null)
            {
                EditorUtility.DisplayDialog("No canvas in scene", "Please add a canvas to the scene and try again", "Ok");
                return;
            }
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            GameObject newObj = (GameObject)GameObject.Instantiate(obj);
            newObj.transform.SetParent(canvas.transform,false);
            newObj.name = newObj.name.Replace("(Clone)","");
            Undo.RegisterCreatedObjectUndo(newObj, "Create Object");
        }

        private static void InstanciateWorldSpace(string path)
        {
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            GameObject newObj = (GameObject)GameObject.Instantiate(obj);
            newObj.name = newObj.name.Replace("(Clone)", "");
            Undo.RegisterCreatedObjectUndo(newObj, "Create Object");
        }

        [MenuItem("Tools/Charts/Clear All")]
        public static void ClearChartGarbage()
        {            
            ChartItem[] children = GameObject.FindObjectsOfType<ChartItem>();
            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i] != null)
                {
                    ChartCommon.SafeDestroy(children[i].gameObject);
                }
            }
        }

        [MenuItem("Tools/Charts/Radar/Canvas")]
        public static void AddRadarChartCanvas()
        {
            InstanciateCanvas("Assets/Chart and Graph/Prefabs/MenuPrefabs/2DRadar.prefab");
        }

        [MenuItem("Tools/Charts/Radar/3D")]
        public static void AddRadarChartWorldSpace()
        {
            InstanciateWorldSpace("Assets/Chart and Graph/Prefabs/MenuPrefabs/3DRadar.prefab");
        }

        [MenuItem("Tools/Charts/Bar/Canvas/Simple")]
        public static void AddBarChartSimpleCanvas()
        {
            InstanciateCanvas("Assets/Chart and Graph/Prefabs/MenuPrefabs/BarCanvasSimple.prefab");            
        }

        [MenuItem("Tools/Charts/Bar/Canvas/Multiple Groups")]
        public static void AddBarChartMultipleCanvas()
        {
            InstanciateCanvas("Assets/Chart and Graph/Prefabs/MenuPrefabs/BarCanvasMultiple.prefab");
        }

        [MenuItem("Tools/Charts/Bar/3D/Simple")]
        public static void AddBarChartSimple3D()
        {
            InstanciateWorldSpace("Assets/Chart and Graph/Prefabs/MenuPrefabs/Bar3DSimple.prefab");
        }

        [MenuItem("Tools/Charts/Bar/3D/Multiple Groups")]
        public static void AddBarChartMultiple3D()
        {
            InstanciateWorldSpace("Assets/Chart and Graph/Prefabs/MenuPrefabs/Bar3DMultiple.prefab");
        }

        [MenuItem("Tools/Charts/Torus/Canvas")]
        public static void AddTorusChartCanvas()
        {
            InstanciateCanvas("Assets/Chart and Graph/Prefabs/MenuPrefabs/TorusCanvas.prefab");
        }

        [MenuItem("Tools/Charts/Pie/Canvas")]
        public static void AddPieChartCanvas()
        {
            InstanciateCanvas("Assets/Chart and Graph/Prefabs/MenuPrefabs/PieCanvas.prefab");
        }

        [MenuItem("Tools/Charts/Torus/3D")]
        public static void AddTorusChart3D()
        {
            InstanciateWorldSpace("Assets/Chart and Graph/Prefabs/MenuPrefabs/Torus3D.prefab");
        }

        [MenuItem("Tools/Charts/Pie/3D")]
        public static void AddPieChart3D()
        {
            InstanciateWorldSpace("Assets/Chart and Graph/Prefabs/MenuPrefabs/Pie3D.prefab");
        }

        [MenuItem("Tools/Charts/Graph/Canvas/Simple")]
        public static void AddGraphSimple()
        {
            InstanciateCanvas("Assets/Chart and Graph/Prefabs/MenuPrefabs/GraphSimple.prefab");
        }

        [MenuItem("Tools/Charts/Graph/Canvas/Multiple")]
        public static void AddGraphMultiple()
        {
            InstanciateCanvas("Assets/Chart and Graph/Prefabs/MenuPrefabs/GraphMultiple.prefab");
        }

        [MenuItem("Tools/Charts/Bubble/3D")]
        public static void Add3DBubble()
        {
            InstanciateWorldSpace("Assets/Chart and Graph/Prefabs/MenuPrefabs/3DBubble.prefab");
        }

        [MenuItem("Tools/Charts/Bubble/Canvas")]
        public static void AddCanvasBubble()
        {
            InstanciateCanvas("Assets/Chart and Graph/Prefabs/MenuPrefabs/2DBubble.prefab");
        }

        [MenuItem("Tools/Charts/Graph/3D")]
        public static void AddGraph3D()
        {
            InstanciateWorldSpace("Assets/Chart and Graph/Prefabs/MenuPrefabs/3DGraph.prefab");
        }
        [MenuItem("Tools/Charts/Legend")]
        public static void AddChartLegend()
        {
            InstanciateCanvas("Assets/Chart and Graph/Prefabs/MenuPrefabs/Legend.prefab");
        }
    }
}
