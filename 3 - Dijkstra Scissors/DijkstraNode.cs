using System;
using System.Collections.Generic;
using System.Drawing;

namespace VisualIntelligentScissors
{
    public class SmartNode : IComparable<SmartNode>
    {



        public int X { get; set; }
        public int Y { get; set; }
        public Boolean Visited { get; set; }
        public int Cost { get; set; }
        public SmartNode PreviousNode { get; set; }



        public SmartNode(int x, int y)
        {
            X = x;
            Y = y;
            Visited = false;
            Cost = -1;
            PreviousNode = null;
        }



        public int CompareTo(SmartNode node)
        {
            return this.Cost.CompareTo(node.Cost);
        }



        public Boolean Equals(SmartNode node)
        {
            if ((this.X == node.X) && (this.Y == node.Y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public void Clean()
        {
            Visited = false;
            Cost = 0;
            PreviousNode = null;
        }
    }
}
