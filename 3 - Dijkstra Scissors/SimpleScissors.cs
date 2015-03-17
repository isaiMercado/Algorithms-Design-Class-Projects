using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;

namespace VisualIntelligentScissors
{
    public class SimpleScissors : Scissors
    {


        public SimpleScissors()
        {
        }



        public SimpleScissors(GrayBitmap image, Bitmap overlay)
            : base(image, overlay)
        {
        }



        public override void FindSegmentation(IList<Point> points, Pen pen)
        {



            if (Image == null) throw new InvalidOperationException("Set Image property first.");



            using (Graphics g = Graphics.FromImage(Overlay))
            {



                // creating two dimensional grid of Nodes from overlay
                NormalNode[,] allNodes = new NormalNode[Overlay.Width, Overlay.Height];
                for (int col = 0; col < Overlay.Width; col++)
                {
                    for (int row = 0; row < Overlay.Height; row++)
                    {
                        allNodes[col, row] = new NormalNode(col, row);
                    }
                }



                //draw all selected Nodes
                for (int i = 0; i < points.Count; i++)
                {
                    Point temp = points[i];
                    g.DrawEllipse(pen, temp.X, temp.Y, 5, 5);
                }
                Program.MainForm.RefreshImage();



                // for all Nodes selected by the user
                for (int i = 0; i < points.Count; i++)
                {
                    Point start = points[i];
                    Point end = points[(i + 1) % points.Count];

                    NormalNode startNode = allNodes[start.X, start.Y];
                    NormalNode endNode = allNodes[end.X, end.Y];

                    NormalNode currentNode = startNode;

                    // while the current Node is not the end Node
                    while (!currentNode.equals(endNode))
                    {

                        // this minimum pixel wil be filled by north,or south,or east or west Node
                        NormalNode smallestGradientNode = new NormalNode(0, 0);

                        try
                        {

                            // loking for north Node
                            NormalNode northNode = allNodes[currentNode.X, currentNode.Y - 1];
                            if (!northNode.Visited)
                            {
                                smallestGradientNode = northNode;
                                smallestGradientNode.gradientWeight = GetPixelWeight(new Point(northNode.X, northNode.Y));
                            }



                            // looking for southNode Node
                            NormalNode southNode = allNodes[currentNode.X, currentNode.Y + 1];
                            if (!southNode.Visited)
                            {
                                int southGradientWeight = GetPixelWeight(new Point(southNode.X, southNode.Y));
                                if (southGradientWeight < smallestGradientNode.gradientWeight)
                                {
                                    smallestGradientNode = southNode;
                                    smallestGradientNode.gradientWeight = southGradientWeight;
                                }
                            }



                            // looking for south Node
                            NormalNode eastNode = allNodes[currentNode.X - 1, currentNode.Y];
                            if (!eastNode.Visited)
                            {
                                int eastGradientWeight = GetPixelWeight(new Point(eastNode.X, eastNode.Y));
                                if (eastGradientWeight < smallestGradientNode.gradientWeight)
                                {
                                    smallestGradientNode = eastNode;
                                    smallestGradientNode.gradientWeight = eastGradientWeight;
                                }
                            }



                            // looking for westNode Node
                            NormalNode westNode = allNodes[currentNode.X + 1, currentNode.Y];
                            if (!westNode.Visited)
                            {
                                int westGradientWeight = GetPixelWeight(new Point(westNode.X, westNode.Y));
                                if (westGradientWeight < smallestGradientNode.gradientWeight)
                                {
                                    smallestGradientNode = westNode;
                                    smallestGradientNode.gradientWeight = westGradientWeight;
                                }
                            }



                            Overlay.SetPixel(smallestGradientNode.X, smallestGradientNode.Y, Color.Red);
                            //Program.MainForm.RefreshImage();
                            smallestGradientNode.Visited = true;



                            // current Node will be the smallest from north, south, east, west Nodes
                            if (northNode.Visited && southNode.Visited && eastNode.Visited && westNode.Visited)
                            {
                                currentNode = endNode;
                                NormalNode cleanNode;
                                for (int row = 0; row < Overlay.Width; row++)
                                {
                                    for (int col = 0; col < Overlay.Height; col++)
                                    {
                                        cleanNode = allNodes[row, col];
                                        cleanNode.Visited = false;
                                        cleanNode.gradientWeight = 99999;
                                    }
                                }
                            }
                            else
                            {
                                currentNode = smallestGradientNode;
                            }



                        }
                        catch (Exception)
                        {
                            currentNode = endNode;
                            NormalNode cleanNode;
                            for (int row = 0; row < Overlay.Width; row++)
                            {
                                for (int col = 0; col < Overlay.Height; col++)
                                {
                                    cleanNode = allNodes[row, col];
                                    cleanNode.Visited = false;
                                    cleanNode.gradientWeight = 99999;
                                }
                            }
                        }
                    }
                }
                Program.MainForm.RefreshImage();
            }
        }
    }
}
