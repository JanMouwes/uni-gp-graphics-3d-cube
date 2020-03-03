using System;
using System.Text;

namespace MatrixTransformations
{
    public class Vector
    {
        public float x, y, z, w;

        public Vector() : this(0, 0, 0, 1)
        { }

        public Vector(float x, float y) : this(x, y, 1,1)
        { }

        public Vector(float x, float y, float z) : this(x, y, z, 1) 
        { }

        public Vector(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector {x = v1.x + v2.x, y = v1.y + v2.y};
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector { x = v1.x - v2.x, y = v1.y - v2.y };
        }

        public override string ToString()
        {
            return $"X: {x}, Y: {y}, Z: {z}, W: {w}";
        }
    }
}
