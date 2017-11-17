using UnityEngine;
using System.Collections;
using ChartAndGraph;
using System.Collections.Generic;

public class MultipleGraphDemo : MonoBehaviour
{

    public GraphChart Graph;
    public GraphAnimation Animation;
    public int TotalPoints = 5;

    void Start()
    {
        if (Graph == null) // the ChartGraph info is obtained via the inspector
            return;
        List<Vector2> animationPoints = new List<Vector2>();
        float x = 0f;
        Graph.HorizontalValueToStringMap.Add(10, "Ten");
        Graph.VerticalValueToStringMap.Add(10, "$$");
        Graph.DataSource.StartBatch(); // calling StartBatch allows changing the graph data without redrawing the graph for every change
        Graph.DataSource.ClearCategory("Player 1"); // clear the "Player 1" category. this category is defined using the GraphChart inspector
        Graph.DataSource.ClearCategory("Player 2"); // clear the "Player 2" category. this category is defined using the GraphChart inspector
        for (int i = 0; i < TotalPoints; i++)  //add random points to the graph
        {
            Graph.DataSource.AddPointToCategory("Player 1", x, Random.value * 20f + 10f); // each time we call AddPointToCategory 
            animationPoints.Add(new Vector2(x, Random.value * 10f));
            //            Graph.DataSource.AddPointToCategory("Player 2", x, Random.value * 10f); // each time we call AddPointToCategory 
            x += Random.value * 3f;

        }
        Graph.DataSource.EndBatch(); // finally we call EndBatch , this will cause the GraphChart to redraw itself
        if (Animation != null)
        {
            Animation.Animate("Player 2",animationPoints,3f);
        }
    }
}
