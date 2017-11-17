using ChartAndGraph.Axis;
using ChartAndGraph.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph
{
    /// <summary>
    /// this is a base class for all chart types
    /// </summary>
    [Serializable]
    public abstract class AnyChart : MonoBehaviour, IInternalUse
    {
        private bool mGenerating = false;
        Dictionary<int, string> mHorizontalValueToStringMap = new Dictionary<int, string>();
        Dictionary<int, string> mVerticalValueToStringMap = new Dictionary<int, string>();

        public Dictionary<int, string> VerticalValueToStringMap
        {
            get { return mVerticalValueToStringMap; }
        }

        public Dictionary<int, string> HorizontalValueToStringMap
        {
            get { return mHorizontalValueToStringMap; }
        }

        protected virtual Camera TextCameraLink
        {
            get { return null; }
        }

        protected virtual float TextIdleDistanceLink
        {
            get { return 20f; }
        }

        [SerializeField]
        private bool keepOrthoSize = false;

        public bool KeepOrthoSize
        {
            get { return keepOrthoSize; }
            set
            {
                KeepOrthoSize = value;
                GenerateChart();
            }
        }

        [SerializeField]
        private bool vRSpaceText = false;

        public bool VRSpaceText
        {
            get { return vRSpaceText; }
            set
            {
                vRSpaceText = value;
                GenerateChart();
            }
        }

        [SerializeField]
        private float vRSpaceScale = 0.1f;

        public float VRSpaceScale
        {
            get { return vRSpaceScale; }
            set
            {
                vRSpaceScale = value;
                GenerateChart();
            }
        }

        HashSet<object> mHovered = new HashSet<object>();
        protected bool IsUnderCanvas { get; private set; }
        protected bool CanvasChanged { get; private set; }
        protected ItemLabels mItemLabels;
        protected VerticalAxis mVerticalAxis;
        protected HorizontalAxis mHorizontalAxis;
        protected CategoryLabels mCategoryLabels;
        protected GroupLabels mGroupLabels;
        protected GameObject VerticalMainDevisions;
        protected GameObject VerticalSubDevisions;
        protected GameObject HorizontalMainDevisions;
        protected GameObject HorizontalSubDevisions;

        bool mGenerateOnNextUpdate = false;

        private void AxisChanged(object sender, EventArgs e)
        {
            GenerateChart();
        }

        protected virtual void OnPropertyUpdated()
        {
            ValidateProperties();
        }

        private void Labels_OnDataChanged(object sender, EventArgs e)
        {
            OnLabelSettingsSet();
        }

        private void Labels_OnDataUpdate(object sender, EventArgs e)
        {
            OnLabelSettingChanged();
        }

        protected virtual void OnLabelSettingChanged()
        {

        }

        protected abstract float TotalDepthLink
        {
            get;
        }

        protected abstract float TotalHeightLink
        {
            get;
        }

        protected abstract float TotalWidthLink
        {
            get;
        }

        protected virtual double GetScrollOffset(int axis)
        {
            return 0.0;
        }

        protected bool hideHierarchy = true;

        /// <summary>
        /// Keeps all the chart elements hidden from the editor and the inspector 
        /// </summary>
        bool IInternalUse.HideHierarchy { get { return hideHierarchy; } }

        // the axis generated for the chart
        List<IAxisGenerator> mAxis = new List<IAxisGenerator>();

        protected void FixAxisLabels()
        {
            for (int i = 0; i < mAxis.Count; ++i)
                mAxis[i].FixLabels(this);
        }

        protected virtual void OnAxisValuesChanged()
        {

        }

        protected virtual void OnLabelSettingsSet()
        {

        }

        protected virtual void Start()
        {
            if (gameObject.activeInHierarchy == false)
                return;
            DoCanvas(true);
            EnsureTextController();
        }

        void DoCanvas(bool start)
        {
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            bool prev = IsUnderCanvas;
            IsUnderCanvas = parentCanvas != null;
            if (IsUnderCanvas == false)
                return;
            if (start == false)
            {
                if (IsUnderCanvas != prev)
                {
                    CanvasChanged = true;
                    GenerateChart();
                    CanvasChanged = false;
                }
                return;
            }
        }

        void CreateTextController()
        {
            GameObject obj = new GameObject("textController", typeof(RectTransform)); ;
            ChartCommon.HideObject(obj, hideHierarchy);
            obj.transform.SetParent(transform);
            obj.transform.localScale = new Vector3(1f, 1f, 1f);
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localPosition = Vector3.zero;
            TextController = obj.AddComponent<TextController>();
            TextController.mParent = this;
        }

        void EnsureTextController()
        {
            if (TextController != null)
                return;
            CreateTextController();
        }
        protected bool Invalidating
        {
            get { return mGenerateOnNextUpdate; }
        }
        protected virtual void Invalidate()
        {
            if (mGenerating)
                return;
            mGenerateOnNextUpdate = true;
        }

        protected virtual void Update()
        {
            if (gameObject.activeInHierarchy == false)
                return;
            DoCanvas(false);
            if (mGenerateOnNextUpdate == true)
            {
                mGenerateOnNextUpdate = false;
                GenerateChart();
            }
        }

        protected virtual void LateUpdate()
        {

        }
        /// <summary>
        /// used internally, do not call this method
        /// </summary>
        protected virtual void OnValidate()
        {
            if (gameObject.activeInHierarchy == false)
                return;
            ValidateProperties();
            DoCanvas(true);
            EnsureTextController();
        }


        protected TextController TextController { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected abstract LegenedData LegendInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        protected abstract bool HasValues(AxisBase axis);

        /// <summary>
        /// 
        /// </summary>
        protected abstract double MaxValue(AxisBase axis);

        /// <summary>
        /// 
        /// </summary>
        protected abstract double MinValue(AxisBase axis);

        protected virtual void OnEnable()
        {
            ChartItem[] children = GetComponentsInChildren<ChartItem>(true);
            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i] != null)
                {
                    if (children[i].gameObject != gameObject)
                        children[i].gameObject.SetActive(true);
                }
            }
        }

        protected virtual void OnDisable()
        {
            ChartItem[] children = GetComponentsInChildren<ChartItem>();
            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i] != null)
                {
                    if (children[i].gameObject != gameObject)
                        children[i].gameObject.SetActive(false);
                }
            }
        }

        public void GenerateChart()
        {
            if (mGenerating == true)
                return;
            mGenerating = true;
            InternalGenerateChart();
            Transform[] children = gameObject.GetComponentsInChildren<Transform>();
            for(int i=0; i< children.Length; i++)
            {
                Transform t = children[i];
                if (t == null || t.gameObject == null)
                    continue;
                t.gameObject.layer = gameObject.layer;
            }
            mGenerating = false;

        }
        public virtual void InternalGenerateChart()
        {
            if (gameObject.activeInHierarchy == false)
                return;
            mGenerateOnNextUpdate = false;
            if (ChartGenerated != null)
                ChartGenerated();
        }
        protected virtual void ClearChart()
        {
            mHovered.Clear();
            if (TextController != null)
            {
                TextController.DestroyAll();
                TextController.transform.SetParent(transform, false);
            }

            ChartItem[] children = GetComponentsInChildren<ChartItem>();
            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i] != null)
                {
                    RectMask2D mask = children[i].GetComponent<RectMask2D>();
                    if(mask != null)
                    {
                        Debug.Log(mask.gameObject);
                    }
                    if (TextController != null && children[i].gameObject == TextController.gameObject)
                        continue;
                    if (children[i].gameObject != gameObject)
                        ChartCommon.SafeDestroy(children[i].gameObject);
                }
            }
            EnsureTextController();

            for (int i = 0; i < mAxis.Count; i++)
            {
                if (mAxis[i] != null && mAxis[i].This() != null)
                {
                    ChartCommon.SafeDestroy(mAxis[i].GetGameObject());
                }
            }
            mAxis.Clear();

        }

        protected virtual void OnNonHoverted()
        {

        }

        protected virtual void OnItemLeave(object userData)
        {
            if (mHovered.Count == 0)
                return;
            mHovered.Remove(userData);
            if (mHovered.Count == 0)
                OnNonHoverted();
        }

        protected virtual void OnItemSelected(object userData)
        {

        }

        protected virtual void OnItemHoverted(object userData)
        {
            mHovered.Add(userData);
        }

        internal protected virtual IAxisGenerator InternalUpdateAxis(ref GameObject axisObject,AxisBase axisBase, ChartOrientation axisOrientation, bool isSubDiv,bool forceRecreate,double scrollOffset)
        {
            IAxisGenerator res = null;
            if (axisObject == null || forceRecreate || CanvasChanged)
            {
                ChartCommon.SafeDestroy(axisObject);
                GameObject axis = null;
                if (IsUnderCanvas)
                    axis = ChartCommon.CreateCanvasChartItem();
                else
                    axis = ChartCommon.CreateChartItem();
                axis.transform.SetParent(transform, false);
                axis.transform.localScale = new Vector3(1f, 1f, 1f);
                axis.transform.localRotation = Quaternion.identity;
                axis.transform.localPosition = new Vector3();
                axis.layer = gameObject.layer; // put the axis on the same layer as the chart
                ChartCommon.HideObject(axis, hideHierarchy);
                axisObject = axis;
                if (IsUnderCanvas)
                    res = axis.AddComponent<CanvasAxisGenerator>();
                else
                    res = axis.AddComponent<AxisGenerator>();
            }
            else
            {
                if (IsUnderCanvas)
                    res = axisObject.GetComponent<CanvasAxisGenerator>();
                else
                    res = axisObject.GetComponent<AxisGenerator>();
            }
            res.SetAxis(scrollOffset,this, axisBase, axisOrientation, isSubDiv);
            
      //      axisObject.transform.localScale = new Vector3(1f, 1f, 1f);
     //       axisObject.transform.localRotation = Quaternion.identity;
     //       axisObject.transform.localPosition = new Vector3();
            return res;
        }

        protected virtual void ValidateProperties()
        {
            if(mItemLabels!=null)
                mItemLabels.ValidateProperties();
            if (mCategoryLabels != null)
                mCategoryLabels.ValidateProperties();
            if (mGroupLabels != null)
                mGroupLabels.ValidateProperties();
            if (mHorizontalAxis != null)
                mHorizontalAxis.ValidateProperties();
            if (mVerticalAxis != null)
                mVerticalAxis.ValidateProperties();
        }

        protected void GenerateAxis(bool force)
        {
            mAxis.Clear();
            if (mVerticalAxis)
            {
                double scroll = GetScrollOffset(1);
                IAxisGenerator main = InternalUpdateAxis(ref VerticalMainDevisions, mVerticalAxis, ChartOrientation.Vertical, false, force, scroll);
                IAxisGenerator sub = InternalUpdateAxis(ref VerticalSubDevisions, mVerticalAxis, ChartOrientation.Vertical, true, force, scroll);
                if (main != null)
                    mAxis.Add(main);
                if (sub != null)
                    mAxis.Add(sub);
            }
            if (mHorizontalAxis)
            {
                double scroll = GetScrollOffset(0);
                IAxisGenerator main = InternalUpdateAxis(ref HorizontalMainDevisions, mHorizontalAxis, ChartOrientation.Horizontal, false, force,scroll);
                IAxisGenerator sub = InternalUpdateAxis(ref HorizontalSubDevisions, mHorizontalAxis, ChartOrientation.Horizontal, true, force,scroll);
                if (main != null)
                    mAxis.Add(main);
                if (sub != null)
                    mAxis.Add(sub);
            }
        }

        private event Action ChartGenerated;

        #region Internal Use
        event Action IInternalUse.Generated
        {
            add
            {
                ChartGenerated += value;
            }
            remove
            {
                ChartGenerated -= value;
            }
        }

        void IInternalUse.InternalItemSelected(object userData)
        {
            OnItemSelected(userData);
        }

        void IInternalUse.InternalItemLeave(object userData)
        {
            OnItemLeave(userData);
        }

        void IInternalUse.InternalItemHovered(object userData)
        {
            OnItemHoverted(userData);
        }

        void IInternalUse.CallOnValidate()
        {
            OnValidate();
        }

        /// <summary>
        /// Label settings for the chart items
        /// </summary>
        ItemLabels IInternalUse.ItemLabels
        {
            get { return mItemLabels; }
            set
            {
                if (mItemLabels != null)
                {

                    ((IInternalSettings)mItemLabels).InternalOnDataUpdate -= Labels_OnDataUpdate;
                    ((IInternalSettings)mItemLabels).InternalOnDataChanged -= Labels_OnDataChanged;
                }
                mItemLabels = value;
                if (mItemLabels != null)
                {
                    ((IInternalSettings)mItemLabels).InternalOnDataUpdate += Labels_OnDataUpdate;
                    ((IInternalSettings)mItemLabels).InternalOnDataChanged += Labels_OnDataChanged;
                }
                OnLabelSettingsSet();
            }
        }

        VerticalAxis IInternalUse.VerticalAxis
        {
            get
            {
                return mVerticalAxis;
            }
            set
            {
                if (mVerticalAxis != null)
                {
                    ((IInternalSettings)mVerticalAxis).InternalOnDataChanged -= AxisChanged;
                    ((IInternalSettings)mVerticalAxis).InternalOnDataUpdate -= AxisChanged;
                }
                mVerticalAxis = value;
                if (mVerticalAxis != null)
                {
                    ((IInternalSettings)mVerticalAxis).InternalOnDataChanged += AxisChanged;
                    ((IInternalSettings)mVerticalAxis).InternalOnDataUpdate += AxisChanged;
                }
                OnAxisValuesChanged();
            }
        }


        HorizontalAxis IInternalUse.HorizontalAxis
        {
            get
            {
                return mHorizontalAxis;
            }
            set
            {
                if (mHorizontalAxis != null)
                {

                    ((IInternalSettings)mHorizontalAxis).InternalOnDataChanged -= AxisChanged;
                    ((IInternalSettings)mHorizontalAxis).InternalOnDataUpdate -= AxisChanged;
                }
                mHorizontalAxis = value;
                if (mHorizontalAxis != null)
                {
                    ((IInternalSettings)mHorizontalAxis).InternalOnDataChanged += AxisChanged;
                    ((IInternalSettings)mHorizontalAxis).InternalOnDataUpdate += AxisChanged;
                }
                OnAxisValuesChanged();
            }
        }

        /// <summary>
        /// Label settings for the chart categories
        /// </summary>
        CategoryLabels IInternalUse.CategoryLabels
        {
            get { return mCategoryLabels; }
            set
            {
                if (mCategoryLabels != null)
                {

                    ((IInternalSettings)mCategoryLabels).InternalOnDataUpdate -= Labels_OnDataUpdate;
                    ((IInternalSettings)mCategoryLabels).InternalOnDataChanged -= Labels_OnDataChanged;
                }
                mCategoryLabels = value;
                if (mCategoryLabels != null)
                {
                    ((IInternalSettings)mCategoryLabels).InternalOnDataUpdate += Labels_OnDataUpdate;
                    ((IInternalSettings)mCategoryLabels).InternalOnDataChanged += Labels_OnDataChanged;
                }
                OnLabelSettingsSet();
            }
        }


        /// <summary>
        /// Label settings for group labels
        /// </summary>
        GroupLabels IInternalUse.GroupLabels
        {
            get { return mGroupLabels; }
            set
            {
                if (mGroupLabels != null)
                {
                    ((IInternalSettings)mGroupLabels).InternalOnDataUpdate -= Labels_OnDataUpdate;
                    ((IInternalSettings)mGroupLabels).InternalOnDataChanged -= Labels_OnDataChanged;
                }
                mGroupLabels = value;
                if (mGroupLabels != null)
                {
                    ((IInternalSettings)mGroupLabels).InternalOnDataUpdate += Labels_OnDataUpdate;
                    ((IInternalSettings)mGroupLabels).InternalOnDataChanged += Labels_OnDataChanged;
                }
                OnLabelSettingsSet();
            }
        }

        TextController IInternalUse.InternalTextController { get { return TextController; } }
        LegenedData IInternalUse.InternalLegendInfo { get { return LegendInfo; } }

        Camera IInternalUse.InternalTextCamera
        {
            get
            {
                return TextCameraLink;
            }
        }

        float IInternalUse.InternalTextIdleDistance
        {
            get
            {
                return TextIdleDistanceLink;
            }
        }

        bool IInternalUse.InternalHasValues(AxisBase axis)
        {
            return HasValues(axis);
        }
        double IInternalUse.InternalMaxValue(AxisBase axis)
        {
            return MaxValue(axis);
        }
        double IInternalUse.InternalMinValue(AxisBase axis)
        {
            return MinValue(axis);
        }
        float IInternalUse.InternalTotalDepth { get { return TotalDepthLink; } }
        float IInternalUse.InternalTotalWidth { get { return TotalWidthLink; } }
        float IInternalUse.InternalTotalHeight { get { return TotalHeightLink; } }

        protected abstract bool SupportsCategoryLabels
        {
            get;
        }

        protected abstract bool SupportsItemLabels
        {
            get;
        }

        protected abstract bool SupportsGroupLables
        {
            get;
        }

        bool IInternalUse.InternalSupportsCategoryLables
        {
            get { return SupportsCategoryLabels; }
        }

        bool IInternalUse.InternalSupportsGroupLabels
        {
            get { return SupportsGroupLables; }
        }

        bool IInternalUse.InternalSupportsItemLabels
        {
            get { return SupportsItemLabels; }
        }
        #endregion
    }
}
