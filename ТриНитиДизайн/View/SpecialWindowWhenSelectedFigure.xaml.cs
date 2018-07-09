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
        List<Line> otshitLines;
        Rectangle firstRec;
        Rectangle lastRec;
        Canvas canvas;

        public SpecialWindowWhenSelectedFigure(List<Figure> _listFigure, List<Line> _otshitLines, Rectangle _firstRec,
            Rectangle _lastRec, Canvas _canvas)
        {
            InitializeComponent();
            button_stegki.Focus();
            button_stegki.BorderThickness = new Thickness(1.9);
            listFigure = _listFigure;
            otshitLines = _otshitLines;
            firstRec = _firstRec;
            lastRec = _lastRec;
            canvas = _canvas;
        }

        private void Prorisovat_Stezhki(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimDrawStegki;
            foreach (Figure fig in listFigure)
                fig.ChangeFigureColor(OptionColor.ColorKrivaya, false);
            canvas.Children.Remove(lastRec);
            canvas.Children.Remove(firstRec);
            this.Close();
        }

        private void Prorisovat_v_tsvete(object sender, RoutedEventArgs e)
        {
            //TODO: move draw in color function here in full
            OptionRegim.regim = Regim.RegimDrawInColor;
            this.Close();
        }

        private void Sokhranit_v_fayl(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "dst files (*.dst)|*.dst";
            Nullable<bool> result = saveFile.ShowDialog();
            if (result == true)
            {
                Point centerPoint = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);
                string contents = WriteDST(listFigure, centerPoint);
                StreamWriter writer = new StreamWriter(saveFile.OpenFile(),Encoding.Default);
                writer.Write(contents);
                writer.Dispose();
                writer.Close();
            }
            this.Close();
        }

        private void Otshit(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimOtshit;
            foreach (Figure fig in listFigure)
                fig.ChangeFigureColor(OptionColor.ColorKrivaya, false);
            canvas.Children.Remove(lastRec);
            canvas.Children.Remove(firstRec);
            Line horizontalLine = new Line();
            double x = listFigure[0].PointStart.X;
            double y = listFigure[0].groupFigures[0].PointStart.Y;
            horizontalLine = GeometryHelper.SetLine(OptionColor.ColorChoosingRec, new Point(x - 350, y),
                new Point(x + 350, y), true, canvas);
            Line verticalLine = new Line();
            verticalLine = GeometryHelper.SetLine(OptionColor.ColorChoosingRec, new Point(x, y - 350),
                new Point(x, y + 350), true, canvas);
            otshitLines.Add(verticalLine);
            otshitLines.Add(horizontalLine);
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

        private string WriteDST(List<Figure> list, Point center)
        {
            string contents = "";
            List<Point> pts = new List<Point>(list[0].Points);
            contents += "LA:Untitled        ";
            contents += BuildMetaInfo("ST:", pts.Count, 7);
            contents += BuildMetaInfo("CO:", 0, 3);

            List<Point> ptsRec = GeometryHelper.GetFourOutsidePointsForGroup(list, 0);
            int height = (int)(ptsRec[1].Y - ptsRec[0].Y);
            int width = (int)(ptsRec[2].X - ptsRec[1].X);
            contents += BuildMetaInfo("+X:", width, 5);
            contents += BuildMetaInfo("-X:", width, 5);
            contents += BuildMetaInfo("+Y:", height, 5);
            contents += BuildMetaInfo("-Y:", height, 5);

            contents += BuildMetaInfo("AX:+", 0, 6);
            contents += BuildMetaInfo("AY:+", 0, 6);
            contents += BuildMetaInfo("MX:+", 0, 6);
            contents += BuildMetaInfo("MY:+", 0, 6);
            contents += "PD:******";
            char[] characters = System.Text.Encoding.ASCII.GetChars(new byte[] { 0x1a });
            contents+= characters[0];
            for (int i = 125; i < 512; i++)
            {
                contents+=" ";
            }

            /*
            int xx = 0;
            int yy = 0;
            int flag = 0;
            int dx, dy;
            foreach (Point p in pts)
            {
                dx = (int)(p.X) - xx;
                dy = (int)(p.Y) - yy;
                xx = (int)(p.X);
                yy = (int)(p.Y);
                if (p == list[0].PointEnd)
                    flag = 1;
                contents = EncodePoint(dx, dy, flag, contents);
            }
            contents += (Char)0xA1;
            characters = System.Text.Encoding.ASCII.GetChars(new byte[] { 0xA1 });
            contents += characters[0];
            */
            for (int i = 0; i < 1024; i++)
                contents += (char)0xA1;
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
            return st;
        }

        private string EncodePoint(int x, int y, int flag, string contents)
        {
            char b0, b1, b2;
            b0 = b1 = b2 = '0';

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

            b2 |= (char)3;

            if (flag == 1)
            {
                b2 = (char)243;
                b0 = b1 = (char)0;
            }

            /*
            if (flags & (JUMP | TRIM))
            {
                b2 = (char)(b2 | 0x83);
            }
            if (flags & STOP)
            {
                b2 = (char)(b2 | 0xC3);
            }
            */
            contents += b0;
            contents += b1;
            contents += b2;
            return contents;
        }

        private char SetBit(int pos)
        {
            return (char)(1 << pos);
        }
    }
}
