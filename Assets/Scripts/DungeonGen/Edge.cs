using UnityEngine;

namespace DungeonGen
{
    public class Edge
    {
        public readonly Point a;
        public readonly Point b;

        private float length = -1;
        public float Length
        {
            get
            {
                if (length < 0)
                {
                    float dx = a.x - b.x;
                    float dy = a.y - b.y;
                    length = Mathf.Sqrt(dx * dx + dy * dy);
                }
                return length;
            }
        }

        public Edge(Point a, Point b)
        {
            if (a == b)
            {
                throw new DuplicatePointException(a, b);
            }
            else if (a < b)
            {
                this.a = a;
                this.b = b;
            }
            else
            {
                this.a = b;
                this.b = a;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Edge other && a == other.a && b == other.b;
        }

        public override int GetHashCode()
        {
            var hashCode = 2118541809;
            hashCode = hashCode * -1521134295 + a.GetHashCode();
            hashCode = hashCode * -1521134295 + b.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"Edge({a}, {b})";
        }

        public static int LengthComparison(Edge x, Edge y)
        {
            float lx = x.Length;
            float ly = y.Length;
            if (Mathf.Approximately(lx, ly))
                return 0;
            else if (lx > ly)
                return 1;
            else
                return -1;
        }
    }
}
