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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using Path = System.Windows.Shapes.Path;
using System.Text.RegularExpressions;

namespace ТриНитиДизайн
{
    public partial class MainWindow
    {
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog op = new SaveFileDialog();
            op.Filter = "Проект Три Нити Дизайн| *.tri";
            op.ShowDialog();
        }


        private void NewProject(object sender, RoutedEventArgs e)
        {
            ListFigure.Clear();
            ListFigure.Add(new Figure(MainCanvas));
            MainCanvas.Children.Clear();
            OptionRegim.regim = Regim.RegimDraw;
            OptionRegim.oldRegim = Regim.RegimFigure;
            TatamiFigures.Clear();
            IndexFigure = 0;
            SecondGladFigure = -1;
            CloseAllTabs();
            SetToDefault();
            MainCanvas.Cursor = NormalCursor;
        }

        private void LoadPLT(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Corel PLT| *.plt";
            Nullable<bool> result = op.ShowDialog();
            if (result == true)
            {
                foreach (Figure fig in ListPltFigure)
                {
                    fig.RemoveFigure(MainCanvas);
                }
                ListPltFigure.Clear();
                StreamReader reader = new StreamReader(op.OpenFile());
                string text = reader.ReadToEnd();
                text = text.Replace("\r\n", "");
                text = text.Replace(";", "");
                text = text.Replace("PD", " ");
                string pattern = @"PU([0-9]| |-)+";
                Regex rgx = new Regex(pattern);
                Vector vect;
                MatchCollection matches = rgx.Matches(text);
                List <List<Point>> pts = new List<List<Point>>();
                double minX = Double.MaxValue;
                double minY = -999999;
                for (int i = 0; i < matches.Count;i++ )
                {
                    string newStuff = matches[i].Value;
                    newStuff = newStuff.Remove(0, 2);
                    pattern = @" ";
                    String[] elements = Regex.Split(newStuff, pattern);
                    double del = 22;
                    pts.Add(new List<Point>());
                    for (int j = 0; j < elements.Length; j += 2)
                    {
                        Point newP = new Point(Double.Parse(elements[j]) / del, Double.Parse(elements[j + 1]) / del);
                        if(newP.X < minX)
                        {
                            minX = newP.X;
                        }
                        if(newP.Y > minY)
                        {
                            minY = newP.Y;
                        }
                        pts[i].Add(newP);
                    }
                }
                vect = new Vector(minX - 4400, minY - 4100);
                for(int i = 0; i< pts.Count;i++)
                {
                    ListPltFigure.Add(new Figure(MainCanvas));
                    for(int j = 0; j < pts[i].Count;j++)
                    {
                        Point newP = new Point((pts[i][j].X - vect.X), (-pts[i][j].Y - vect.Y));
                        ListPltFigure[ListPltFigure.Count - 1].AddPoint(newP, OptionColor.ColorPltFigure, false, 8);
                    }
                }
            }

        }

        private void DeletePLT(object sender, RoutedEventArgs e)
        {
            foreach(Figure fig in ListPltFigure)
            {
                fig.RemoveFigure(MainCanvas);
            }
            ListPltFigure.Clear();
            
            ListPltFigure.Add(new Figure(MainCanvas));
        }

        private void DeleteFigureClick(object sender, RoutedEventArgs e)
        {
            ListFigure[IndexFigure].ClearFigure();
            RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Проект Три Нити Дизайн| *.tri";
            op.ShowDialog();
        }
        private void OpenWindowsColor(object sender, RoutedEventArgs e)
        {
            WindowColors window = new WindowColors();
            window.ShowDialog();
        }


        private void OpenSetting(object sender, RoutedEventArgs e)
        {
            Setting set = new Setting();
            set.ShowInTaskbar = false;
            set.ShowDialog();
        }


        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            About ab = new About();
            ab.ShowInTaskbar = false;
            ab.ShowDialog();
        }
    }
}