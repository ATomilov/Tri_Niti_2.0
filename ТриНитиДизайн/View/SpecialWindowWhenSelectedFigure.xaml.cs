using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ТриНитиДизайн.View
{
    /// <summary>
    /// Логика взаимодействия для SpecialWindowWhenSelectedFigure.xaml
    /// </summary>
    public partial class SpecialWindowWhenSelectedFigure : Window
    {
        List<Figure> listFigure;
        List<Line> unembroidLines;
        Rectangle firstRec;
        Rectangle lastRec;
        Canvas canvas;

        public SpecialWindowWhenSelectedFigure(List<Figure> _listFigure, List<Line> _unembroidLines, Rectangle _firstRec,
            Rectangle _lastRec, Canvas _canvas)
        {
            InitializeComponent();
            button_stegki.Focus();
            button_stegki.BorderThickness = new Thickness(1.9);
            listFigure = _listFigure;
            unembroidLines = _unembroidLines;
            firstRec = _firstRec;
            lastRec = _lastRec;
            canvas = _canvas;
        }

        private void Prorisovat_Stezhki(object sender, RoutedEventArgs e)
        {
            OptionMode.mode = Mode.modeDrawStitchesInColor;
            foreach (Figure fig in listFigure)
                fig.ChangeFigureColor(OptionColor.colorCurve, false);
            canvas.Children.Remove(lastRec);
            canvas.Children.Remove(firstRec);
            this.Close();
        }

        private void Prorisovat_v_tsvete(object sender, RoutedEventArgs e)
        {
            //TODO: move draw in color function here in full
            OptionMode.mode = Mode.modeDrawInColor;
            this.Close();
        }

        private void Sokhranit_v_fayl(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "dst files (*.dst)|*.dst";
            Nullable<bool> result = saveFile.ShowDialog();
            if (result == true)
            {
                StreamWriter writer = new StreamWriter(saveFile.OpenFile(), Encoding.Default);
                string contents = WriteDST(listFigure, System.IO.Path.GetFileNameWithoutExtension(saveFile.FileName));
                writer.Write(contents);
                writer.Dispose();
                writer.Close();
            }
            this.Close();
        }

        private void Otshit(object sender, RoutedEventArgs e)
        {
            OptionMode.mode = Mode.modeUnembroid;
            foreach (Figure fig in listFigure)
                fig.ChangeFigureColor(OptionColor.colorCurve, false);
            canvas.Children.Remove(lastRec);
            canvas.Children.Remove(firstRec);
            Line horizontalLine = new Line();
            double x = listFigure[0].PointStart.X;
            double y = listFigure[0].groupFigures[0].PointStart.Y;
            horizontalLine = GeometryHelper.SetLine(OptionColor.colorArc, new Point(x - 350, y),
                new Point(x + 350, y), true, canvas);
            Line verticalLine = new Line();
            verticalLine = GeometryHelper.SetLine(OptionColor.colorArc, new Point(x, y - 350),
                new Point(x, y + 350), true, canvas);
            unembroidLines.Add(verticalLine);
            unembroidLines.Add(horizontalLine);
            this.Close();
        }

        private void Otmenit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_stegki_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            button_stegki.BorderThickness = new Thickness(1);
        }

        private string WriteDST(List<Figure> list, string title)
        {
            string contents = "";            
            contents += "LA:" + title;
            if (contents.Length < 19)
            {
                while (contents.Length < 19)
                    contents += " ";
            }
            else
                contents = contents.Remove(19);
            contents += "\n";

            int stCount = 0;
            foreach (Figure fig in list)
                stCount += fig.Points.Count;
            contents += BuildMetaInfo("ST:", stCount, 7);
            contents += BuildMetaInfo("CO:", list.Count - 1, 3);

            List<Point> ptsRec = GeometryHelper.GetFourOutsidePointsForGroup(list, 0);
            int height = (int)((ptsRec[1].Y - ptsRec[0].Y)*5);
            int width = (int)((ptsRec[2].X - ptsRec[1].X)*5);
            contents += BuildMetaInfo("+X:", width, 5);
            contents += BuildMetaInfo("-X:", width, 5);
            contents += BuildMetaInfo("+Y:", height, 5);
            contents += BuildMetaInfo("-Y:", height, 5);

            contents += BuildMetaInfo("AX:+", 0, 6);
            contents += BuildMetaInfo("AY:+", 0, 6);
            contents += BuildMetaInfo("MX:+", 0, 6);
            contents += BuildMetaInfo("MY:+", 0, 6);
            contents += "PD:******\n";
            contents += EmbChar(0x1a);

            for (int i = 125; i < 512; i++)
            {
                contents+=" ";
            }
                        
            int xx, yy, dx, dy;
            for (int i = 0; i < list.Count; i++)
            {
                contents = EncodePoint(0, 0, 0, contents);
                List<Point> pts = new List<Point>(list[i].Points);
                xx = (int)(pts[0].X * 5);
                yy = (int)(pts[0].Y * 5);
                for (int j = 1; j < pts.Count; j++)
                {
                    dx = (int)(pts[j].X * 5) - xx;
                    dy = yy - (int)(pts[j].Y * 5);
                    xx = (int)(pts[j].X * 5);
                    yy = (int)(pts[j].Y * 5);
                    contents = EncodePoint(dx, dy, 0, contents);
                }
                if (i != list.Count - 1)
                    contents = JumpToNewEmbroideryPart(pts[pts.Count - 1], list[i + 1].PointStart, contents);
            }
            contents = EncodePoint(0, 0, 3, contents);
            contents += EmbChar(0xA1);
            return contents;
        }

        private string BuildMetaInfo(string st, int num, int count)
        {
            st += num;
            int index;
            if (st[0] == 'A' || st[0] == 'M')
                index = 4;
            else
                index = 3;
            while (st.Length < (count+3))
                st = st.Insert(index, " ");
            st += "\n";
            return st;
        }

        private string JumpToNewEmbroideryPart(Point pointStart, Point pointEnd, string contents)
        {
            contents = EncodePoint(0, 0, 1, contents);
            pointStart = new Point((int)(pointStart.X * 5), (int)(pointStart.Y * 5));
            pointEnd = new Point((int)(pointEnd.X * 5), (int)(pointEnd.Y * 5));
            Vector vect = pointEnd - pointStart;
            int divider = 4;
            vect /= divider;
            while (vect.X > 110 || vect.X < -110 || vect.Y > 110 || vect.Y < -110)
            {
                vect *= divider;
                divider++;
                vect /= divider;
            }
            divider--;
            for (int i = 0; i < divider; i++)
                contents = EncodePoint((int)vect.X, -(int)vect.Y, 1, contents);
            Point penulitmatePoint = new Point(pointStart.X + (int)(vect.X * divider), pointStart.Y + (int)(vect.Y * divider));
            contents = EncodePoint((int)(pointEnd.X - penulitmatePoint.X), (int)(penulitmatePoint.Y - pointEnd.Y), 1, contents);
            contents = EncodePoint(0, 0, 2, contents);
            return contents;
        }

        private string EncodePoint(int x, int y, int flag, string contents)
        {
            int b0, b1, b2;
            b0 = b1 = b2 = 0;

            /* cannot encode values > +121 or < -121. */
            if (x > 121 || x < -121)
            {
                contents += "encode error";
                return contents;
            }
            if (y > 121 || y < -121)
            {
                contents += "encode error";
                return contents;
            }

            if (x >= +41) { b2 += SetBit(2); x -= 81; }
            if (x <= -41) { b2 += SetBit(3); x += 81; }
            if (x >= +14) { b1 += SetBit(2); x -= 27; }
            if (x <= -14) { b1 += SetBit(3); x += 27; }
            if (x >= +5) { b0 += SetBit(2); x -= 9; }
            if (x <= -5) { b0 += SetBit(3); x += 9; }
            if (x >= +2) { b1 += SetBit(0); x -= 3; }
            if (x <= -2) { b1 += SetBit(1); x += 3; }
            if (x >= +1) { b0 += SetBit(0); x -= 1; }
            if (x <= -1) { b0 += SetBit(1); x += 1; }
            if (x != 0)
            {
                contents += "encode error";
                return contents;
            }
            if (y >= +41) { b2 += SetBit(5); y -= 81; }
            if (y <= -41) { b2 += SetBit(4); y += 81; }
            if (y >= +14) { b1 += SetBit(5); y -= 27; }
            if (y <= -14) { b1 += SetBit(4); y += 27; }
            if (y >= +5) { b0 += SetBit(5); y -= 9; }
            if (y <= -5) { b0 += SetBit(4); y += 9; }
            if (y >= +2) { b1 += SetBit(7); y -= 3; }
            if (y <= -2) { b1 += SetBit(6); y += 3; }
            if (y >= +1) { b0 += SetBit(7); y -= 1; }
            if (y <= -1) { b0 += SetBit(6); y += 1; }

            if (y != 0)
            {
                contents += "encode error";
                return contents;
            }

            b2 |= 3;

            //jump
            if (flag == 1)
            {
                b2 = (b2 | 0x83);
            }

            //end one part of embroidery
            if (flag == 2)
            {
                b2 = (b2 | 0xC3);
            }

            //end full embroidery
            if (flag == 3)
            {
                b2 = 243;
                b0 = b1 = 0;
            }
            contents += EmbChar(b0);
            contents += EmbChar(b1);
            contents += EmbChar(b2);
            return contents;
        }

        private char EmbChar(int num)
        {
            string newstring = 
                "ЂЃ‚ѓ„…†‡€‰Љ‹ЊЌЋЏђ‘’“”•–—™љ›њќћџ ЎўЈ¤Ґ¦§Ё©Є«¬­®Ї°±Ііґµ¶·ё№є»јЅѕїАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюя";
            char ch;
            if (num > 126)
                ch = newstring[num - 127];
            else
                ch = (char)num;
            return ch;
        }

        private int SetBit(int pos)
        {
            return 1 << pos;
        }
    }
}
