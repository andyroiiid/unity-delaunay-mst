using System.Collections.Generic;
using UnityEngine;
using DungeonGen;

public class Test : MonoBehaviour
{
    public Material lineMaterial;

    private Camera mainCamera;
    private HashSet<Point> points;
    private HashSet<GameObject> lines;

    void Awake()
    {
        mainCamera = Camera.main;
        points = new HashSet<Point>();
        lines = new HashSet<GameObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"mousePosition: {mousePosition}");
            points.Add(new Point(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y)));

            RegenerateLines();
        }
    }

    void RegenerateLines()
    {
        foreach (var line in lines)
            Destroy(line);
        lines.Clear();

        var triangles = BowyerWatson.Triangulate(points);

        var graph = new HashSet<Edge>();
        foreach (var triangle in triangles)
            graph.UnionWith(triangle.edges);

        var tree = Kruskal.MinimumSpanningTree(graph);

        foreach (var edge in tree)
        {
            Vector3 p1 = new Vector3(edge.a.x, edge.a.y);
            Vector3 p2 = new Vector3(edge.b.x, edge.b.y);
            var line = new GameObject();
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.SetPosition(0, p1);
            lineRenderer.SetPosition(1, p2);
            lines.Add(line);
        }
    }
}
