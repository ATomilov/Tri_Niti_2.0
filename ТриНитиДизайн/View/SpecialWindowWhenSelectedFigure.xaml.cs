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
                string contents = "123";



                StreamWriter writer = new StreamWriter(saveFile.OpenFile());
                writer.WriteLine(contents);
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

        
    }
}
