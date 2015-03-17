using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data.Linq;
using System.Linq;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        private static List<OutterPoint> upperPerimeter; // north half of list
        private static List<OutterPoint> lowerPerimeter; // south half of list

        private static PointF lowestXPoint;
        private static PointF highestXPoint;

        private static int UP = 1; // flag to tell function is that it is solving upper points
        private static int DOWN = 0; // flag to tell function that it is solving lower points

        public void Solve(PictureBox box, Graphics g, List<PointF> pointList)
        {
            ConvexHullSolver.upperPerimeter = new List<OutterPoint>();
            ConvexHullSolver.lowerPerimeter = new List<OutterPoint>();

            ExtremeXPoints points = new ExtremeXPoints();
            points.setXExtremePoints(pointList); // get points with lower x value and greatest x value

            SplitedLists lists = new SplitedLists();
            lists.setSouthAndNorthLists(pointList, points.Left, points.Right); // splits list in north and south lists 

            ConvexHullSolver.lowestXPoint = points.Left;
            ConvexHullSolver.highestXPoint = points.Right;

            ConvexHullSolver.upperPerimeter.Add(new OutterPoint(ConvexHullSolver.highestXPoint, points.Right)); //saving lowest x point and highest x point to 
                                                                                                                //upper perimeter list
            ConvexHullSolver.upperPerimeter.Add(new OutterPoint(ConvexHullSolver.highestXPoint, points.Left));
            ConvexHullSolver.lowerPerimeter.Add(new OutterPoint(ConvexHullSolver.lowestXPoint, points.Left));  //saving lowest x point and highest x point to 
                                                                                                                //lower perimeter list
            ConvexHullSolver.lowerPerimeter.Add(new OutterPoint(ConvexHullSolver.lowestXPoint, points.Right));

            solving(box, g, lists.NorthList, points.Left, points.Right, UP); // call recursive function for first half
            solving(box, g, lists.SouthList, points.Right, points.Left, DOWN); // call recursive function for second half
            

            paintHull(box, g, points.Left, points.Right);
        }

        private void paintHull(PictureBox box, Graphics g, PointF left, PointF right)
        {
            upperPerimeter = upperPerimeter.OrderBy(x => x.Angle).ToList();
            lowerPerimeter = lowerPerimeter.OrderBy(x => x.Angle).ToList();

            for (int a = 0; a < upperPerimeter.Count - 1; a++)
            {
                g.DrawLine(new Pen(Color.Orange, 3), upperPerimeter[a].Point, upperPerimeter[a + 1].Point); 
            }

            for (int a = 0; a < lowerPerimeter.Count - 1; a++)
            {
                g.DrawLine(new Pen(Color.Orange, 3), lowerPerimeter[a].Point, lowerPerimeter[a + 1].Point);
            }

            box.Refresh();
        }


        public void solving(PictureBox box, Graphics g, List<System.Drawing.PointF> pointList, PointF leftPoint, PointF rightPoint, int cuadrant)
        {
            int indexOfInsertion = pointList.IndexOf(rightPoint);
            if (pointList.Count == 0)
            {
                return; // id list id zero size it means that there are no more points above it so it returns
            }
            else if (pointList.Count == 1)
            {
                if (cuadrant == UP) 
                    ConvexHullSolver.upperPerimeter.Add(new OutterPoint(ConvexHullSolver.highestXPoint, pointList[0]));
                if (cuadrant == DOWN) 
                    ConvexHullSolver.lowerPerimeter.Add(new OutterPoint(ConvexHullSolver.lowestXPoint, pointList[0]));
                return; // if list is size 1, it means that the point is an edge. Therefore it is saved into the perimeter list
            }
            else
            {
                PointF furthestPoint = findFurthestPoint(leftPoint, rightPoint, pointList); // by comparing cross vector distances we get relative distance
                                                                                            //that avoid divition

                if (cuadrant == UP) 
                    ConvexHullSolver.upperPerimeter.Add(new OutterPoint(ConvexHullSolver.highestXPoint, furthestPoint)); // the furthest point is added to the 
                                                                                                                         //perimeter list
                if (cuadrant == DOWN)
                    ConvexHullSolver.lowerPerimeter.Add(new OutterPoint(ConvexHullSolver.lowestXPoint, furthestPoint));

                SplitedLists leftLists = new SplitedLists();
                leftLists.setSouthAndNorthLists(pointList, leftPoint, furthestPoint); // from this split we only care about the upper points of the list,
                                                                                      //the lower points are inside the hull

                SplitedLists rightLists = new SplitedLists(); // from this split we only care about the upper points of the list, the lower points are inside 
                                                              //the hull
                rightLists.setSouthAndNorthLists(pointList, furthestPoint, rightPoint);

                solving(box, g, leftLists.NorthList, leftPoint, furthestPoint,cuadrant); // re call function to solve left smaller subproblem
                solving(box, g, rightLists.NorthList, furthestPoint, rightPoint, cuadrant); // re call function to solve right smaller subproblem
            }
        }

        public PointF findFurthestPoint(PointF left, PointF right, List<PointF> pointList)
        {
            double farthestDistance = 0.0;
            PointF farthestPoint = new PointF();

            foreach (PointF point in pointList) // for each point in the list the one with the greates distance is saved
            {
                double pointDistance = distance(left, right, point);
                if (pointDistance > farthestDistance)
                {
                    farthestDistance = pointDistance;
                    farthestPoint = point;
                }
            }
            return farthestPoint;
        }

        public double distance(PointF left, PointF right, PointF point) // distance is calculated by cross product of the tangents created by most left and
                                                                        //most right point compared to the point in cuestion 
        {
            double valueX = right.X - left.X;
            double valueY = right.Y - left.Y;
            double num = valueX * (left.Y - point.Y) - valueY * (left.X - point.X);
            return Math.Abs(num);
        }

    }

    #region temporal classes

    class SplitedLists
    {
        public List<PointF> NorthList { get; set; }
        public List<PointF> SouthList { get; set; }

        public SplitedLists()
        {
            NorthList = new List<PointF>();
            SouthList = new List<PointF>();
        }
        public void setSouthAndNorthLists(List<PointF> pointList, PointF leftX, PointF rightX)
        {
            foreach (PointF point in pointList) // for each point location is calculated and it is sent to northlist or south list
            {
                if (getLocation(leftX, rightX, point) == 1)
                {
                    NorthList.Add(point);
                }
                else
                {
                    SouthList.Add(point);
                }
            }
        }

        private int getLocation(PointF leftX, PointF rightX, PointF point) // location is calculated by seeing at the cross product sign 
        {
            double location = (rightX.X - leftX.X) * (point.Y - leftX.Y) - (rightX.Y - leftX.Y) * (point.X - leftX.X);
            return (location > 0) ? 1 : -1;
        }

    }

    class ExtremeXPoints
    {
        public PointF Left { get; set; }
        public PointF Right { get; set; }
        public ExtremeXPoints()
        {
            Left = new PointF();
            Right = new PointF();
        }

        public void setXExtremePoints(List<PointF> pointList)
        {
            PointF mostLeft = pointList[0];
            PointF mostRight = pointList[0];
            foreach (PointF point in pointList) // for each point the x coordinate is compared and only the extremes are saved
            {
                if (point.X < mostLeft.X)
                {
                    mostLeft = point;
                }
                if (point.X > mostRight.X)
                {
                    mostRight = point;
                }
            }
            Left = mostLeft;
            Right = mostRight;
        }
    }

    class OutterPoint 
    {
        public double Angle { get; set; }
        public PointF Point { get; set; }

        public OutterPoint(PointF left, PointF point)
        {
            Angle = (point.Y - left.Y) / (point.X - left.X); // when ordering perimeter points clock wise, the angle is used to sort them. How ever no trig
                                                             //functions are used since we only care about the ratio
            Point = point;
        }
    }

        
   
     #endregion
}


