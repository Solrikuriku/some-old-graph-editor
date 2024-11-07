using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace cureach
{
    public class ArcsManipulation
    {
        float ox, oy, lx, ly, cx, cy;
        float rCX, rCY;
        float angleBetweenRadiuses, lineBetweenRadiuses;
        float startAngle, sweepAngle;
        float rad;

        public void ClearAll()
        {

        }
        //ёк макарек
        public void CalculateAll(PointF o, PointF r, PointF c)
        {
            ox = o.X; oy = o.Y; lx = r.X; ly = r.Y; cx = c.X; cy = c.Y;
            rad = (float)Math.Sqrt((lx - ox) * (lx - ox) + (ly - oy) * (ly - oy));
            angleBetweenRadiuses = (float)(Math.Asin(Math.Abs(o.Y - c.Y) /
                Math.Sqrt((o.X - c.X) * (o.X - c.X) + (o.Y - c.Y) * (o.Y - c.Y))));
            angleBetweenRadiuses = AngleDirection(cx, cy, angleBetweenRadiuses);
            rCX = (float)Math.Cos(angleBetweenRadiuses) * rad + ox;
            rCY = (float)Math.Sin(angleBetweenRadiuses) * rad + oy;
            lineBetweenRadiuses = (float)Math.Sqrt((lx - rCX) * (lx - rCX) + (ly - rCY) * (ly - rCY));
        }
        public PointF GetRC()
        {
            return new PointF(rCX, rCY);
        }
        public RectangleF StartCoordinates()
        {
            return new RectangleF(ox - rad, oy - rad, 2 * rad, 2 * rad);
        }
        public float StartAngle()
        {
            startAngle = (float)(Math.Acos(Math.Abs(lx - ox) / rad));
            startAngle = AngleDirection(lx, ly, startAngle);
            return startAngle;
        }
        public float SweepAngle()
        {
            sweepAngle = (float)(2 * Math.Asin(lineBetweenRadiuses / (2 * rad)));
            sweepAngle = ControlCoordinates(rCX, rCY, sweepAngle);
            //if (stateAngle == DirectionAngle.Minus)
            //    sweepAngle = -(2 * (float)Math.PI - sweepAngle);
            return sweepAngle;
        }
        public float AngleDirection(float sX, float sY, float angle)
        {
            if (sY >= oy && sX < ox)
                angle = (float)Math.PI - angle;
            else if (sY < oy && sX <= ox)
                angle = angle - (float)Math.PI;
            else if (sY <= oy && sX > ox)
                angle = 0.0f - angle;
            return angle;
        }
        public float ControlCoordinates(float sX, float sY, float angle)
        {
            PointF controlCoordinates;
            if ((lx > ox && ly > oy) || (lx == ox && ly > oy) || (lx > ox && ly == oy)) 
            {
                controlCoordinates = new PointF(lx - 2 * Math.Abs(lx - ox), ly - 2 * Math.Abs(ly - oy));
                if (sX > controlCoordinates.X && sX <= (rad + ox) && sY < ly && sY >= (oy - rad))
                    angle = 2 * (float)Math.PI - angle;
            }
            else if ((lx < ox && ly > oy) || (lx == ox && ly > oy) || (lx < ox && ly == oy))
            {
                controlCoordinates = new PointF(lx + 2 * Math.Abs(lx - ox), ly - 2 * Math.Abs(ly - oy));
                if (sX > lx && sX <= (rad + ox) && sY > controlCoordinates.Y && sY <= (oy + rad))
                    angle = 2 * (float)Math.PI - angle;
            }
            else if ((lx < ox && ly < oy) || (lx == ox && ly < oy) || (lx < ox && ly == oy))
            {
                controlCoordinates = new PointF(lx + 2 * Math.Abs(lx - ox), ly + 2 * Math.Abs(lx - oy));
                if (sX >= (ox - rad) && sX < controlCoordinates.X && sY <= (oy + rad) && sY > ly)
                    angle = 2 * (float)Math.PI - angle;
            }
            else if ((lx > ox && ly < oy) || (lx == ox && ly < oy) || (lx < ox && ly == oy))
            {
                controlCoordinates = new PointF(lx - 2 * Math.Abs(lx - ox), ly + 2 * Math.Abs(ly - oy));
                if (sX >= (ox - rad) && sX < lx && sY >= (oy - rad) && sY < controlCoordinates.Y)
                    angle = 2 * (float)Math.PI - angle;
            }
            return angle;
        }
    }

    public class MinMaxOfArc
    {
        float maxY;
        float minY;
        float maxX;
        float minX;

        public void StandardValues(PointF p1, PointF p2)
        {
            if (p1.Y <= p2.Y)
            {
                maxY = p1.Y;
                minY = p2.Y;
            }
            else
            {
                maxY = p2.Y;
                minY = p1.Y;
            }

            if (p1.X <= p2.X)
            {
                maxX = p2.X;
                minX = p1.X;
            }
            else
            {
                maxX = p1.X;
                minX = p2.X;
            }
        }
        public (float, float, float, float) GetMinMax(float firAng, float laAng, int rad, PointF oP)
        {
            laAng = firAng + laAng;
            if (firAng > laAng)
                (firAng, laAng) = (laAng, firAng);

            for (int i = (int)firAng; i <= (int)laAng; i++)
            {
                int y = rad * (int)Math.Sin(i * Math.PI / 180.0f) + (int)oP.Y;
                int x = rad * (int)Math.Cos(i * Math.PI / 180.0f) + (int)oP.X;
                if ((int)Math.Sin(i * Math.PI / 180.0f) == -1)
                {
                    maxY = y;
                }
                else if ((int)Math.Sin(i * Math.PI / 180.0f) == 1)
                {
                    minY = y;
                }
                if ((int)Math.Cos(i * Math.PI / 180.0f) == -1)
                {
                    minX = x;
                }
                else if ((int)Math.Cos(i * Math.PI / 180.0f) == 1)
                {
                    maxX = x;
                }
            }
            return (maxY, minY, minX, maxX);
        }
    }
    public class MinMaxOfLine
    {
        float maxY;
        float minY;
        float maxX;
        float minX;
        public (float, float, float, float) GetMinMax(PointF p1, PointF p2)
        {
            if (p1.Y <= p2.Y)
            {
                maxY = p1.Y;
                minY = p2.Y;
            }
            else
            {
                maxY = p2.Y;
                minY = p1.Y;
            }

            if (p1.X <= p2.X)
            {
                maxX = p2.X;
                minX = p1.X;
            }
            else
            {
                maxX = p1.X;
                minX = p2.X;
            }

            return (maxY, minY, minX, maxX);
        }
    }
    public class MinMaxOfAllPolyline
    {
        float maxY;
        float minY;
        float maxX;
        float minX;

        List<float> allTopBottom = new List<float>();
        List<float> allLeftRight = new List<float>();

        public void MakeGroupOfCoords(int id, List<DrawPolylineArcItem> arcList, List<DrawPolylineLineItem> lineList)
        {
            foreach (var l in arcList)
            {
                allTopBottom.Add(l.Top);
                allTopBottom.Add(l.Bottom);
                allLeftRight.Add(l.Left);
                allLeftRight.Add(l.Right);
            }

            foreach (var l in lineList)
            {
                allTopBottom.Add(l.Top);
                allTopBottom.Add(l.Bottom);
                allLeftRight.Add(l.Left);
                allLeftRight.Add(l.Right);
            }
        }

        public (float, float, float, float) GetMinMax()
        {
            minY = allTopBottom.Max();
            maxY = allTopBottom.Min();
            minX = allLeftRight.Min();
            maxX = allLeftRight.Max();

            return (maxY, minY, minX, maxX);
        }

        public void ClearList()
        {
            allTopBottom.Clear();
            allLeftRight.Clear();
        }
    }
    public class MinMaxOfTextRectangle
    {
        float maxY;
        float minY;
        float maxX;
        float minX;
        public (float, float, float, float) GetMinMax(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float[] yPoints = new float[] { p1.Y, p2.Y, p3.Y, p4.Y };
            float[] xPoints = new float[] { p1.X, p2.X, p3.X, p4.X };

            maxY = yPoints.Min();
            minY = yPoints.Max();
            minX = xPoints.Min();
            maxX = xPoints.Max();

            return (maxY, minY, minX, maxX);
        }
    }

    //этот класс занимается тестированием перед удалением
    public class TestForDelete
    {
        public bool BorderTest(float xa, float ya, float xb, float yb, float cx, float cy)
        {
            if (xa >= xb)
                (xa, xb) = (xb, xa);
            if (ya >= yb)
                (ya, yb) = (yb, ya);

            if ((xa - 5 <= cx) && (xb + 5 >= cx) && (ya - 5 <= cy) && (yb + 5 >= cy))
                return true;
            else
                return false;
        }
        public float LineLength(float x1, float y1, float x2, float y2)
        {
            float d = (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            return d;
        }
        public bool MatrixCollision(PointF start, PointF end, RectangleF testRect)
        {
            //основная линия
            var coordRectangle = new PointF[] {
                                                new PointF { X = testRect.X, Y = testRect.Y }, //0
                                                new PointF { X = (testRect.X + testRect.Width), Y = testRect.Y }, //1
                                                new PointF { X = (testRect.X + testRect.Width), Y = (testRect.Y + testRect.Height) }, //2
                                                new PointF { X = testRect.X, Y = (testRect.Y + testRect.Height) },//3
                                                new PointF { X = testRect.X, Y = testRect.Y } //4 (для связки 3-0)
                                              };
            bool isCollided = false;
            //метод Крамера?
            for (int i = 0; i < coordRectangle.Length - 1; i++)
            {
                //основной определитель матрицы
                float dMain = (end.X - start.X) * (coordRectangle[i].Y - coordRectangle[i + 1].Y) 
                    - (coordRectangle[i].X - coordRectangle[i + 1].X) * (end.Y - start.Y);
                if (dMain == 0)
                    break;
                float s = ((coordRectangle[i].X - start.X) * (coordRectangle[i].Y - coordRectangle[i + 1].Y) 
                    - (coordRectangle[i].X - coordRectangle[i + 1].X) * (coordRectangle[i].Y - start.Y))
                    / dMain;
                float t = ((end.X - start.X) * (coordRectangle[i].Y - start.Y)
                    - (coordRectangle[i].X - start.X) * (end.Y - start.Y))
                    / dMain;
                if ((s >= 0 && t >= 0) && (s <= 1 && t <= 1))
                {
                    isCollided = true;
                    break;
                }
            }
            return isCollided;
        }
        public bool TextTest(PointF p, RectangleF rect, int deg)
        {
            var coordRectangle = new PointF[] { 
                                                new PointF { X = rect.X, Y = rect.Y }, //0
                                                new PointF { X = (rect.X + rect.Width), Y = rect.Y }, //1
                                                new PointF { X = rect.X, Y = (rect.Y + rect.Height) }, //2
                                                new PointF { X = (rect.X + rect.Width), Y = (rect.Y + rect.Height) } //3
                                              };

            float angle = (deg * (float)Math.PI) / 180;
            //точка, от которой идет поворот
            PointF centerPoint = new PointF((rect.X + rect.Width / 2), (rect.Y + rect.Height / 2));
            //считаем координаты после поворота
            for (int i = 0; i < coordRectangle.Length; i++)
            {
                coordRectangle[i] = CalculateNewPoint(coordRectangle[i], centerPoint, angle);
            }

            float sT1 = GetTriangleSquare(p, coordRectangle[0], coordRectangle[1]);
            float sT2 = GetTriangleSquare(p, coordRectangle[0], coordRectangle[2]);
            float sT3 = GetTriangleSquare(p, coordRectangle[1], coordRectangle[3]);
            float sT4 = GetTriangleSquare(p, coordRectangle[2], coordRectangle[3]);

            float squareRect = (rect.Width * rect.Height);

            return ((sT1 + sT2 + sT3 + sT4) <= squareRect + 5) && ((sT1 + sT2 + sT3 + sT4) >= squareRect - 5);
        }
        public PointF[] CoordinatesAfterRotating(float deg, PointF[] cR, RectangleF rect)
        {
            float angle = (deg * (float)Math.PI) / 180;
            PointF centerPoint = new PointF((rect.X + rect.Width / 2), (rect.Y + rect.Height / 2));

            for (int i = 0; i < cR.Length - 1; i++)
            {
                cR[i] = CalculateNewPoint(cR[i], centerPoint, angle);
            }
            return cR;
        }
        public float GetTriangleSquare(PointF cP, PointF c1, PointF c2)
        {
            float OA = LineLength(cP.X, cP.Y, c1.X, c1.Y);
            float OB = LineLength(cP.X, cP.Y, c2.X, c2.Y);
            float AB = LineLength(c1.X, c1.Y, c2.X, c2.Y);

            float p = (OA + OB + AB) / 2;

            return (float)Math.Sqrt(p*(p-OA)*(p-OB)*(p-AB));
        }
        public PointF CalculateNewPoint(PointF cR, PointF cP, double ang)
        {  
                return new PointF
                    (
                    (cR.X - cP.X) * (float)Math.Cos(ang) - (cR.Y - cP.Y) * (float)Math.Sin(ang) + cP.X,
                    (cR.Y - cP.Y) * (float)Math.Cos(ang) + (cR.X - cP.X) * (float)Math.Sin(ang) + cP.Y
                    );
        }
        public List<PointF> TestDots(RectangleF d)
        {
            List<PointF> _listOfDots = new List<PointF>();
            for (int i = (int)d.X + 1; i < (int)(d.X + d.Width); i++)
            {
                _listOfDots.Add(new PointF(i, d.Y));
                _listOfDots.Add(new PointF(i, d.Y + d.Height));
            }

            for (int i = (int)d.Y + 1; i < (int)(d.Y + d.Height); i++)
            {
                _listOfDots.Add(new PointF(d.X, i));
                _listOfDots.Add(new PointF(d.X + d.Width, i));
            }

            return _listOfDots;
        }
        public List<PointF> AddPointInLines(PointF one, PointF two)
        {
            List<PointF> _listOfDots = new List<PointF>();
            if (one.Y == two.Y)
            {
                for (int i = (int)one.X; i <= (int)two.X; i++)
                {
                    _listOfDots.Add(new PointF(i, one.Y));
                }
            }
            else if (one.X == two.X)
            {
                for (int i = (int)one.Y; i <= (int)two.Y; i++)
                {
                    _listOfDots.Add(new PointF(one.X, i));
                }
            }
            return _listOfDots;
        }
    }
}
