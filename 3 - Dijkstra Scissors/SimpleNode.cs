using System;
using System.Collections.Generic;
using System.Drawing;

namespace VisualIntelligentScissors
{
    class NormalNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Boolean Visited { get; set; }
        public int gradientWeight { get; set; }

        public NormalNode(int x, int y)
        {
            X = x;
            Y = y;
            Visited = false;
            gradientWeight = 99999;
        }

        public bool equals(NormalNode otherNode)
        {
            if (this == null && otherNode != null) return false;
            if (this != null && otherNode == null) return false;
            if (otherNode.X != this.X) return false;
            if (otherNode.Y != this.Y) return false;
            if (otherNode.Visited != this.Visited) return false;
            if (otherNode.gradientWeight != this.gradientWeight) return false;
            return true;
        }
    }
}
