using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph
{
    /// <summary>
    /// holds common operations of the charting library
    /// </summary>
    public class ChartCommon
    {

        class IntComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }
        }

        private static Material mDefaultMaterial;

        static ChartCommon()
        {
            DefaultIntComparer = new IntComparer();
        }

        internal static float SmoothLerp(float from,float to,float factor)
        {
            return (from * (1f - factor)) + (to * factor);
        }

        internal static GameObject CreateCanvasChartItem()
        {
            GameObject obj = new GameObject("item",typeof(RectTransform));
            obj.AddComponent<ChartItem>();
            return obj;
        }

        internal static GameObject CreateChartItem()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<ChartItem>();
            return obj;
        }

        internal static void HideObject(GameObject obj,bool hideMode)
        {
        //    return;
            if (IsInEditMode == true)
            {
                obj.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInBuild;
                return;
            }
            if (hideMode == false)
            {
                obj.hideFlags = HideFlags.DontSaveInEditor;
                return;
            }
            obj.hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
        }

        internal static float GetAutoLength(AnyChart parent, ChartOrientation orientation)
        {
            return (orientation == ChartOrientation.Vertical) ? ((IInternalUse)parent).InternalTotalWidth : ((IInternalUse)parent).InternalTotalHeight;
        }

        internal static float GetAutoDepth(AnyChart parent,ChartOrientation orientation, ChartDivisionInfo info)
        {
            float depth = ((IInternalUse)parent).InternalTotalDepth;
            if (info.MarkDepth.Automatic == false)
                depth = info.MarkDepth.Value;
            return depth;
        }

        internal static float GetAutoLength(AnyChart parent,ChartOrientation orientation,ChartDivisionInfo info)
        {
            float length = (orientation == ChartOrientation.Vertical) ? ((IInternalUse)parent).InternalTotalWidth : ((IInternalUse)parent).InternalTotalHeight;
            if (info.MarkLength.Automatic == false)
                length = info.MarkLength.Value;
            return length;
        }

        internal static Vector2 Perpendicular(Vector2 v)
        {
            return new Vector2(v.y, -v.x);
        }

        internal static bool SegmentIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
        {
            intersection = new Vector2();

            Vector2 dirA = a2 - a1;
            Vector2 dirB = b2 - b1;

            float dotA = Vector2.Dot(dirA, Perpendicular(dirB));
            if (dotA == 0)
                return false;

            Vector2 dirAB = b1 - a1;
            float t = Vector2.Dot(dirAB, Perpendicular(dirB)) / dotA;
            if (t < 0 || t > 1)
                return false;

            float s = Vector2.Dot(dirAB, Perpendicular(dirA))/ dotA;
            if (s < 0 || s > 1)
                return false;

            intersection = a1 + t * dirA;

            return true;
        }

        internal static Vector2 FromPolarRadians(float angleDeg, float radius)
        {
            float x = radius * Mathf.Cos(angleDeg);
            float y = radius * Mathf.Sin(angleDeg);
            return new Vector2(x, y);
        }

        internal static Vector2 FromPolar(float angleDeg,float radius)
        {
            angleDeg *= Mathf.Deg2Rad;
            float x = radius * Mathf.Cos(angleDeg);
            float y = radius * Mathf.Sin(angleDeg);
            return new Vector2(x, y);
        }
        internal static Rect FixRect(Rect r)
        {
            float x = r.x;
            float y = r.y;
            float width = r.width;
            float height = r.height;
            if(width <0)
            {
                x = r.x + width;
                width = -width;
            }
            if(height < 0)
            {
                y = r.y + height;
                height = -height;
            }
            return new Rect(x, y, width, height);
        }

        internal static Material DefaultMaterial
        {
            get
            {
                if (mDefaultMaterial == null)
                {
                    mDefaultMaterial = new Material(Shader.Find("Standard"));
                    mDefaultMaterial.color = Color.blue;
                }
                return mDefaultMaterial;
            }
        }

        /// <summary>
        /// safely assigns a material to a renderer. if the material is null than the default material is set instead
        /// </summary>
        /// <param name="renderer">the renderer</param>
        /// <param name="material">the material</param>
        /// <returns>true if material is not null, false otherwise</returns>
        internal static bool SafeAssignMaterial(Renderer renderer,Material material,Material defualt)
        {
            Material toSet = material;
            if (toSet == null)
            {
                toSet = defualt;
                if(toSet == null)
                    toSet = DefaultMaterial;
            }
            renderer.sharedMaterial = toSet;
            return material != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newMesh"></param>
        /// <param name="cleanMesh"></param>
        internal static void CleanMesh(Mesh newMesh,ref Mesh cleanMesh)
        {
            if (cleanMesh == newMesh)
                return;
            if (cleanMesh != null)
                ChartCommon.SafeDestroy(cleanMesh);
            cleanMesh = newMesh;
        }

        internal static bool IsInEditMode
        {
            get
            {
                return Application.isPlaying == false && Application.isEditor;
            }
        }

        public static void SafeDestroy(UnityEngine.Object obj)
        {
            if (obj == null)
                return;
            if (Application.isEditor && Application.isPlaying == false)
                UnityEngine.Object.DestroyImmediate(obj);
            else
                UnityEngine.Object.Destroy(obj);
        }
        internal static UIVertex CreateVertex(Vector3 pos, Vector2 uv)
        {
            return CreateVertex(pos, uv, pos.z);
        }

        internal static UIVertex CreateVertex(Vector3 pos, Vector2 uv, float z)
        {
            UIVertex vert = new UIVertex();
            vert.color = Color.white;
            vert.uv0 = uv;
            pos.z = z;
            vert.position = pos;
            return vert;
        }

        internal static float GetTiling(MaterialTiling tiling)
        {
            if (tiling.EnableTiling == false)
                return -1f;
            return tiling.TileFactor;
        }

        internal static void FixBillboardText(ItemLabelsBase labels,BillboardText text)
        {
            float sharpness = Mathf.Clamp(labels.FontSharpness, 1f, 3f);
            text.Scale = 1f / sharpness;
            text.UIText.fontSize = (int)(labels.FontSize * sharpness);
            text.UIText.transform.localScale = new Vector3(text.Scale, text.Scale);
        }
        internal static T EnsureComponent<T>(GameObject obj) where T : Component
        {
            T comp = obj.GetComponent<T>();
            if (comp == null)
                comp = obj.AddComponent<T>();
            return comp;
        }


        static private float DotProduct(Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 ab = b - a;
            Vector2 bc = c - b;
            return Vector2.Dot(ab,bc);
        }

        static private float CrossProduct(Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 ab = b - a;
            Vector2 ac = c - a;
            return ab.x * ac.y - ab.y * ac.x;
        }

        static internal float SegmentPointSqrDistance(Vector2 a, Vector2 b, Vector2 point)
        {
            float dot = DotProduct(a, b, point);

            if (dot > 0)
                return (b - point).sqrMagnitude;

            dot = DotProduct(b, a, point);
            if (dot > 0)
                return (a - point).sqrMagnitude;

            float cross = CrossProduct(a, b, point);
            return (cross * cross) / (a - b).sqrMagnitude;
        }
         
        /* internal static BillboardText CreateBillboardText(Text prefab, Transform parentTransform, string text, float x, float y, float z, float angle, bool hideHirarechy, int fontSize, float sharpness)
         {
             return CreateBillboardText(prefab, parentTransform, text, x, y, z, angle, null, hideHirarechy, fontSize, sharpness);
         }*/
        internal static BillboardText UpdateBillboardText(BillboardText billboardText, Transform parentTransform, string text, float x, float y, float z, float angle, Transform relativeFrom, bool hideHirarechy)
        {
            GameObject UIText = billboardText.UIText.gameObject;
            GameObject billboard = billboardText.gameObject;
            ChartCommon.HideObject(billboard, hideHirarechy);
            TextDirection direction = UIText.GetComponent<TextDirection>();
            Text TextObj = billboardText.UIText;

            if (direction != null)
            {
                TextObj = direction.Text;
                if (relativeFrom != null)
                    direction.SetRelativeTo(relativeFrom, billboard.transform);
                else
                    direction.SetDirection(angle);
            }
           // if (parentTransform != null && billboard.transform.parent != parentTransform)
           // {
            //    billboard.transform.SetParent(parentTransform, false);
           // }
            TextObj.text = text;
            billboard.transform.localPosition = new Vector3(x, y, z);
            return billboardText;
        }

        internal static BillboardText CreateBillboardText(BillboardText item,Text prefab, Transform parentTransform, string text, float x, float y, float z, float angle,Transform relativeFrom,bool hideHirarechy,int fontSize,float sharpness)
        {
            if(item != null)
                return UpdateBillboardText(item, parentTransform, text, x, y, z, angle, relativeFrom, hideHirarechy);
            if (prefab == null || prefab.gameObject == null)
            {
                GameObject g = Resources.Load("Chart And Graph/DefaultText") as GameObject;
                prefab = g.GetComponent<Text>();
            }

            GameObject UIText = (GameObject)GameObject.Instantiate(prefab.gameObject);
            GameObject billboard = new GameObject();
            ChartCommon.HideObject(billboard, hideHirarechy);

            if (parentTransform != null)
            {
                billboard.transform.SetParent(parentTransform, false);
                UIText.transform.SetParent(parentTransform, false);
            }

            BillboardText billboardText = billboard.AddComponent<BillboardText>();
            billboard.AddComponent<ChartItem>();
            TextDirection direction = UIText.GetComponent<TextDirection>();
            Text TextObj = UIText.GetComponent<Text>();

            if (direction != null)
            {
                TextObj = direction.Text;
                if (relativeFrom != null)
                    direction.SetRelativeTo(relativeFrom, billboard.transform);
                else
                    direction.SetDirection(angle);
            }

            if (billboardText == null || TextObj == null)
            {
                SafeDestroy(UIText);
                SafeDestroy(billboard);
                return null;
            }

            sharpness = Mathf.Clamp(sharpness,1f, 3f);
            TextObj.fontSize = (int)(fontSize * sharpness);
            TextObj.horizontalOverflow = HorizontalWrapMode.Overflow;
            TextObj.verticalOverflow = VerticalWrapMode.Overflow;
            TextObj.resizeTextForBestFit = false;
            billboardText.Scale = 1f/ sharpness;
            
            TextObj.text = text;
            billboardText.UIText = TextObj;
            billboardText.Direction = direction;
            if (direction != null)
                billboardText.RectTransformOverride = direction.GetComponent<RectTransform>();
            else
                billboardText.RectTransformOverride = null;
            billboard.transform.localPosition = new Vector3(x, y, z);
            return billboardText;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IEqualityComparer<int> DefaultIntComparer { get; private set; }
    }
}
