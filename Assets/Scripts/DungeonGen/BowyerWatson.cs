using System.Collections.Generic;

namespace DungeonGen
{
    public static class BowyerWatson
    {
        private static Triangle CalcSuperTriangle(IEnumerable<Point> points)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach (var point in points)
            {
                if (point.x < minX) minX = point.x;
                if (point.x > maxX) maxX = point.x;
                if (point.y < minY) minY = point.y;
                if (point.y > maxY) maxY = point.y;
            }

            int dx = (maxX - minX + 1) / 2;

            // just select a random triangle containing all points
            Point p1 = new Point(minX - dx - 1, minY - 1);
            Point p2 = new Point(minX + dx, maxY + (maxY - minY) + 1);
            Point p3 = new Point(maxX + dx + 1, minY - 1);

            return new Triangle(p1, p2, p3);
        }

        public static HashSet<Triangle> Triangulate(IEnumerable<Point> points)
        {
            Triangle superTriangle = CalcSuperTriangle(points);
            HashSet<Triangle> triangulation = new HashSet<Triangle>() { superTriangle };

            foreach (var point in points)
            {
                HashSet<Triangle> badTriangles = new HashSet<Triangle>();
                foreach (var triangle in triangulation)
                {
                    if (triangle.CircumCircleContains(point))
                        badTriangles.Add(triangle);
                }

                HashSet<Edge> polygon = new HashSet<Edge>();
                foreach (var badTriangle in badTriangles)
                {
                    foreach (var edge in badTriangle.edges)
                    {
                        bool isShared = false;
                        foreach (var otherTriangle in badTriangles)
                        {
                            if (badTriangle == otherTriangle)
                                continue;
                            if (otherTriangle.HasEdge(edge))
                                isShared = true;
                        }
                        if (!isShared)
                            polygon.Add(edge);
                    }
                }

                triangulation.ExceptWith(badTriangles);

                foreach (var edge in polygon)
                {
                    triangulation.Add(new Triangle(point, edge.a, edge.b));
                }
            }

            triangulation.RemoveWhere((Triangle t) => t.HasVertexFrom(superTriangle));

            return triangulation;
        }
    }
}
