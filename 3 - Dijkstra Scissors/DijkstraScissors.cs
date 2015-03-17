using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;

namespace VisualIntelligentScissors
{
    public class DijkstraScissors : Scissors
    {



        private PriorityQueue priorityQueue { get; set; }
        private SmartNode[,] AllNodes { get; set; }



        public DijkstraScissors()
        {
        }



        public DijkstraScissors(GrayBitmap image, Bitmap overlay)
            : base(image, overlay)
        {
        }



        public override void FindSegmentation(IList<Point> points, Pen pen)
        {
            if (Image == null) throw new InvalidOperationException("Set Image property first.");


            using (Graphics g = Graphics.FromImage(Overlay))
            {
                //add all Nodes to a 2D array of SmartNodes
                AllNodes = new SmartNode[Overlay.Width, Overlay.Height];
                for (int col = 0; col < Overlay.Width; col++)
                {
                    for (int row = 0; row < Overlay.Height; row++)
                    {
                        AllNodes[col, row] = new SmartNode(col, row);
                    }
                }



                //draw all selected Nodes
                for (int i = 0; i < points.Count; i++)
                {
                    Point temp = points[i];
                    g.DrawEllipse(pen, temp.X, temp.Y, 5, 5);
                }
                Program.MainForm.RefreshImage();



                //for all user-selected Nodes
                for (int i = 0; i < points.Count; i++)
                {
                    List<SmartNode> toBeCleaned = new List<SmartNode>();
                    priorityQueue = new PriorityQueue();

                    Point start = points[i];
                    Point end = points[(i + 1) % points.Count];

                    SmartNode startNode = AllNodes[start.X, start.Y];
                    SmartNode endNode = AllNodes[end.X, end.Y];

                    SmartNode currentNode = startNode;
                    priorityQueue.Add(currentNode);
                    toBeCleaned.Add(currentNode);

                    while (!currentNode.Equals(endNode) && priorityQueue.hasNextNode())
                    {
                        currentNode = priorityQueue.nextNode;
                        currentNode.Visited = true;


                        //checking north Node
                        if (currentNode.Y - 1 < Overlay.Height-1 && currentNode.Y - 1 > 0)
                        {
                            SmartNode northNode = AllNodes[currentNode.X, currentNode.Y - 1];
                            CheckAndAdd(currentNode, northNode);
                            toBeCleaned.Add(northNode);
                        }


                        // checking south Node
                        if (currentNode.Y + 1 < Overlay.Height-1 && currentNode.Y + 1 > 0)
                        {
                            SmartNode southNode = AllNodes[currentNode.X, currentNode.Y + 1];
                            CheckAndAdd(currentNode, southNode);
                            toBeCleaned.Add(southNode);
                        }


                        // checking east Node 
                        if (currentNode.X - 1 < Overlay.Width-1 && currentNode.X - 1 > 0)
                        {
                            SmartNode eastNode = AllNodes[currentNode.X - 1, currentNode.Y];
                            CheckAndAdd(currentNode, eastNode);
                            toBeCleaned.Add(eastNode);
                        }


                        // checking west Node
                        if (currentNode.X + 1 < Overlay.Width-1 && currentNode.X + 1 > 0)
                        {
                            SmartNode westNode = AllNodes[currentNode.X + 1, currentNode.Y];
                            CheckAndAdd(currentNode, westNode);
                            toBeCleaned.Add(westNode);
                        }


                    }
                    

                    // going backwards the nodes to draw the nodes with lowest cost
                    // Program.MainForm.RefreshImage();
                    if (currentNode.Equals(endNode))
                    {
                        SmartNode backNode = endNode;
                        while (backNode.PreviousNode != null)
                        {
                            Overlay.SetPixel(backNode.X, backNode.Y, Color.Red);
                            backNode = backNode.PreviousNode;
                        }
                        Program.MainForm.RefreshImage();
                    }


                    for (int a = 0; a < toBeCleaned.Count; a++)
                    {
                        toBeCleaned[a].Clean();
                    }


                }
            }
        }


        public void CheckAndAdd(SmartNode currentNode, SmartNode otherNode)
        {
            if (!otherNode.Visited)
            {
                if (!priorityQueue.Contains(otherNode)) 
                {
                    otherNode.Cost = currentNode.Cost + GetPixelWeight(new Point(otherNode.X, otherNode.Y));
                    otherNode.PreviousNode = currentNode;
                    priorityQueue.Add(otherNode);
                    //Overlay.SetPixel(otherNode.X, otherNode.Y, Color.Green);
                }
            }
        }


    }


}

