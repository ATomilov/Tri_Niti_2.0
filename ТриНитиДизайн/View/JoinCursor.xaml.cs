using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для JoinCursor.xaml
    /// </summary>
    public partial class JoinCursor : Window
    {
        List<Figure> currentList;
        Figure firstFigure;
        Figure secondFigure;
        Canvas canvas;

        public JoinCursor(List<Figure> _currentList, Figure _firstFigure, Figure _secondFigure, Canvas _canvas)
        {
            InitializeComponent();
            currentList = _currentList;
            firstFigure = _firstFigure;
            secondFigure = _secondFigure;
            canvas = _canvas;
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void chain_button_Click(object sender, RoutedEventArgs e)
        {
            Point start = firstFigure.groupFigures[firstFigure.groupFigures.Count - 1].PointEnd;
            Point end = secondFigure.groupFigures[0].PointStart;
            Vector newFigVect = end - start;
            newFigVect /= 5;
            Figure newFigure = new Figure(canvas);
            Point p = start;
            for (int i = 0; i < 6; i++)
            {
                newFigure.AddPoint(p, OptionColor.ColorDraw, false,false, OptionDrawLine.SizeWidthAndHeightRectangle);
                p += newFigVect;
            }
            currentList.Add(newFigure);

            List<Figure> group1 = new List<Figure>(firstFigure.groupFigures);
            List<Figure> group2 = new List<Figure>(secondFigure.groupFigures);

            foreach (Figure fig in group1)
            {
                fig.groupFigures.Add(newFigure);
                foreach (Figure fig2 in group2)
                    fig.groupFigures.Add(fig2);
            }

            foreach (Figure fig in group2)
            {
                newFigure.groupFigures.Add(fig);
                fig.groupFigures.Insert(0, newFigure);
                for (int i = group1.Count - 1; i > -1; i--)
                    fig.groupFigures.Insert(0, group1[i]);
            }

            for (int i = group1.Count - 1; i > -1; i--)
                newFigure.groupFigures.Insert(0, group1[i]);

            this.Close();
        }

        private void transposition_button_Click(object sender, RoutedEventArgs e)
        {
            List<Figure> group1 = new List<Figure>(firstFigure.groupFigures);
            List<Figure> group2 = new List<Figure>(secondFigure.groupFigures);

            foreach (Figure fig in group1)
                foreach (Figure fig2 in group2)
                    fig.groupFigures.Add(fig2);

            foreach (Figure fig in group2)
                for (int i = group1.Count - 1; i > -1; i--)
                    fig.groupFigures.Insert(0, group1[i]);
            this.Close();
        }

        private void shift_dots_button_Click(object sender, RoutedEventArgs e)
        {
            Figure lastFigureInGroup = firstFigure.groupFigures[firstFigure.groupFigures.Count - 1];
            Figure firstFigureInGroup = secondFigure.groupFigures[0];
            Point start = lastFigureInGroup.PointEnd;
            Point end = firstFigureInGroup.PointStart;
            Point middle = new Point();
            middle.X = (start.X + end.X) / 2;
            middle.Y = (start.Y + end.Y) / 2;
            Shape sh;
            if (lastFigureInGroup.Points.Count == 1)
            {
                lastFigureInGroup.DictionaryPointLines.TryGetValue(lastFigureInGroup.Points[0], out sh);
                lastFigureInGroup.DeleteShape(sh, lastFigureInGroup.Points[0], canvas);
                lastFigureInGroup.Points.Remove(start);
                lastFigureInGroup.AddPoint(middle, OptionColor.ColorDraw, false, true, 0);
            }
            else
            {
                lastFigureInGroup.DictionaryPointLines.TryGetValue(lastFigureInGroup.Points[lastFigureInGroup.Points.Count - 2], out sh);
                lastFigureInGroup.DeleteShape(sh, lastFigureInGroup.Points[lastFigureInGroup.Points.Count - 2], canvas);
                lastFigureInGroup.Points.Remove(start);
                lastFigureInGroup.PointEnd = lastFigureInGroup.Points[lastFigureInGroup.Points.Count - 1];
                lastFigureInGroup.AddPoint(middle, OptionColor.ColorDraw, false, false, 0);
            }
            if (firstFigureInGroup.Points.Count == 1)
            {
                firstFigureInGroup.DictionaryPointLines.TryGetValue(firstFigureInGroup.Points[0], out sh);
                firstFigureInGroup.DeleteShape(sh, firstFigureInGroup.Points[0], canvas);
                firstFigureInGroup.Points.Remove(end);
                firstFigureInGroup.AddPoint(middle, OptionColor.ColorDraw, false, true, 0);
            }
            else
            {
                firstFigureInGroup.DictionaryPointLines.TryGetValue(end, out sh);
                firstFigureInGroup.DeleteShape(sh, end, canvas);
                firstFigureInGroup.Points.Remove(end);
                sh = GeometryHelper.SetLine(OptionColor.ColorDraw, middle, firstFigureInGroup.Points[0], false, canvas);
                firstFigureInGroup.AddShape(sh, middle, null);
                firstFigureInGroup.PointStart = middle;
                firstFigureInGroup.Points.Insert(0, middle);
            }
            List<Figure> group1 = new List<Figure>(firstFigure.groupFigures);
            List<Figure> group2 = new List<Figure>(secondFigure.groupFigures);

            foreach (Figure fig in group1)
            {
                foreach (Figure fig2 in group2)
                    fig.groupFigures.Add(fig2);
            }

            foreach (Figure fig in group2)
            {
                for (int i = group1.Count - 1; i > -1; i--)
                    fig.groupFigures.Insert(0, group1[i]);
            }
            this.Close();
        }

        private void shift_elements_button_Click(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimCursorJoinShiftElements;
            this.Close();
        }
    }
}
