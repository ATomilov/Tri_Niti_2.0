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
            this.Title = "Три Нити Дизайн 1.0 ";
        }

        private void SaveProjectAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "txt files (*.txt)|*.txt";
            Nullable<bool> result = saveFile.ShowDialog();
            if (result == true)
            {
                this.Title = "Три Нити Дизайн 1.0 - " + saveFile.SafeFileName;
                pathToFile = saveFile.FileName;
                string dots = "";
                for (int i = 0; i < ListFigure.Count; i++)
                {
                    if (ListFigure[i].Points.Count > 0)
                    {
                        dots += ListFigure[i].Points[0].X;
                        dots += " ";
                        dots += ListFigure[i].Points[0].Y;
                        dots += " ";
                        for (int j = 1; j < ListFigure[i].Points.Count; j++)
                        {
                            Shape sh;
                            ListFigure[i].DictionaryPointLines.TryGetValue(ListFigure[i].Points[j - 1], out sh);
                            if (sh is Path)
                            {
                                if (sh.MinHeight == 10)
                                    dots += "S";
                                else
                                {
                                    if (sh.MinHeight == 5)
                                        dots += "C";
                                    else
                                        dots += "A";

                                    Point contP;
                                    ListFigure[i].DictionaryShapeControlPoints.TryGetValue(ListFigure[i].Points[j - 1], out contP);
                                    dots += " ";
                                    dots += contP.X;
                                    dots += " ";
                                    dots += contP.Y;
                                }
                            }
                            else
                                dots += "L";
                            dots += " ";
                            dots += ListFigure[i].Points[j].X;
                            dots += " ";
                            dots += ListFigure[i].Points[j].Y;
                            dots += " ";
                        }
                        dots += "!";
                    }
                }
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
                {
                    if (ListFigure[i].Points.Count > 0)
                    {
                        dots += ListFigure[i].Points[0].X;
                        dots += " ";
                        dots += ListFigure[i].Points[0].Y;
                        dots += " ";
                        for (int j = 1; j < ListFigure[i].Points.Count; j++)
                        {
                            Shape sh;
                            ListFigure[i].DictionaryPointLines.TryGetValue(ListFigure[i].Points[j - 1], out sh);
                            if (sh is Path)
                            {
                                if (sh.MinHeight == 10)
                                    dots += "S";
                                else
                                {
                                    if (sh.MinHeight == 5)
                                        dots += "C";
                                    else
                                        dots += "A";

                                    Point contP;
                                    ListFigure[i].DictionaryShapeControlPoints.TryGetValue(ListFigure[i].Points[j - 1], out contP);
                                    dots += " ";
                                    dots += contP.X;
                                    dots += " ";
                                    dots += contP.Y;
                                }
                            }
                            else
                                dots += "L";
                            dots += " ";
                            dots += ListFigure[i].Points[j].X;
                            dots += " ";
                            dots += ListFigure[i].Points[j].Y;
                            dots += " ";
                        }
                        dots += "!";
                    }
                }
                StreamWriter writer = new StreamWriter(pathToFile);
                writer.WriteLine(dots);
                writer.Dispose();
                writer.Close();
            }
        }

        private void LoadProject(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "txt files| *.txt";
            Nullable<bool> result = op.ShowDialog();
            if (result == true)
            {
                pathToFile = op.FileName;
                this.Title = "Три Нити Дизайн 1.0 - " + op.SafeFileName;
                ClearEverything(true);
                StreamReader reader = new StreamReader(op.OpenFile());
                string text = reader.ReadToEnd();
                string pattern = @"([0-9]| |-|,|C|A|S|L)+!";
                Regex rgx = new Regex(pattern);
                MatchCollection matches = rgx.Matches(text);
                if(matches.Count == 0)
                {
                    ListFigure.Add(new Figure(MainCanvas));
                }
                for (int i = 0; i < matches.Count; i++)
                {           
                    ListFigure.Add(new Figure(MainCanvas));
                    string newStuff = matches[i].Value;
                    pattern = @" ";
                    String[] elements = Regex.Split(newStuff, pattern);
                    Point p = new Point(Double.Parse(elements[0]), Double.Parse(elements[1]));
                    ListFigure[i].AddPoint(p, OptionColor.ColorSelection, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                    int j = 2;
                    List<Point> ptsForCurves = new List<Point>();
                    Dictionary<Point, string> tempDicShape = new Dictionary<Point, string>();
                    Dictionary<Point, Point> tempDicContPoints = new Dictionary<Point, Point>();
                    while(!elements[j].Equals("!"))
                    {
                        if(elements[j].Equals("L"))
                        {
                            p = new Point(Double.Parse(elements[j+1]), Double.Parse(elements[j+2]));
                            ListFigure[i].AddPoint(p, OptionColor.ColorSelection, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                        }
                        else if(elements[j].Equals("C"))
                        {
                            ptsForCurves.Add(p);
                            tempDicContPoints.Add(p, new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2])));
                            tempDicShape.Add(p, "C");
                            j+=2;
                            p = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
                            ptsForCurves.Add(p);
                            ListFigure[i].AddPoint(p, OptionColor.ColorSelection, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                        }
                        else if (elements[j].Equals("A"))
                        {
                            ptsForCurves.Add(p);
                            tempDicContPoints.Add(p, new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2])));
                            tempDicShape.Add(p, "A");
                            j += 2;
                            p = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
                            ptsForCurves.Add(p);
                            ListFigure[i].AddPoint(p, OptionColor.ColorSelection, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                        }
                        else if (elements[j].Equals("S"))
                        {
                            p = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
                            ListFigure[i].AddPoint(p, OptionColor.ColorSelection, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                            int index = ListFigure[i].Points.IndexOf(p);
                            if (!ListFigure[i].PointsCount.Contains(index - 1))
                                ListFigure[i].PointsCount.Add(index - 1);
                            if (!ListFigure[i].PointsCount.Contains(index))
                                ListFigure[i].PointsCount.Add(index);
                        }
                        j += 3;
                    }
                    if (ListFigure[i].PointsCount.Count > 0)
                        MakeSpline(ListFigure[i], OptionColor.ColorSelection, MainCanvas);

                    for (int z = 0; z < ptsForCurves.Count; z += 2)
                    {
                        Shape sh;
                        ListFigure[i].DictionaryPointLines.TryGetValue(ptsForCurves[z], out sh);
                        ListFigure[i].DeleteShape(sh, ptsForCurves[z], MainCanvas);
                        string type;
                        tempDicShape.TryGetValue(ptsForCurves[z], out type);
                        Point contP;
                        tempDicContPoints.TryGetValue(ptsForCurves[z], out contP);
                        if (type.Equals("C"))
                        {
                            List<Point> pts = new List<Point>();
                            pts.Add(ptsForCurves[z]);
                            pts.Add(contP);
                            pts.Add(ptsForCurves[z + 1]);
                            sh = SetSpline(5, 0.75, pts, true, OptionColor.ColorSelection, MainCanvas);
                        }
                        else
                            sh = SetArc(OptionColor.ColorSelection, ptsForCurves[z], ptsForCurves[z + 1], contP, MainCanvas);
                        ListFigure[i].AddShape(sh, ptsForCurves[z], contP);
                    }
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
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);

        }

        private void CopyFigureClick(object sender, RoutedEventArgs e)
        {
            CopyFigure = new Figure(MainCanvas);
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                for(int i = 0; i < ListFigure[IndexFigure].Points.Count; i++)
                {
                    Point p = ListFigure[IndexFigure].Points[i];
                    if (i != ListFigure[IndexFigure].Points.Count - 1)
                    {
                        Shape sh;
                        ListFigure[IndexFigure].DictionaryPointLines.TryGetValue(p, out sh);
                        Point contP;
                        ListFigure[IndexFigure].DictionaryShapeControlPoints.TryGetValue(p, out contP);
                        Shape newSh = DeepCopy(sh);
                        CopyFigure.AddShape(newSh, p, contP);
                    }
                    CopyFigure.Points.Add(p);
                }
                CopyFigure.PointStart = ListFigure[IndexFigure].Points[0];
                CopyFigure.PointEnd = ListFigure[IndexFigure].Points[ListFigure[IndexFigure].Points.Count - 1];
            }
        }

       public Shape DeepCopy(Shape element)
       {
           string shapestring = XamlWriter.Save(element);
           StringReader stringReader = new StringReader(shapestring);
           XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
           Shape DeepCopyobject = (Shape)XamlReader.Load(xmlTextReader);
           return DeepCopyobject;
       }

        private void PasteFigureClick(object sender, RoutedEventArgs e)
        {
            CopyFigure.ChangeFigureColor(OptionColor.ColorSelection, false);
            ListFigure.Add(CopyFigure);
            CopyFigure = new Figure(MainCanvas);
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