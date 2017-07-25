using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class Vector2<T>
    {
        public T x;
        public T y;

        public Vector2(T x, T y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
