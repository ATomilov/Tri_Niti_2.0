﻿using System;
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
                for (int i = 0; i < matches.Count; i++)
                {
                    string newStuff = matches[i].Value;
                    Figure fig = LoadingFigure(newStuff);
                    ListFigure.Add(fig);
                }
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
                fig.RemoveFigure(MainCanvas);
            ListPltFigure.Clear();
            
            ListPltFigure.Add(new Figure(MainCanvas));
        }

        private void DeleteFigureClick(object sender, RoutedEventArgs e)
        {
            if (ListFigure[IndexFigure].Points.Count > 1)
            {
                DeletedFigure = DeepCopyFigure(ListFigure[IndexFigure]);
                restore_button.IsEnabled = true;
                ListFigure[IndexFigure] = new Figure(MainCanvas);
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            }
        }

        private void RestoreFigureClick(object sender, RoutedEventArgs e)
        {
            restore_button.IsEnabled = false;
            ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
            ListFigure.Add(DeletedFigure);
            IndexFigure = ListFigure.IndexOf(DeletedFigure);
            DeletedFigure = new Figure(MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            DrawFirstAndLastRectangle();
            DrawOutsideRectangles(true, false, MainCanvas);
        }

        private void CopyFigureClick(object sender, RoutedEventArgs e)
        {
            CopyFigure = new Figure(MainCanvas);
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                CopyFigure = DeepCopyFigure(ListFigure[IndexFigure]);
            }
        }

        private void PasteFigureClick(object sender, RoutedEventArgs e)
        {
            CopyFigure.ChangeFigureColor(OptionColor.ColorSelection, false);
            ListFigure.Add(CopyFigure);
            CopyFigure = new Figure(MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            DrawFirstAndLastRectangle();
            DrawOutsideRectangles(true, false, MainCanvas);
        }

        private void CopyFigureFromClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "ell files (*.ell)|*.ell";
            Nullable<bool> result = saveFile.ShowDialog();
            if (result == true)
            {
                this.Title = "Три Нити Дизайн 1.0 - " + saveFile.SafeFileName;
                pathToFile = saveFile.FileName;
                string dots;
                dots = SavingFigures(ListFigure[IndexFigure]);
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
                pathToFile = op.FileName;
                this.Title = "Три Нити Дизайн 1.0 - " + op.SafeFileName;
                StreamReader reader = new StreamReader(op.OpenFile());
                string text = reader.ReadToEnd();
                string pattern = @"([0-9]| |-|,|C|A|L)+!";
                Regex rgx = new Regex(pattern);
                MatchCollection matches = rgx.Matches(text);
                for (int i = 0; i < matches.Count; i++)
                {
                    string newStuff = matches[i].Value;
                    Figure fig = LoadingFigure(newStuff);
                    fig.ChangeFigureColor(OptionColor.ColorDraw, false);
                    ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
                    ListFigure.Add(fig);
                    IndexFigure = ListFigure.IndexOf(fig);
                }
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            }
        }

        private void RefreshImageClick(object sender, RoutedEventArgs e)
        {
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            DrawFirstAndLastRectangle();
            DrawOutsideRectangles(true, false, MainCanvas);
        }

        private void CreatePltClick(object sender, RoutedEventArgs e)
        {
            Figure newFig = DeepCopyFigure(ListFigure[IndexFigure]);
            newFig.ChangeFigureColor(OptionColor.ColorPltFigure, false);
            ListPltFigure.Add(newFig);
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            DrawFirstAndLastRectangle();
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
                if (OptionRegim.regim == Regim.RegimDrawInColor)
                {
                    CursorMenuDrawInColor();
                }
            }
        }
    }    
}