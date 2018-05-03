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
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Markup;
using System.Xml;

namespace ТриНитиДизайн
{
    public partial class MainWindow
    {
        private void NewProject(object sender, RoutedEventArgs e)
        {
            ClearEverything(false);
            pathToFile = null;
            this.Title = "Три Нити Дизайн 1.0 ";
        }

        private void SaveProjectAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "prj files (*.prj)|*.prj";
            Nullable<bool> result = saveFile.ShowDialog();
            if (result == true)
            {
                this.Title = "Три Нити Дизайн 1.0 - " + saveFile.SafeFileName;
                pathToFile = saveFile.FileName;
                string dots = "";
                for (int i = 0; i < ListFigure.Count; i++)
                    dots+= SavingFigures(ListFigure[i]);
                dots += SaveGroups(ListFigure);
                StreamWriter writer = new StreamWriter(saveFile.OpenFile());
                writer.WriteLine(dots);
                writer.Dispose();
                writer.Close();
            }
        }

        private void SaveProject(object sender, RoutedEventArgs e)
        {
            if(pathToFile != null)
            {
                string dots = "";
                for (int i = 0; i < ListFigure.Count; i++)
                    dots += SavingFigures(ListFigure[i]);
                dots += SaveGroups(ListFigure);
                StreamWriter writer = new StreamWriter(pathToFile);
                writer.WriteLine(dots);
                writer.Dispose();
                writer.Close();
            }
            else
                SaveProjectAs(null, null);
        }

        private void LoadProject(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "prj files| *.prj";
            Nullable<bool> result = op.ShowDialog();
            if (result == true)
            {
                pathToFile = op.FileName;
                this.Title = "Три Нити Дизайн 1.0 - " + op.SafeFileName;
                ClearEverything(true);
                StreamReader reader = new StreamReader(op.OpenFile());
                string text = reader.ReadToEnd();
                string pattern = @"([0-9]| |-|,|C|A|L)+!";
                Regex rgx = new Regex(pattern);
                MatchCollection matches = rgx.Matches(text);
                if(matches.Count == 0)
                    ListFigure.Add(new Figure(MainCanvas));
                for (int i = 0; i < matches.Count - 1; i++)
                {
                    string newStuff = matches[i].Value;
                    Figure fig = LoadingFigure(newStuff);
                    ListFigure.Add(fig);
                }
                LoadGroups(ListFigure, matches[matches.Count - 1].Value);
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            }
        }

        private void LoadPLT(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Corel PLT| *.plt";
            Nullable<bool> result = op.ShowDialog();
            if (result == true)
            {
                foreach (Figure fig in ListPltFigure)
                    fig.RemoveFigure(MainCanvas);
                ListPltFigure.Clear();
                StreamReader reader = new StreamReader(op.OpenFile());
                string text = reader.ReadToEnd();
                text = text.Replace("\r\n", "");
                text = text.Replace(";", "");
                text = text.Replace("PD", " ");
                text = text.Replace(",", " ");
                string pattern = @"([0-9]| |-)+";
                Regex rgx = new Regex(pattern);
                Vector vect;
                MatchCollection matches = rgx.Matches(text);
                List <List<Point>> pts = new List<List<Point>>();
                double minX = Double.MaxValue;
                double minY = -999999;
                for (int i = 0; i < matches.Count;i++ )
                {
                    string newStuff = matches[i].Value;
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
                vect = new Vector(minX -500, minY - 800);
                for(int i = 0; i< pts.Count;i++)
                {
                    ListPltFigure.Add(new Figure(MainCanvas));
                    for(int j = 0; j < pts[i].Count;j++)
                    {
                        Point newP = new Point((pts[i][j].X - vect.X), (-pts[i][j].Y - vect.Y));
                        ListPltFigure[ListPltFigure.Count - 1].AddPoint(newP, OptionColor.ColorPltFigure, false,false, 8);
                    }
                }
            }

        }

        private void DeletePLT(object sender, RoutedEventArgs e)
        {
            foreach(Figure fig in ListPltFigure)
                fig.RemoveFigure(MainCanvas);
            ListPltFigure.Clear();
            
            ListPltFigure.Add(new Figure(MainCanvas));
        }

        private void DeleteFigureClick(object sender, RoutedEventArgs e)
        {
            DeletedGroup.Clear();
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                List<Figure> group = new List<Figure>(ListFigure[IndexFigure].groupFigures);
                foreach (Figure fig in group)
                {
                    fig.groupFigures.Clear();
                    DeletedGroup.Add(DeepCopyFigure(fig));
                    fig.ClearFigure();
                }
                restore_button.IsEnabled = true;
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                ClearStatusBar();
            }
        }

