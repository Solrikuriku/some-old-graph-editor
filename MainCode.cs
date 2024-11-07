using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using static cureach.MainCode;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System;
using System.Xml.Linq;
using System.Formats.Asn1;

//пикчурбокс отображает тянущуюся линию
//битмап рисует финальный результат
//у каждого объекта - свой класс с настройками (?)


//в MainCode лучше хранить обработчики событий
namespace cureach
{
    public partial class MainCode : Form
    {
        public MainCode()
        {
            InitializeComponent();
            //красим пикчурбокс
            Canvas.BackColor = Color.White;
            //добавляем событие прокрутки мыши
            Canvas.MouseWheel += new MouseEventHandler(Canvas_MouseWheel);
            ChooseFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ChooseFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ChooseFontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        private Bitmap canvasBitmap = new Bitmap(863, 619);

        private ArcsManipulation _getArc = new ArcsManipulation();
        private TextControl _txt = new TextControl();
        private MinMaxOfArc _minMaxArc = new MinMaxOfArc();
        private MinMaxOfLine _minMaxLine = new MinMaxOfLine();
        private MinMaxOfTextRectangle _minMaxText = new MinMaxOfTextRectangle();
        private MinMaxOfAllPolyline _absolutelyPolylineTBLR = new MinMaxOfAllPolyline();

        private TestForDelete _checkThis = new TestForDelete();

        List<DrawLineItem> linesStorage = new List<DrawLineItem>();
        List<DrawPolylineLineItem> polLineStorage = new List<DrawPolylineLineItem>();
        List<DrawPolylineArcItem> polArcStorage = new List<DrawPolylineArcItem>();
        List<DrawEllipseItem> ellipseStorage = new List<DrawEllipseItem>();
        List<DrawArcItem> arcStorage = new List<DrawArcItem>();
        List<DrawTextItem> textStorage = new List<DrawTextItem>();
        List<PolylineMaxMin> polTBLRStorage = new List<PolylineMaxMin>();

        List<PointF> currentObjects = new List<PointF>();

        private int idPolyline = 0;

        private Bitmap openBit;

        private bool isRCMAllow = false;
        private bool isLoaded = false;
        private float firstAngle;
        private float lastAngle;

        private int textDegree = 0;
        public enum StopPolyline
        {
            Stop,
            Continue
        }
        public enum DirectionAngle
        {
            Plus,
            Minus
        }
        public enum PolylineState
        {
            Line,
            Arc
        }
        public enum TextState
        {
            Start,
            Print,
            Change,
            Save
        }
        PolylineState figure = PolylineState.Line;
        DirectionAngle stateAngle = DirectionAngle.Plus;
        StopPolyline statePolyline = StopPolyline.Continue;
        TextState stateText = TextState.Start;

        private PointF currentPoint;

        //смена координат
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //фиксация текущих точек
            currentPoint.X = e.X;
            currentPoint.Y = e.Y;
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (DrawLine.Checked)
            {
                currentObjects.Add(new Point(e.X, e.Y));
                if (currentObjects.Count == 2)
                {
                    (float top, float bottom, float left, float right) = _minMaxLine.GetMinMax(currentObjects[0], currentObjects[1]);
                    linesStorage.Add(new DrawLineItem(currentObjects[0].X,
                        currentObjects[0].Y, currentObjects[1].X, currentObjects[1].Y,
                        top, bottom, left, right));
                    ReRendering();
                    currentObjects.Clear();
                }
            }

            if (DrawCircle.Checked)
            {
                currentObjects.Add(new Point(e.X, e.Y));
                if (currentObjects.Count == 2)
                {
                    float rad = _checkThis.LineLength(currentObjects[0].X, currentObjects[0].Y,
                        currentObjects[1].X, currentObjects[1].Y);
                    ellipseStorage.Add(new DrawEllipseItem(currentObjects[0].X - rad, currentObjects[0].Y - rad,
                        2 * rad, 2 * rad, rad, currentObjects[0].Y - rad, currentObjects[0].Y + rad,
                        currentObjects[0].X - rad, currentObjects[0].X + rad));
                    ReRendering();
                    currentObjects.Clear();
                }
            }

            if (DrawArc.Checked)
            {
                currentObjects.Add(new Point(e.X, e.Y));

                if (currentObjects.Count == 3)
                {
                    _minMaxArc.StandardValues(currentObjects[1], _getArc.GetRC());
                    (float top, float bottom, float left, float right) = _minMaxArc.GetMinMax(firstAngle * 180.0f / (float)Math.PI, lastAngle * 180.0f / (float)Math.PI,
                        (int)_getArc.StartCoordinates().Width / 2, currentObjects[0]);
                    arcStorage.Add(new DrawArcItem(
                        _getArc.StartCoordinates().X, _getArc.StartCoordinates().Y,
                        _getArc.StartCoordinates().Width, _getArc.StartCoordinates().Height,
                        firstAngle * 180.0f / (float)Math.PI, lastAngle * 180.0f / (float)Math.PI,
                        currentObjects[0].X, currentObjects[0].Y, currentObjects[1].X, currentObjects[1].Y,
                        _getArc.GetRC().X, _getArc.GetRC().Y,
                        top, bottom, left, right,
                        stateAngle));
                    ReRendering();
                    currentObjects.Clear();
                }
            }

            if (DrawPolyline.Checked)
            {
                statePolyline = StopPolyline.Continue;
                if (e.Button == MouseButtons.Left)
                {
                    isRCMAllow = true;
                    currentObjects.Add(new Point(e.X, e.Y));
                    if (figure == PolylineState.Line && currentObjects.Count > 1)
                    {
                        (float top, float bottom, float left, float right) = _minMaxLine.GetMinMax(currentObjects[currentObjects.Count - 2], currentObjects[currentObjects.Count - 1]);
                        polLineStorage.Add(new DrawPolylineLineItem(idPolyline,
                            currentObjects[currentObjects.Count - 2].X, currentObjects[currentObjects.Count - 2].Y,
                            currentObjects[currentObjects.Count - 1].X, currentObjects[currentObjects.Count - 1].Y,
                            top, bottom, left, right
                            ));

                        ReRendering();
                    }
                    else if (figure == PolylineState.Arc && currentObjects.Count > 1)
                    {
                        currentObjects[currentObjects.Count - 1] =
                        _getArc.GetRC();

                        _minMaxArc.StandardValues(currentObjects[currentObjects.Count - 3], _getArc.GetRC());
                        (float top, float bottom, float left, float right) = _minMaxArc.GetMinMax(firstAngle * 180.0f / (float)Math.PI, lastAngle * 180.0f / (float)Math.PI,
                            (int)_getArc.StartCoordinates().Width / 2, currentObjects[currentObjects.Count - 2]);

                        //тест другого хранилища
                        polArcStorage.Add(new DrawPolylineArcItem(idPolyline,
                            _getArc.StartCoordinates().X, _getArc.StartCoordinates().Y,
                            _getArc.StartCoordinates().Width, _getArc.StartCoordinates().Height,
                            firstAngle * 180.0f / (float)Math.PI,
                            lastAngle * 180.0f / (float)Math.PI,
                            currentObjects[currentObjects.Count - 3].X, currentObjects[currentObjects.Count - 3].Y,
                            currentObjects[currentObjects.Count - 2].X, currentObjects[currentObjects.Count - 2].Y,
                            currentObjects[currentObjects.Count - 1].X, currentObjects[currentObjects.Count - 1].Y,
                            top, bottom, left, right,
                            stateAngle
                            ));

                        ReRendering();
                    }
                    figure = PolylineState.Line;
                }
                //господи какой отвратительный код
                else if (e.Button == MouseButtons.Right && isRCMAllow == true)
                {
                    currentObjects.Add(new Point(e.X, e.Y));
                    figure = PolylineState.Arc;
                    isRCMAllow = false;
                }
            }

            if (AddText.Checked)
            {
                _txt.GetParametersFont(ChooseFont.GetItemText(ChooseFont.SelectedItem));
                _txt.GetParametersSize(Convert.ToSingle(ChooseFontSize.SelectedItem));
                _txt.GetParametersStyle(ChooseFontStyle.GetItemText(ChooseFontStyle.SelectedItem));
            }

            if (RLText.Checked && stateText == TextState.Change)
            {
                //textDegree = 0;
                _txt.MoveText(new Point(e.X, e.Y));
            }
            //тестовое удаление (вроде работает)
            if (Delete.Checked)
            {
                DeleteByClick(new PointF(e.X, e.Y));
                ReRendering();
            }
            if (DeleteNotFullRect.Checked)
            {
                currentObjects.Add(new Point(e.X, e.Y));

                if (currentObjects.Count == 2)
                {
                    //отправляем прямоугольничек 
                    DeleteByRectangleNotFull(new RectangleF(currentObjects[0].X, currentObjects[0].Y,
                        currentObjects[1].X - currentObjects[0].X, currentObjects[1].Y - currentObjects[0].Y));
                    ReRendering();
                    DeleteByRectangleFull(new RectangleF(currentObjects[0].X, currentObjects[0].Y,
                        currentObjects[1].X - currentObjects[0].X, currentObjects[1].Y - currentObjects[0].Y));
                    currentObjects.Clear();
                    ReRendering();

                }
            }
            if (DeleteFullRect.Checked)
            {
                currentObjects.Add(new Point(e.X, e.Y));

                if (currentObjects.Count == 2)
                {
                    //отправляем прямоугольничек 
                    DeleteByRectangleFull(new RectangleF(currentObjects[0].X, currentObjects[0].Y,
                        currentObjects[1].X - currentObjects[0].X, currentObjects[1].Y - currentObjects[0].Y));
                    currentObjects.Clear();
                    ReRendering();
                }
            }
        }
        //отрисовка без сохранения в битмап
        //ПРОВЕРКУ НА ОТРИЦАТЕЛЬНОСТЬ КООРДИНАТ В ОТДЕЛЬНУЮ ФУНКЦИЮ
        //ПРИДУМАТЬ ЧТО-ТО С УГЛОМ
        //после отрисовки всех фигур почистить этот ужас

