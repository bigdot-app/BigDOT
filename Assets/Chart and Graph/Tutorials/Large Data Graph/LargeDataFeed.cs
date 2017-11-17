using UnityEngine;
using ChartAndGraph;
using System.Collections.Generic;
using System;

public class LargeDataFeed : MonoBehaviour, IComparer<DoubleVector2>
{
    List<DoubleVector2> mData = new List<DoubleVector2>();
    double pageSize = 50f;
    double currentPagePosition = 0.0;
    GraphChartBase graph;
    void Start()
    {
        graph = GetComponent<GraphChartBase>();
        double x = 0f;
        for (int i = 0; i < 250000; i++)    // initialize with random data
        {
            mData.Add(new DoubleVector2(x, UnityEngine.Random.value));
            x += UnityEngine.Random.value * 10f;
        }
        LoadPage(currentPagePosition); // load the page at position 0
    }

    int FindClosestIndex(double position) // if you want to know what is index is currently displayed . use binary search to find it
    {
        //NOTE :: this method assumes your data is sorted !!! 
        int res = mData.BinarySearch(new DoubleVector2(position, 0.0), this);
        if (res >= 0)
            return res;
        return ~res;
    }


    void findPointsForPage(double position, out int start, out int end) // given a page position , find the right most and left most indices in the data for that page. 
    {
        int index = FindClosestIndex(position);
        int i = index;
        double endPosition = position + pageSize;
        double startPosition = position - pageSize;

        //starting from the current index , we find the page boundries
        for (start = index; start > 0; start--)
        {
            if (mData[i].x < startPosition) // take the first point that is out of the page. so the graph doesn't break at the edge
                break;
        }
        for (end = index; end < mData.Count; end++)
        {
            if (mData[i].x > endPosition) // take the first point that is out of the page
                break;
        }
    }
    private void Update()
    {
        if (graph != null)
        {
            //check the scrolling position of the graph. if we are past the view size , load a new page
            double pageStartThreshold = currentPagePosition - pageSize;
            double pageEndThreshold = currentPagePosition + pageSize - graph.DataSource.HorizontalViewSize;
            if (graph.HorizontalScrolling < pageStartThreshold || graph.HorizontalScrolling > pageEndThreshold)
            {
                LoadPage(graph.HorizontalScrolling);
            }
        }
    }
    void LoadPage(double pagePosition)
    {

        if (graph != null)
        {

            Debug.Log("Loading page :" + pagePosition);
            graph.DataSource.StartBatch(); // call start batch 
            graph.DataSource.HorizontalViewOrigin = 0;
            int start, end;
            findPointsForPage(pagePosition, out start, out end); // get the page edges
            graph.DataSource.ClearCategory("Player 1"); // clear the cateogry
            for (int i = start; i < end; i++) // load the data
                graph.DataSource.AddPointToCategory("Player 1", mData[i].x, mData[i].y);
            graph.DataSource.EndBatch();
            graph.HorizontalScrolling = pagePosition;
        }
        currentPagePosition = pagePosition;
    }

    public int Compare(DoubleVector2 x, DoubleVector2 y)
    {
        if (x.x < y.x)
            return -1;
        if (x.x < y.x)
            return 1;
        return 0;
    }
}