        private void RestoreFigureClick(object sender, RoutedEventArgs e)
        {
            restore_button.IsEnabled = false;
            ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
            List<Figure> group = new List<Figure>(DeletedGroup);
            foreach (Figure fig in DeletedGroup)
            {
                fig.groupFigures.Clear();
                foreach (Figure figGroup in group)
                    fig.groupFigures.Add(figGroup);
                ListFigure.Add(fig);
            }
            IndexFigure = ListFigure.IndexOf(DeletedGroup[0]);
            DeletedGroup = new List<Figure>();
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            DrawOutsideRectangles(true, false, MainCanvas);
            ShowPositionStatus(ListFigure[IndexFigure], true, false);
        }

        private void CopyFigureClick(object sender, RoutedEventArgs e)
        {
            CopyGroup = new List<Figure>();
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                List<Figure> group = new List<Figure>(ListFigure[IndexFigure].groupFigures);
                foreach (Figure fig in group)
                {
                    CopyGroup.Add(DeepCopyFigure(fig));
                }
            }
        }

        private void PasteFigureClick(object sender, RoutedEventArgs e)
        {
            List<Figure> group = new List<Figure>(CopyGroup);
            foreach (Figure fig in CopyGroup)
            {
                fig.ChangeFigureColor(OptionColor.ColorSelection, false);
                fig.groupFigures.Clear();
                foreach (Figure figGroup in group)
                    fig.groupFigures.Add(figGroup);
                ListFigure.Add(fig);
            }
            CopyGroup = new List<Figure>();
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            DrawOutsideRectangles(true, false, MainCanvas);
        }

        private void CopyFigureFromClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "ell files (*.ell)|*.ell";
            Nullable<bool> result = saveFile.ShowDialog();
            if (result == true)
            {
                string dots = "";
                foreach(Figure fig in ListFigure[IndexFigure].groupFigures)
                    dots += SavingFigures(fig);
                StreamWriter writer = new StreamWriter(saveFile.OpenFile());
                writer.WriteLine(dots);
                writer.Dispose();
                writer.Close();
            }
        }

        private void PasteFigureFromClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "ell files| *.ell";
            Nullable<bool> result = op.ShowDialog();
            if (result == true)
            {
                StreamReader reader = new StreamReader(op.OpenFile());
                string text = reader.ReadToEnd();
                string pattern = @"([0-9]| |-|,|C|A|L)+!";
                Regex rgx = new Regex(pattern);
                MatchCollection matches = rgx.Matches(text);
                List<Figure> group = new List<Figure>();
                for (int i = 0; i < matches.Count; i++)
                {
                    string newStuff = matches[i].Value;
                    Figure fig = LoadingFigure(newStuff);
                    group.Add(fig);
                    fig.ChangeFigureColor(OptionColor.ColorDraw, false);
                    ListFigure.Add(fig);
                }
                foreach(Figure fig in group)
                {
                    fig.groupFigures.Clear();
                    foreach (Figure newFig in group)
                        fig.groupFigures.Add(newFig);
                }
                ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
                IndexFigure = ListFigure.IndexOf(group[0]);
                
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                DrawOutsideRectangles(true, false, MainCanvas);
            }
        }

        private void RefreshImageClick(object sender, RoutedEventArgs e)
        {
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            DrawOutsideRectangles(true, false, MainCanvas);
        }

        private void CreatePltClick(object sender, RoutedEventArgs e)
        {
            foreach (Figure fig in ListFigure[IndexFigure].groupFigures)
            {
                Figure newFig = DeepCopyFigure(fig);
                newFig.ChangeFigureColor(OptionColor.ColorPltFigure, false);
                foreach (Shape sh in newFig.InvShapes)
                    MainCanvas.Children.Remove(sh);
                newFig.InvShapes.Clear();
                newFig.DictionaryInvLines.Clear();
                ListPltFigure.Add(newFig);
            }
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            DrawOutsideRectangles(true, false, MainCanvas);
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Проект Три Нити Дизайн| *.tri";
            op.ShowDialog();
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

        private void ShowSpecialWindow(object sender, RoutedEventArgs e)
        {
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                var SpecialWindow = new View.SpecialWindowWhenSelectedFigure();
                SpecialWindow.ShowDialog();
                if (OptionRegim.regim == Regim.RegimDrawStegki)
                {
                    CursorMenuDrawStegki();
                }
                else if (OptionRegim.regim == Regim.RegimDrawInColor)
                {
                    CursorMenuDrawInColor();
                }
                else if (OptionRegim.regim == Regim.RegimOtshit)
                {
                    CursorMenuOtshit();
                }
            }
        }
    }    
}