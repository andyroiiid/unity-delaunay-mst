using System;
using UnityEngine;

namespace DungeonGen
{
    public class DuplicatePointException : ApplicationException
    {
        public Point a;
        public Point b;

        public DuplicatePointException(Point a, Point b) : base()
        {
            this.a = a;
            this.b = b;
        }
    }
}
