using System.Collections.Generic;

namespace DungeonGen
{
    public class Triangle
    {
        private readonly Point a;
        private readonly Point b;
        private readonly Point c;

        public readonly HashSet<Edge> edges;

        private readonly float circumCenterX;
        private readonly float circumCenterY;
        private readonly float circumRadius2;

        public Triangle(Point a, Point b, Point c)
        {
            // hashing depends on the orderliness of a, b, c
            if (a < b)
            {
                if (b < c)
                {
                    // a, b, c
                    this.a = a;
                    this.b = b;
                    this.c = c;
                }
                else if (a < c)
                {
                    // a, c, b
                    this.a = a;
                    this.b = c;
                    this.c = b;
                }
                else
                {
                    // c, a, b
                    this.a = c;
                    this.b = a;
                    this.c = b;
                }
            }
            else if (a < c)
            {
                // b, a, c
                this.a = b;
                this.b = a;
                this.c = c;
            }
            else if (b < c)
            {
                // b, c, a
                this.a = b;
                this.b = c;
                this.c = a;
            }
            else
            {
                // c, b, a
                this.a = c;
                this.b = a;
                this.c = b;
            }

            edges = new HashSet<Edge>
            {
                new Edge(this.a, this.b),
                new Edge(this.b, this.c),
                new Edge(this.a, this.c)
            };

            float D = (a.x * (b.y - c.y) +
                     b.x * (c.y - a.y) +
                     c.x * (a.y - b.y)) * 2;
            float x = (a.x * a.x + a.y * a.y) * (b.y - c.y) +
                    (b.x * b.x + b.y * b.y) * (c.y - a.y) +
                    (c.x * c.x + c.y * c.y) * (a.y - b.y);
            float y = (a.x * a.x + a.y * a.y) * (c.x - b.x) +
                    (b.x * b.x + b.y * b.y) * (a.x - c.x) +
                    (c.x * c.x + c.y * c.y) * (b.x - a.x);

            circumCenterX = x / D;
            circumCenterY = y / D;
            float dx = a.x - circumCenterX;
            float dy = a.y - circumCenterY;
            circumRadius2 = dx * dx + dy * dy;
        }

        public override bool Equals(object obj)
        {
            return obj is Triangle other && a == other.a && b == other.b && c == other.c;
        }

        public override int GetHashCode()
        {
            var hashCode = 1474027755;
            hashCode = hashCode * -1521134295 + a.GetHashCode();
            hashCode = hashCode * -1521134295 + b.GetHashCode();
            hashCode = hashCode * -1521134295 + c.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"Triangle({a}, {b}, {c})";
        }

        public bool HasEdge(Edge edge)
        {
            return edges.Contains(edge);
        }

        private bool HasVertex(Point point)
        {
            return a == point || b == point || c == point;
        }

        public bool HasVertexFrom(Triangle triangle)
        {
            return HasVertex(triangle.a) || HasVertex(triangle.b) || HasVertex(triangle.c);
        }

        public bool CircumCircleContains(Point point)
        {
            float dx = point.x - circumCenterX;
            float dy = point.y - circumCenterY;
            float distance2 = dx * dx + dy * dy;
            return distance2 < circumRadius2;
        }
    }
}
