using ChartAndGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GraphAnimation : MonoBehaviour
{
    GraphChartBase graphChart;
    public float AnimationTime = 3f;
    public bool ModifyRange = true;
    Dictionary<String, InnerAnimation> mAnimations = new Dictionary<string, InnerAnimation>();

    class InnerAnimation
    {
        public float maxX, minX, maxY, minY;
        public float totalTime = 3f;
        public float next = 0f;
        public string category;
        public List<Vector2> points;
        public int index;

        public void Update(GraphChartBase graphChart)
        {
            if (graphChart == null || points == null || points.Count == 0)
                return;
            if (index >= points.Count)
                return;
            next -= Time.deltaTime;
            if (next <= 0)
            {
                next = totalTime / (float)points.Count;
                Vector2 point = points[index];
                graphChart.DataSource.AddPointToCategoryRealtime(category, point.x, point.y, next);
                ++index;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        graphChart = GetComponent<GraphChartBase>();
    }

    bool IsValidFloat(float val)
    {
        if (float.IsNaN(val))
            return false;
        if (float.IsInfinity(val))
            return false;
        return true;
    }

    public void Animate(string category, List<Vector2> points,float totalTime)
    {
        if (graphChart == null)
            return;
        if (points == null)
            return;
        if (points.Count == 0)
            return;
        InnerAnimation anim = new InnerAnimation();
        anim.maxX = float.MinValue;
        anim.maxY = float.MinValue;
        anim.minX = float.MaxValue;
        anim.minY = float.MaxValue;

        for (int i = 0; i < points.Count; ++i)
        {
            anim.maxX = Mathf.Max(points[i].x, anim.maxX);
            anim.maxY = Mathf.Max(points[i].y, anim.maxY);
            anim.minX = Mathf.Min(points[i].x, anim.minX);
            anim.minY = Mathf.Min(points[i].y, anim.minY);
        }

        if (ModifyRange)
        {
            float maxX = anim.maxX;
            float maxY = anim.maxY;
            float minX = anim.minX;
            float minY = anim.minY;
            foreach (InnerAnimation a in mAnimations.Values)
            {
                maxX = Mathf.Max(maxX, a.maxX);
                maxY = Mathf.Max(maxY, a.maxY);
                minX = Mathf.Min(minX, a.minX);
                minY = Mathf.Min(minY, a.minY);
            }
            IInternalGraphData g = graphChart.DataSource;
            maxX = (float)Math.Max(g.GetMaxValue(0, true),maxX);
            minX = (float)Math.Min(g.GetMinValue(0, true), minX);
            maxY = (float)Math.Max(g.GetMaxValue(1, true), maxY);
            minY = (float)Math.Min(g.GetMinValue(1, true), minY);

            if (IsValidFloat(maxX) && IsValidFloat(maxY) && IsValidFloat(minX) && IsValidFloat(minY))
            {
                graphChart.DataSource.StartBatch();
                graphChart.DataSource.AutomaticHorizontalView = false;
                graphChart.DataSource.AutomaticVerticallView = false;
                graphChart.DataSource.HorizontalViewSize = (maxX - minX);
                graphChart.DataSource.HorizontalViewOrigin = minX;
                graphChart.DataSource.VerticalViewSize = (maxY - minY);
                graphChart.DataSource.VerticalViewOrigin = minY;
                graphChart.DataSource.EndBatch();
            }
        }

        anim.points = points;
        anim.index = 0;
        anim.next = 0;
        anim.totalTime = totalTime;
        anim.category = category;
        graphChart.DataSource.ClearCategory(category);
        mAnimations[category] = anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (graphChart == null)
            return;
        foreach(InnerAnimation anim in mAnimations.Values)
        {
            anim.Update(graphChart);
        }
    }
}
