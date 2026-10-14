using System.Collections.Generic;
using UnityEngine;

namespace ITKamianets.Engine.Scene
{
    /// <summary>
    /// Splits a simple polygon (convex or concave, no holes) into triangles by ear clipping.
    /// Room scans give concave floors (L-shaped rooms), so a fan from one corner is not enough.
    /// </summary>
    public static class PolygonTriangulator
    {
        /// <summary>Triangle indices into <paramref name="points"/>, counter-clockwise.</summary>
        public static List<int> Triangulate(IList<Vector2> points)
        {
            var triangles = new List<int>();
            var count = points.Count;
            if (count < 3)
            {
                return triangles;
            }

            var remaining = new List<int>(count);
            for (var i = 0; i < count; i++)
            {
                remaining.Add(i);
            }

            // Work counter-clockwise so "convex corner" has one meaning.
            if (SignedArea(points) < 0f)
            {
                remaining.Reverse();
            }

            var guard = count * count;
            while (remaining.Count > 3 && guard-- > 0)
            {
                var clipped = false;
                for (var i = 0; i < remaining.Count; i++)
                {
                    var a = remaining[(i + remaining.Count - 1) % remaining.Count];
                    var b = remaining[i];
                    var c = remaining[(i + 1) % remaining.Count];

                    if (!IsEar(points, remaining, a, b, c))
                    {
                        continue;
                    }

                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(c);
                    remaining.RemoveAt(i);
                    clipped = true;
                    break;
                }

                // Degenerate input (self-intersecting, collinear): stop instead of looping.
                if (!clipped)
                {
                    break;
                }
            }

            if (remaining.Count == 3)
            {
                triangles.Add(remaining[0]);
                triangles.Add(remaining[1]);
                triangles.Add(remaining[2]);
            }

            return triangles;
        }

        private static bool IsEar(IList<Vector2> points, List<int> remaining, int a, int b, int c)
        {
            if (Cross(points[a], points[b], points[c]) <= 0f)
            {
                return false;
            }

            foreach (var other in remaining)
            {
                if (other == a || other == b || other == c)
                {
                    continue;
                }

                if (Inside(points[other], points[a], points[b], points[c]))
                {
                    return false;
                }
            }

            return true;
        }

        private static float Cross(Vector2 a, Vector2 b, Vector2 c)
        {
            return (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);
        }

        private static bool Inside(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            return Cross(a, b, p) >= 0f && Cross(b, c, p) >= 0f && Cross(c, a, p) >= 0f;
        }

        private static float SignedArea(IList<Vector2> points)
        {
            var area = 0f;
            for (var i = 0; i < points.Count; i++)
            {
                var a = points[i];
                var b = points[(i + 1) % points.Count];
                area += a.x * b.y - b.x * a.y;
            }

            return area * 0.5f;
        }
    }
}
