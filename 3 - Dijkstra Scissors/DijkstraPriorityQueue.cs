using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace VisualIntelligentScissors
{
    class PriorityQueue
    {



        private List<SmartNode> node;



        public PriorityQueue()
        {
            node = new List<SmartNode>();
        }



        public Boolean hasNextNode()
        {
            if (node.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public SmartNode nextNode
        {
            get
            {
                SmartNode temp = null;
                int minimumCost = 999999;
                int index = -1;

                for (int i = 0; i < node.Count; i++)
                {
                    if (node[i].Cost < minimumCost)
                    {
                        temp = node[i];
                        index = i;
                        minimumCost = temp.Cost;
                    }
                }

                if (index != -1)
                    node.RemoveAt(index);

                //Console.WriteLine("count " + node.Count);

                return temp;
            }
        }



        public void Add(SmartNode newNode)
        {
            node.Add(newNode);
        }



        public Boolean Contains(SmartNode otherNode)
        {
            for (int i = 0; i < node.Count; i++)
            {
                if (node[i].Equals(otherNode))
                {
                    return true;
                }
            }
            return false;
        }


        public void Remove(SmartNode otherNode) 
        {
            node.Remove(otherNode);
        }


    }
}