        //для расчета радиусов и прочей фигни - свой отдельный класс/метод!
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (DrawLine.Checked && currentObjects.Count != 0)
                e.Graphics.DrawLine(new Pen(Color.Black), currentObjects[0].X,
                currentObjects[0].Y, currentPoint.X, currentPoint.Y);

            if (DrawCircle.Checked && currentObjects.Count != 0)
            {
                float rad = _checkThis.LineLength(currentObjects[0].X, currentObjects[0].Y,
                    currentPoint.X, currentPoint.Y);
                e.Graphics.DrawLine(new Pen(Color.Gray), currentObjects[0].X,
                currentObjects[0].Y, currentPoint.X, currentPoint.Y);
                e.Graphics.DrawEllipse(new Pen(Color.Gray), currentObjects[0].X - rad, currentObjects[0].Y - rad,
                2 * rad, 2 * rad);
            }

            if (DrawArc.Checked && currentObjects.Count == 1)
            {
                e.Graphics.DrawLine(new Pen(Color.Black), currentObjects[0].X,
                currentObjects[0].Y, currentPoint.X, currentPoint.Y);
            }
            if (DrawArc.Checked && currentObjects.Count == 2)
            {
                _getArc.CalculateAll(currentObjects[0], currentObjects[1],
                    currentPoint);
                firstAngle = _getArc.StartAngle();
                lastAngle = _getArc.SweepAngle();
                if (stateAngle == DirectionAngle.Minus)
                    lastAngle = -(2 * (float)Math.PI - lastAngle);
                e.Graphics.DrawLine(new Pen(Color.Gray), currentObjects[0], currentObjects[1]);
                e.Graphics.DrawLine(new Pen(Color.Black), currentObjects[0], _getArc.GetRC());
                e.Graphics.DrawArc(new Pen(Color.Black), _getArc.StartCoordinates(),
                    firstAngle * 180.0f / (float)Math.PI, lastAngle * 180.0f / (float)Math.PI);
            }
            //первый клик - радиус
            //второй клик - угол
            if (DrawPolyline.Checked && currentObjects.Count > 0 && statePolyline == StopPolyline.Continue)
            {
                if (figure == PolylineState.Line)
                {
                    e.Graphics.DrawLine(new Pen(Color.Black), currentObjects[currentObjects.Count - 1], currentPoint);
                }
                else if (figure == PolylineState.Arc)
                {
                    _getArc.CalculateAll(currentObjects[currentObjects.Count - 1],
                        currentObjects[currentObjects.Count - 2], currentPoint);
                    firstAngle = _getArc.StartAngle();
                    lastAngle = _getArc.SweepAngle();
                    if (stateAngle == DirectionAngle.Minus)
                        lastAngle = -(2 * (float)Math.PI - lastAngle);
                    e.Graphics.DrawLine(new Pen(Color.Gray),
                        currentObjects[currentObjects.Count - 1],
                        currentObjects[currentObjects.Count - 2]
                        );
                    e.Graphics.DrawLine(new Pen(Color.Black),
                        currentObjects[currentObjects.Count - 1],
                        _getArc.GetRC());
                    e.Graphics.DrawArc(new Pen(Color.Black), _getArc.StartCoordinates(),
                        firstAngle * 180.0f / (float)Math.PI, lastAngle * 180.0f / (float)Math.PI);
                }
            }
            if (AddText.Checked)
            {
                _txt.ChangeTextStyle();
            }
            if (RLText.Checked && stateText == TextState.Change)
            {
                e.Graphics.TranslateTransform(_txt.ReturnTextCoordinates().Width / 2 + _txt.ReturnTextCoordinates().X,
                    _txt.ReturnTextCoordinates().Height / 2 + _txt.ReturnTextCoordinates().Y);
                e.Graphics.RotateTransform(textDegree);
                e.Graphics.TranslateTransform(-(_txt.ReturnTextCoordinates().Width / 2 + _txt.ReturnTextCoordinates().X),
                    -(_txt.ReturnTextCoordinates().Height / 2 + _txt.ReturnTextCoordinates().Y));
                e.Graphics.DrawString(_txt.ReturnText(), _txt.ReturnFont(), new SolidBrush(Color.Black),
                    _txt.ReturnTextCoordinates());
            }
            if (DeleteNotFullRect.Checked && currentObjects.Count == 1)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Gray), currentObjects[0].X, currentObjects[0].Y,
                    currentPoint.X - currentObjects[0].X, currentPoint.Y - currentObjects[0].Y);
            }
            if (DeleteFullRect.Checked && currentObjects.Count == 1)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Red), currentObjects[0].X, currentObjects[0].Y,
                    currentPoint.X - currentObjects[0].X, currentPoint.Y - currentObjects[0].Y);
            }
            Refresh();
        }
        void Canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            //получилось не очень, согласна
            if (DrawArc.Checked || DrawPolyline.Checked)
            {
                if (e.Delta > 0)
                    stateAngle = DirectionAngle.Plus;
                else if (e.Delta < 0)
                    stateAngle = DirectionAngle.Minus;
            }
            if (RLText.Checked)
            {
                if (e.Delta > 0)
                    textDegree += 15;
                else if (e.Delta < 0)
                    textDegree -= 15;
            }
        }

        private void Canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (DrawPolyline.Checked)
            {
                if (statePolyline == StopPolyline.Continue)
                    statePolyline = StopPolyline.Stop;
                isRCMAllow = false;
                statePolyline = StopPolyline.Continue;
                figure = PolylineState.Line;

                var aL = polLineStorage.Where(r => r.ID == idPolyline).ToList();
                var aA = polArcStorage.Where(r => r.ID == idPolyline).ToList();

                _absolutelyPolylineTBLR.MakeGroupOfCoords(idPolyline, aA, aL);
                (float top, float bottom, float left, float right) = _absolutelyPolylineTBLR.GetMinMax();
                polTBLRStorage.Add(new PolylineMaxMin(idPolyline, top, bottom, left, right));
                _absolutelyPolylineTBLR.ClearList();
                idPolyline++;
                currentObjects.Clear();
            }
            if (AddText.Checked && stateText == TextState.Start)
            {
                _txt.AddControlOnCanvas(e.X, e.Y, Canvas);
                stateText = TextState.Print;
            }
            //сохраняем текст, переводим в режим пеинт через графикс
            else if (AddText.Checked && stateText == TextState.Print)
            {
                stateText = TextState.Change;
                _txt.DeleteTextBox(Canvas);
                _txt.ChangeTBWidth();
                AddText.Checked = false;
                RLText.Checked = true;
            }
            //сохраняет в битмап после вращалок и перетасовок
            else if (RLText.Checked && stateText == TextState.Change)
            {
                var points = new PointF[] {
                                                new PointF { X = _txt.ReturnTextCoordinates().X, Y = _txt.ReturnTextCoordinates().Y }, //0
                                                new PointF { X = (_txt.ReturnTextCoordinates().X + _txt.ReturnTextCoordinates().Width), Y = _txt.ReturnTextCoordinates().Y }, //1
                                                new PointF { X = _txt.ReturnTextCoordinates().X, Y = (_txt.ReturnTextCoordinates().Y + _txt.ReturnTextCoordinates().Height) }, //2
                                                new PointF { X = (_txt.ReturnTextCoordinates().X + _txt.ReturnTextCoordinates().Width), Y = (_txt.ReturnTextCoordinates().Y + _txt.ReturnTextCoordinates().Height) }, //3
                                              };

                float angle = (textDegree * (float)Math.PI) / 180;
                PointF centerPoint = new PointF((_txt.ReturnTextCoordinates().X + _txt.ReturnTextCoordinates().Width / 2), (_txt.ReturnTextCoordinates().Y + _txt.ReturnTextCoordinates().Height / 2));

                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = _checkThis.CalculateNewPoint(points[i], centerPoint, angle);
                }

                (float top, float bottom, float left, float right) = _minMaxText.GetMinMax(points[0], points[1], points[2], points[3]);

                textStorage.Add(new DrawTextItem(_txt.ReturnText(), _txt.ReturnFontFamily(),
                    _txt.ReturnSize(), _txt.ReturnFontStyle(), _txt.ReturnTextCoordinates().X,
                    _txt.ReturnTextCoordinates().Y, _txt.ReturnTextCoordinates().Width,
                    _txt.ReturnTextCoordinates().Height,
                    textDegree, top, bottom, left, right));

                ReRendering();

                textDegree = 0;
                stateText = TextState.Start;

                _txt.ClearData();
                AddText.Checked = true;
                RLText.Checked = false;
            }
        }
        private void MainCode_Load(object sender, EventArgs e)
        {
            RLText.Enabled = false;
            //устанавливаем в комбобокс шрифты
            InstalledFontCollection fontCollection = new InstalledFontCollection();
            foreach (var f in fontCollection.Families)
            {
                ChooseFont.Items.Add(f.Name);
            }
            //первый шрифт
            //Segoe UI - стандартный
            if (ChooseFont.SelectedIndex == -1)
                ChooseFont.SelectedIndex = ChooseFont.FindString("Segoe UI");
            //размер шрифта, выбираем стандартный
            if (ChooseFontSize.SelectedIndex == -1)
                ChooseFontSize.SelectedItem = "9";
            //тип шрифта берем из enum FontStyle
            foreach (string styleName in Enum.GetNames(typeof(FontStyle)))
            {
                ChooseFontStyle.Items.Add(styleName);
            }
            if (ChooseFontStyle.SelectedIndex == -1)
                ChooseFontStyle.SelectedIndex = ChooseFontStyle.FindString("Regular");

            _txt.GetParametersFont(ChooseFont.GetItemText(ChooseFont.SelectedItem));
            _txt.GetParametersSize(Convert.ToSingle(ChooseFontSize.SelectedItem));
            _txt.GetParametersStyle(ChooseFontStyle.GetItemText(ChooseFontStyle.SelectedItem));
        }
        //класс для дальнейшей пересылки параметров шрифтов
        private void ChooseFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            _txt.GetParametersFont(ChooseFont.GetItemText(ChooseFont.SelectedItem));
        }
        private void ChooseFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            _txt.GetParametersSize(Convert.ToSingle(ChooseFontSize.SelectedItem));
        }

        private void ChooseFontStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            _txt.GetParametersStyle(ChooseFontStyle.GetItemText(ChooseFontStyle.SelectedItem));
        }

        private void ClearCanvas_Click(object sender, EventArgs e)
        {
            Graphics.FromImage(canvasBitmap).Clear(Color.White);
            Canvas.Image = canvasBitmap;

            currentObjects.Clear();

            linesStorage.Clear();
            ellipseStorage.Clear();
            arcStorage.Clear();
            polArcStorage.Clear();
            polLineStorage.Clear();
            polTBLRStorage.Clear();
            textStorage.Clear();

            figure = PolylineState.Line;
            stateAngle = DirectionAngle.Plus;
            statePolyline = StopPolyline.Continue;
            stateText = TextState.Start;

            idPolyline = 0;
            textDegree = 0;
            isRCMAllow = false;
            isLoaded = false;
        }

        private void SaveInFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "jpg";
            saveFileDialog1.Filter = "JPG images (*.jpg)|*.jpg";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var fileName = saveFileDialog1.FileName;
                canvasBitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
        //будет время: провести рефакторинг
        public void DeleteByClick(PointF p)
        {
            bool isFind = false;
            //сначала поиск линии
            foreach (var l in linesStorage.ToList())
            {
                if (_checkThis.BorderTest(l.X1, l.Y1, l.X2, l.Y2, p.X, p.Y))
                {
                    float AB = _checkThis.LineLength(l.X1, l.Y1, l.X2, l.Y2);
                    float AC = _checkThis.LineLength(l.X1, l.Y1, p.X, p.Y);
                    float CB = _checkThis.LineLength(p.X, p.Y, l.X2, l.Y2);

                    if ((AC + CB) >= (AB - 0.5) && (AC + CB) <= (AB + 0.5))
                    {
                        linesStorage.Remove(l);
                        isFind = true;
                    }
                }
                if (isFind)
                    break;
            }
            foreach (var l in ellipseStorage.ToList())
            {
                if (EllipseCollision(p, l))
                {
                    ellipseStorage.Remove(l);
                    isFind = true;
                }
                if (isFind)
                    break;
            }
            foreach (var l in arcStorage.ToList())
            {
                if (ArcCollision(p, new RectangleF(l.AX, l.AY, l.Width, l.Height), l.LastAngle,
                    new PointF(l.X1, l.Y1), new PointF(l.X2, l.Y2), l.Direction))
                {
                    arcStorage.Remove(l);
                    isFind = true;
                }
                if (isFind)
                    break;
            }
            foreach (var l in polLineStorage.ToList())
            {
                //удаление все с найденным индексом
                if (_checkThis.BorderTest(l.X1, l.Y1, l.X2, l.Y2, p.X, p.Y) && !isFind)
                {
                    float AB = _checkThis.LineLength(l.X1, l.Y1, l.X2, l.Y2);
                    float AC = _checkThis.LineLength(l.X1, l.Y1, p.X, p.Y);
                    float CB = _checkThis.LineLength(p.X, p.Y, l.X2, l.Y2);

                    if ((AC + CB) >= (AB - 0.5) && (AC + CB) <= (AB + 0.5))
                    {
                        polLineStorage.RemoveAll(r => r.ID == l.ID);
                        polArcStorage.RemoveAll(r => r.ID == l.ID);
                        polTBLRStorage.RemoveAll(r => r.ID == l.ID);
                        isFind = true;
                    }
                }
                if (isFind)
                    break;
            }
            foreach (var l in polArcStorage.ToList())
            {
                // Ну нЕлЬзЯ жЕ тАк пИсАтЬ кОд
                if (ArcCollision(p, new RectangleF(l.AX, l.AY, l.Width, l.Height), l.LastAngle, new PointF(l.X2, l.Y2),
                    new PointF(l.X1, l.Y1), l.Direction))
                {
                    polLineStorage.RemoveAll(r => r.ID == l.ID);
                    polArcStorage.RemoveAll(r => r.ID == l.ID);
                    polTBLRStorage.RemoveAll(r => r.ID == l.ID);
                    isFind = true;
                }
                if (isFind)
                    break;
            }
            foreach (var l in textStorage.ToList())
            {
                if (_checkThis.TextTest(p, new RectangleF(l.TextX, l.TextY, l.Width, l.Height), l.Degree) && !isFind)
                {
                    textStorage.Remove(l);
                    isFind = true;
                }
                if (isFind)
                    break;
            }
        }
        //отправка координат прямоугольника
        //и пересекает, и находится внутри
        public void DeleteByRectangleNotFull(RectangleF delRect)
        {
            List<PointF> listOfRectPoints = _checkThis.TestDots(delRect);
            //сначала поиск линии
            foreach (var l in linesStorage.ToList())
            {
                if (_checkThis.MatrixCollision(new PointF(l.X1, l.Y1), new PointF(l.X2, l.Y2), delRect))
                    linesStorage.Remove(l);
            }
            foreach (var l in ellipseStorage.ToList())
            {
                foreach (var p in listOfRectPoints)
                {
                    if (EllipseCollision(p, l))
                        ellipseStorage.Remove(l);
                }
            }
            foreach (var l in arcStorage.ToList())
            {
                //список линий для пересечений?
                foreach (var p in listOfRectPoints)
                {
                    if (ArcCollision(p, new RectangleF(l.AX, l.AY, l.Width, l.Height), l.LastAngle,
                        new PointF(l.X1, l.Y1), new PointF(l.X2, l.Y2), l.Direction))
                        arcStorage.Remove(l);
                }
            }
            foreach (var l in polLineStorage.ToList())
            {
                if (_checkThis.MatrixCollision(new PointF(l.X1, l.Y1), new PointF(l.X2, l.Y2), delRect))
                {
                    polLineStorage.RemoveAll(r => r.ID == l.ID);
                    polArcStorage.RemoveAll(r => r.ID == l.ID);
                    polTBLRStorage.RemoveAll(r => r.ID == l.ID);
                }
            }
            foreach (var l in polArcStorage.ToList())
            {
                foreach (var p in listOfRectPoints)
                {
                    if (ArcCollision(p, new RectangleF(l.AX, l.AY, l.Width, l.Height), l.LastAngle,
                        new PointF(l.X2, l.Y2), new PointF(l.X1, l.Y1), l.Direction))
                    {
                        polLineStorage.RemoveAll(r => r.ID == l.ID);
                        polArcStorage.RemoveAll(r => r.ID == l.ID);
                        polTBLRStorage.RemoveAll(r => r.ID == l.ID);
                    }
                }
            }
            foreach (var l in textStorage.ToList())
            {
                var coordRectangle = new PointF[] {
                                                new PointF { X = l.TextX, Y = l.TextY }, //0
                                                new PointF { X = (l.TextX + l.Width), Y = l.TextY }, //1
                                                new PointF { X = l.TextX, Y = (l.TextY + l.Height) }, //2
                                                new PointF { X = (l.TextX + l.Width), Y = (l.TextY + l.Height) }, //3
                                                new PointF { X = l.TextX, Y = l.TextY } //4
                                              };
                coordRectangle = _checkThis.CoordinatesAfterRotating(l.Degree, coordRectangle, new RectangleF(l.TextX, l.TextY, l.Width, l.Height));

                for (int i = 0; i < coordRectangle.Length - 1; i++)
                {
                    if (_checkThis.MatrixCollision(coordRectangle[i], coordRectangle[i + 1], delRect))
                        textStorage.Remove(l);
                }
            }
        }

        public void DeleteByRectangleFull(RectangleF delRect)
        {
            //сначала поиск линии
            float delRectTop = delRect.Y;
            float delRectBottom = delRect.Y + delRect.Height;
            float delRectLeft = delRect.X;
            float delRectRight = delRect.X + delRect.Width;
            foreach (var l in linesStorage.ToList())
            {
                if (l.Top >= delRect.Top && l.Bottom <= delRectBottom && l.Left >= delRectLeft && l.Right <= delRectRight)
                    linesStorage.Remove(l);
            }
            foreach (var l in ellipseStorage.ToList())
            {
                if (l.Top >= delRect.Top && l.Bottom <= delRectBottom && l.Left >= delRectLeft && l.Right <= delRectRight)
                    ellipseStorage.Remove(l);
            }
            foreach (var l in arcStorage.ToList())
            {
                if (l.Top >= delRect.Top && l.Bottom <= delRectBottom && l.Left >= delRectLeft && l.Right <= delRectRight)
                    arcStorage.Remove(l);
            }
            foreach (var l in polTBLRStorage.ToList())
            {
                if (l.Top >= delRect.Top && l.Bottom <= delRectBottom && l.Left >= delRectLeft && l.Right <= delRectRight)
                {
                    polLineStorage.RemoveAll(r => r.ID == l.ID);
                    polArcStorage.RemoveAll(r => r.ID == l.ID);
                    polTBLRStorage.RemoveAll(r => r.ID == l.ID);
                }
            }
            foreach (var l in textStorage.ToList())
            {
                if (l.Top >= delRect.Top && l.Bottom <= delRectBottom && l.Left >= delRectLeft && l.Right <= delRectRight)
                    textStorage.Remove(l);
            }
        }
        //ререндеринг
        public void ReRendering()
        {
            if (!isLoaded)
                Graphics.FromImage(canvasBitmap).Clear(Color.White);
            else
                Graphics.FromImage(canvasBitmap).DrawImage(openBit, 0, 0, 863, 619);
            Canvas.Image = canvasBitmap;
            foreach (var item in linesStorage)
            {
                Graphics.FromImage(canvasBitmap).DrawLine(new Pen(Color.Black), new PointF(item.X1, item.Y1),
                    new PointF(item.X2, item.Y2));
                Canvas.Image = canvasBitmap;
            }
            foreach (var item in ellipseStorage)
            {
                Graphics.FromImage(canvasBitmap).DrawEllipse(new Pen(Color.Black),
                    new RectangleF(item.EX, item.EY, item.Width, item.Height));
                Canvas.Image = canvasBitmap;
            }
            foreach (var item in arcStorage)
            {
                Graphics.FromImage(canvasBitmap).DrawArc(new Pen(Color.Black),
                    new RectangleF(item.AX, item.AY, item.Width, item.Height), item.FirstAngle, item.LastAngle);
                Canvas.Image = canvasBitmap;
            }
            foreach (var item in polLineStorage)
            {
                Graphics.FromImage(canvasBitmap).DrawLine(new Pen(Color.Black), new PointF(item.X1, item.Y1),
                    new PointF(item.X2, item.Y2));
                Canvas.Image = canvasBitmap;
            }
            foreach (var item in polArcStorage)
            {
                Graphics.FromImage(canvasBitmap).DrawArc(new Pen(Color.Black), new RectangleF(item.AX, item.AY, item.Width, item.Height),
                    item.FirstAngle, item.LastAngle);
                Canvas.Image = canvasBitmap;
            }
            foreach (var item in textStorage)
            {
                Graphics ReDraw = Graphics.FromImage(canvasBitmap);
                //включаем разрешение на перемещения и повороты
                ReDraw.InterpolationMode = InterpolationMode.High;
                ReDraw.TranslateTransform(item.Width / 2 + item.TextX,
                    item.Height / 2 + item.TextY);
                ReDraw.RotateTransform(item.Degree);
                ReDraw.TranslateTransform(-(item.Width / 2 + item.TextX),
                    -(item.Height / 2 + item.TextY));
                ReDraw.DrawString(item.Text, new Font(new FontFamily(item.TextFontFamily), item.TextSize,
                    (FontStyle)Enum.Parse(typeof(FontStyle), item.TextFontStyle)), new SolidBrush(Color.Black),
                    new RectangleF(item.TextX, item.TextY, item.Width, item.Height));
                Canvas.Image = canvasBitmap;
            }
        }

        public bool ArcCollision(PointF tP, RectangleF arcRect, float arcAngle, PointF aSP, PointF aMP,
                                DirectionAngle aD)
        {
            float testPoint = (float)Math.Sqrt((tP.X - (arcRect.X + arcRect.Width / 2)) *
                    (tP.X - (arcRect.X + arcRect.Width / 2))
                    + (tP.Y - (arcRect.Y + arcRect.Width / 2)) * (tP.Y - (arcRect.Y + arcRect.Width / 2)));
            _getArc.CalculateAll(aSP, aMP, tP);
            float lA = _getArc.SweepAngle();
            if (aD == DirectionAngle.Minus)
                lA = -(2 * (float)Math.PI - lA);
            lA = lA * 180.0f / (float)Math.PI;
            bool isDir;
            if (arcAngle < 0)
                isDir = (lA >= arcAngle);
            else
                isDir = (lA <= arcAngle);

            return (testPoint >= (arcRect.Width / 2 - 3) && testPoint <= (arcRect.Width / 2 + 3) &&
                    isDir);
        }

        public bool EllipseCollision(PointF tP, DrawEllipseItem ellipse)
        {
            float testPoint = (float)Math.Sqrt((tP.X - (ellipse.EX + ellipse.Radius)) * (tP.X - (ellipse.EX + ellipse.Radius))
                    + (tP.Y - (ellipse.EY + ellipse.Radius)) * (tP.Y - (ellipse.EY + ellipse.Radius)));
            return (testPoint >= (ellipse.Radius - 3)) && (testPoint <= (ellipse.Radius + 3));
        }

        private void OpenFromFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "jpg";
            openFileDialog1.Filter = "JPG images (*.jpg)|*.jpg";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openBit = new Bitmap(openFileDialog1.FileName);
                Canvas.Image = openBit;
                isLoaded = true;

                //currentObjects.Clear();
                //linesStorage.Clear();
                //ellipseStorage.Clear();
                //arcStorage.Clear();
                //polArcStorage.Clear();
                //polLineStorage.Clear();
                //polTBLRStorage.Clear();
                //textStorage.Clear();

                //figure = PolylineState.Line;
                //stateAngle = DirectionAngle.Plus;
                //statePolyline = StopPolyline.Continue;
                //stateText = TextState.Start;

                //idPolyline = 0;
                //textDegree = 0;
                //isRCMAllow = false;
                ReRendering();
            }
        }

        private void SaveList_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Text file (.txt)|*.txt";

            var counts = new List<string>()
            {
                linesStorage.Count.ToString(),
                ellipseStorage.Count.ToString(),
                arcStorage.Count.ToString(),
                polLineStorage.Count.ToString(),
                polArcStorage.Count.ToString(),
                polTBLRStorage.Count.ToString(),
                textStorage.Count.ToString()
            };

            var countString = string.Join(',', counts);
            countString = AddAll(countString);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var fileName = saveFileDialog1.FileName;
                File.WriteAllText(fileName, countString);
            }
        }

        public string AddAll(string mainLine)
        {
            foreach (var l in linesStorage)
            {
                mainLine += "\n";
                var line = l.X1 + "↑" + l.Y1 + "↑" + l.X2 + "↑" + l.Y2 + "↑"
                    + l.Top.ToString() + "↑" + l.Bottom.ToString() + "↑" + l.Left.ToString() +
                    "↑" + l.Right.ToString();
                mainLine += line;
            }
            foreach (var l in ellipseStorage)
            {
                mainLine += "\n";
                var line = l.EX.ToString() + "↑" + l.EY.ToString() + "↑" +
                    l.Width.ToString() + "↑" + l.Height.ToString() + "↑" + l.Radius.ToString() + "↑"
                    + l.Top.ToString() + "↑" + l.Bottom.ToString() + "↑" + l.Left.ToString() +
                    "↑" + l.Right.ToString();
                mainLine += line;
            }
            foreach (var l in arcStorage)
            {
                mainLine += "\n";
                var line = l.AX.ToString() + "↑" + l.AY.ToString() + "↑" + l.Width + "↑" + l.Height +
                    "↑" + l.FirstAngle.ToString() + "↑" + l.LastAngle.ToString()
                    + "↑" + l.X1 + "↑" + l.Y1 + "↑" + l.X2 + "↑" + l.Y2 + "↑" + l.X3 + "↑" + l.Y3 + "↑"
                    + l.Top.ToString() + "↑" + l.Bottom.ToString() + "↑" + l.Left.ToString() +
                    "↑" + l.Right.ToString() + "↑" + l.Direction.ToString();
                mainLine += line;
            }
            foreach (var l in polLineStorage)
            {
                mainLine += "\n";
                var line = l.ID.ToString() + "↑" + l.X1 + "↑" + l.Y1 + "↑" + l.X2 + "↑" + l.Y2 + "↑"
                    + l.Top.ToString() + "↑" + l.Bottom.ToString() + "↑" + l.Left.ToString() +
                    "↑" + l.Right.ToString();
                mainLine += line;
            }
            foreach (var l in polArcStorage)
            {
                mainLine += "\n";
                var line = l.ID.ToString() + "↑" + l.AX.ToString() + "↑" + l.AY.ToString() + "↑" + l.Width + "↑" + l.Height +
                    "↑" + l.FirstAngle.ToString() + "↑" + l.LastAngle.ToString()
                    + "↑" + l.X1 + "↑" + l.Y1 + "↑" + l.X2 + "↑" + l.Y2 + "↑" + l.X3 + "↑" + l.Y3 + "↑"
                    + l.Top.ToString() + "↑" + l.Bottom.ToString() + "↑" + l.Left.ToString() +
                    "↑" + l.Right.ToString() + "↑" + l.Direction.ToString();
                mainLine += line;
            }
            foreach (var l in polTBLRStorage)
            {
                mainLine += "\n";
                var line = l.ID.ToString() + "↑" + l.Top.ToString() + "↑" + l.Bottom.ToString() + "↑"
                    + l.Left.ToString() + "↑" + l.Right.ToString();
                mainLine += line;
            }
            foreach (var l in textStorage)
            {
                mainLine += "\n";
                var line = l.Text.ToString() + "↑" + l.TextFontFamily.ToString() + "↑" + l.TextSize.ToString()
                    + "↑" + l.TextFontStyle.ToString() + "↑"
                    + l.TextX.ToString() + "↑" + l.TextY.ToString() + "↑"
                    + l.Width.ToString() + "↑" + l.Height.ToString() + "↑" + l.Degree.ToString() + "↑" +
                    l.Top.ToString() + "↑" + l.Bottom.ToString() + "↑"
                    + l.Left.ToString() + "↑" + l.Right.ToString();
                mainLine += line;
            }
            return mainLine;
        }

        public (int, int, int, int, int, int, int) ReturnCounts(string path)
        {
            var result = path.Split(',');

            int lCNT = int.Parse(result[0]);
            int eCNT = int.Parse(result[1]);
            int aCNT = int.Parse(result[2]);
            int plCNT = int.Parse(result[3]);
            int paCNT = int.Parse(result[4]);
            int mmpCNT = int.Parse(result[5]);
            int tCNT = int.Parse(result[6]);

            return (lCNT, eCNT, aCNT, plCNT, paCNT, mmpCNT, tCNT);
        }

        private void OpenList_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "TXT file(*.txt) | *.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int lines = 0; int ellipses = 0; int arcs = 0; int polLines = 0; int polArcs = 0;
                int polMM = 0; int texts = 0;

                (lines, ellipses, arcs, polLines,
                    polArcs, polMM, texts) = ReturnCounts(File.ReadLines(openFileDialog1.FileName).First());

                for (int i = 1; i <= lines; i++)
                {
                    var readline = File.ReadLines(openFileDialog1.FileName).ElementAtOrDefault(i);
                    var result = readline.Split('↑');
                    linesStorage.Add(new DrawLineItem(float.Parse(result[0]), float.Parse(result[1]),
                        float.Parse(result[2]), float.Parse(result[3]), float.Parse(result[4]),
                        float.Parse(result[5]), float.Parse(result[6]), float.Parse(result[7])));
                }
                for (int i = lines + 1; i <= ellipses + lines; i++)
                {
                    var readline = File.ReadLines(openFileDialog1.FileName).ElementAtOrDefault(i);
                    var result = readline.Split('↑');
                    ellipseStorage.Add(new DrawEllipseItem(float.Parse(result[0]), float.Parse(result[1]),
                        float.Parse(result[2]), float.Parse(result[3]), float.Parse(result[4]),
                        float.Parse(result[5]), float.Parse(result[6]), float.Parse(result[7]), float.Parse(result[8])));
                }
                for (int i = ellipses + lines + 1; i <= ellipses + lines + arcs; i++)
                {
                    var readline = File.ReadLines(openFileDialog1.FileName).ElementAtOrDefault(i);
                    var result = readline.Split('↑');
                    arcStorage.Add(new DrawArcItem(float.Parse(result[0]), float.Parse(result[1]),
                        float.Parse(result[2]), float.Parse(result[3]), float.Parse(result[4]),
                        float.Parse(result[5]), float.Parse(result[6]), float.Parse(result[7]),
                        float.Parse(result[8]), float.Parse(result[9]), float.Parse(result[10]),
                        float.Parse(result[11]), float.Parse(result[12]), float.Parse(result[13]),
                        float.Parse(result[14]), float.Parse(result[15]),
                        (DirectionAngle)Enum.Parse(typeof(DirectionAngle), result[16])));
                }
                for (int i = ellipses + lines + arcs + 1; i <= ellipses + lines + arcs + polLines; i++)
                {
                    var readline = File.ReadLines(openFileDialog1.FileName).ElementAtOrDefault(i);
                    var result = readline.Split('↑');
                    polLineStorage.Add(new DrawPolylineLineItem(idPolyline, float.Parse(result[1]),
                        float.Parse(result[2]), float.Parse(result[3]), float.Parse(result[4]), float.Parse(result[5]),
                        float.Parse(result[6]), float.Parse(result[7]), float.Parse(result[8])));
                }
                for (int i = ellipses + lines + arcs + polLines + 1; i <= ellipses + lines + arcs + polLines + polArcs; i++)
                {
                    var readline = File.ReadLines(openFileDialog1.FileName).ElementAtOrDefault(i);
                    var result = readline.Split('↑');
                    polArcStorage.Add(new DrawPolylineArcItem(idPolyline, float.Parse(result[1]), float.Parse(result[2]),
                        float.Parse(result[3]), float.Parse(result[4]), float.Parse(result[5]),
                        float.Parse(result[6]), float.Parse(result[7]), float.Parse(result[8]),
                        float.Parse(result[9]), float.Parse(result[10]), float.Parse(result[11]),
                        float.Parse(result[12]), float.Parse(result[13]), float.Parse(result[14]),
                        float.Parse(result[15]), float.Parse(result[16]),
                        (DirectionAngle)Enum.Parse(typeof(DirectionAngle), result[17])));
                }
                for (int i = ellipses + lines + arcs + polLines + polArcs + 1; i <= ellipses + lines + arcs + polLines + polArcs + polMM; i++)
                {
                    var readline = File.ReadLines(openFileDialog1.FileName).ElementAtOrDefault(i);
                    var result = readline.Split('↑');
                    polTBLRStorage.Add(new PolylineMaxMin(idPolyline, float.Parse(result[1]), float.Parse(result[2]),
                        float.Parse(result[3]), float.Parse(result[4])));
                    idPolyline++;
                }
                for (int i = ellipses + lines + arcs + polLines + polArcs + polMM + 1; i <= ellipses + lines + arcs + polLines + polArcs + polMM + texts; i++)
                {
                    var readline = File.ReadLines(openFileDialog1.FileName).ElementAtOrDefault(i);
                    var result = readline.Split('↑');
                    textStorage.Add(new DrawTextItem(result[0], result[1], float.Parse(result[2]), result[3], float.Parse(result[4]), float.Parse(result[5]),
                        float.Parse(result[6]), float.Parse(result[7]), int.Parse(result[8]),
                        float.Parse(result[9]), float.Parse(result[10]), float.Parse(result[11]), float.Parse(result[12])));
                }
                ReRendering();
            }
        }
    }
}