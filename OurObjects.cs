using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static cureach.MainCode;

namespace cureach
{
    [Serializable()]
    public class DrawLineItem
    {
        //public Pen Pen { get; set; }
        //public PointF StartPoint { get; set; }
        //public PointF EndPoint { get; set; }
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }
        public DrawLineItem(float x1, float y1, float x2, float y2, float t, float btm, float lft, float rght)
        {
            //Pen = pen;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Top = t;
            Bottom = btm;
            Left = lft;
            Right = rght;
        }
    }
    public class DrawEllipseItem
    {
        //public Pen Pen { get; set; }
        //public RectangleF Rect { get; set; }
        public float EX { get; set; }
        public float EY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Radius { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }
        public DrawEllipseItem(float x, float y, float wid, float hei, float rad, float t, float btm, float lft, float rght)
        {
            //Pen = pen;
            //Rect = rect;
            EX = x;
            EY = y;
            Width = wid;
            Height = hei;
            Radius = rad;
            Top = t;
            Bottom = btm;
            Left = lft;
            Right = rght;
        }
    }
    public class DrawArcItem
    {
        public float AX { get; set; }
        public float AY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float FirstAngle { get; set; }
        public float LastAngle { get; set; }
        public DirectionAngle Direction { get; set; }
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float X3 { get; set; }
        public float Y3 { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }
        public DrawArcItem(float ax, float ay, float wid, float hei, float fA, float lA, float x1, float y1, float x2, float y2,
            float x3, float y3, float t, float b, float l, float r, 
            DirectionAngle dir)
        {
            //Pen = pen;
            //Rect = rect;
            AX = ax;
            AY = ay;
            Width = wid;
            Height = hei;
            FirstAngle = fA;
            LastAngle = lA;
            Direction = dir;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            X3 = x3;
            Y3 = y3;
            //StartPoint = sP;
            //MidPoint = mP;
            //EndPoint = eP;
            Top = t;
            Bottom = b;
            Left = l;
            Right = r;
        }
    }
    public class DrawPolylineLineItem
    {
        public int ID { get; set; }
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }
        public DrawPolylineLineItem(int id, float x1, float y1, float x2, float y2, float t, float btm, float lft, float rght)
        {
            //Pen = pen;
            ID = id;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Top = t;
            Bottom = btm;
            Left = lft;
            Right = rght;
        }
    }
    public class DrawPolylineArcItem
    {
        public int ID { get; set; }
        public float AX { get; set; }
        public float AY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float FirstAngle { get; set; }
        public float LastAngle { get; set; }
        public DirectionAngle Direction { get; set; }
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float X3 { get; set; }
        public float Y3 { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }
        public DrawPolylineArcItem(int id, float ax, float ay, float wid, float hei, float fA, float lA, float x1, float y1, float x2, float y2,
            float x3, float y3, float t, float b, float l, float r,
            DirectionAngle dir)
        {
            ID = id;
            AX = ax;
            AY = ay;
            Width = wid;
            Height = hei;
            FirstAngle = fA;
            LastAngle = lA;
            Direction = dir;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            X3 = x3;
            Y3 = y3;
            Top = t;
            Bottom = b;
            Left = l;
            Right = r;
        }
    }
    public class PolylineMaxMin
    {
        public int ID { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }

        public PolylineMaxMin(int id, float t, float btm, float lft, float rght)
        {
            ID = id;
            Top = t;
            Bottom = btm;
            Left = lft;
            Right = rght;
        }
    }
    [Serializable]
    public class DrawTextItem
    {
        public string Text { get; set; }
        public string TextFontFamily { get; set; }
        public float TextSize { get; set; }
        public string TextFontStyle { get; set; }
        public float TextX { get; set; }
        public float TextY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int Degree { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }
        public DrawTextItem(string text, string ff, float size, string fs, 
            float tx, float ty, float w, float h, int deg, float t, float btm, float lft, float rght) 
        {
            Text = text;
            TextFontFamily = ff;
            TextSize = size;
            TextFontStyle = fs;
            TextX = tx;
            TextY = ty;
            Width = w;
            Height = h;
            Degree = deg;
            Top = t;
            Bottom = btm;
            Left = lft;
            Right = rght;
        }
    }

    //настраивает параметры текста на этапе его печати
    public class TextControl
    {
        TextBox ourTextHere = new TextBox();
        string ourFont, ourStyle;
        float ourSize;
        RectangleF ourRect;
        public void GetParametersFont(string oF)
        {
            ourFont = oF;
        }
        public void GetParametersSize(float oS)
        {
            ourSize = oS;
        }
        public void GetParametersStyle(string oS)
        {
            ourStyle = oS;
        }
        public void AddControlOnCanvas(int px, int py, Control canv)
        {
            ourTextHere.Location = new Point(px, py);
            ourTextHere.AutoSize = true;
            ourTextHere.TextChanged += new EventHandler(ourTextHere_TextChanged);
            canv.Controls.Add(ourTextHere);
            ourRect = new RectangleF(ourTextHere.Location.X, ourTextHere.Location.Y, ourTextHere.Width, ourTextHere.Height);
        }
        private void ourTextHere_TextChanged(object sender, EventArgs e)
        {
            ReRendering();
        }
        public void ChangeTextStyle()
        {
            FontFamily newFont = new FontFamily(ourFont);
            ourTextHere.Font = new Font(newFont, ourSize, FontStyleSettings());
            ReRendering();
        }
        public void ReRendering()
        {
            Size size = TextRenderer.MeasureText(ourTextHere.Text, ourTextHere.Font);
            ourTextHere.Width = 10 + size.Width;
            ourTextHere.Height = size.Height;
            ourRect = new RectangleF(ourTextHere.Location.X, ourTextHere.Location.Y, ourTextHere.Width, ourTextHere.Height);
        }
        public string ReturnText()
        {
            return ourTextHere.Text;
        }
        //только для paint-события
        public Font ReturnFont()
        {
            return ourTextHere.Font;
        }
        public string ReturnFontFamily()
        {
            return ourFont;
        }
        public float ReturnSize()
        {
            return ourSize;
        }
        public string ReturnFontStyle()
        {
            return FontStyleSettings().ToString();
        }
        public void ChangeTBWidth()
        {
            ourTextHere.Width -= 10;
        }
        public RectangleF ReturnTextCoordinates()
        {
            return ourRect;
        }
        public void MoveText(PointF p)
        {
            ourRect = new RectangleF(p.X - ourTextHere.Width/2, p.Y - ourTextHere.Height/2, ourTextHere.Width, ourTextHere.Height);
        }
        public FontStyle FontStyleSettings()
        {
            switch (ourStyle)
            {
                case "Bold":
                    return FontStyle.Bold;
                case "Italic":
                    return FontStyle.Italic;
                case "Strikeout":
                    return FontStyle.Strikeout;
                case "Underline":
                    return FontStyle.Underline;
                default:
                    return FontStyle.Regular;
            }
        }
        public void DeleteTextBox(Control canv)
        {
            canv.Controls.Remove(ourTextHere);
        }
        public void ClearData()
        {
            ourTextHere.Clear();
        }
    }
}
