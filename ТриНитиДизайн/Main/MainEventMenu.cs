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
            //ClearEverything(false);
            //pathToFile = null;
            //this.Title = "Три Нити Дизайн 1.0 ";
        }

        private void SaveProjectAs(object sender, RoutedEventArgs e)
        {
            //SaveFileDialog saveFile = new SaveFileDialog();
            //saveFile.Filter = "prj files (*.prj)|*.prj";
            //Nullable<bool> result = saveFile.ShowDialog();
            //if (result == true)
            //{
            //    this.Title = "Три Нити Дизайн 1.0 - " + saveFile.SafeFileName;
            //    pathToFile = saveFile.FileName;
            //    string dots = "";
            //    for (int i = 0; i < listFigure.Count; i++)
            //        dots+= SavingFigures(listFigure[i]);
            //    dots += SaveGroups(listFigure);
            //    StreamWriter writer = new StreamWriter(saveFile.OpenFile());
            //    writer.WriteLine(dots);
            //    writer.Dispose();
            //    writer.Close();
            //}
        }

        private void SaveProject(object sender, RoutedEventArgs e)
        {
            //if(pathToFile != null)
            //{
            //    string dots = "";
            //    for (int i = 0; i < listFigure.Count; i++)
            //        dots += SavingFigures(listFigure[i]);
            //    dots += SaveGroups(listFigure);
            //    StreamWriter writer = new StreamWriter(pathToFile);
            //    writer.WriteLine(dots);
            //    writer.Dispose();
            //    writer.Close();
            //}
            //else
            //    SaveProjectAs(null, null);
        }

        private void LoadProject(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog op = new OpenFileDialog();
            //op.Filter = "prj files| *.prj";
            //Nullable<bool> result = op.ShowDialog();
            //if (result == true)
            //{
            //    pathToFile = op.FileName;
            //    this.Title = "Три Нити Дизайн 1.0 - " + op.SafeFileName;
            //    ClearEverything(true);
            //    StreamReader reader = new StreamReader(op.OpenFile());
            //    string text = reader.ReadToEnd();
            //    string pattern = @"([0-9]| |-|,|C|A|L)+!";
            //    Regex rgx = new Regex(pattern);
            //    MatchCollection matches = rgx.Matches(text);
            //    if(matches.Count == 0)
            //        listFigure.Add(new Figure(mainCanvas));
            //    for (int i = 0; i < matches.Count - 1; i++)
            //    {
            //        string newStuff = matches[i].Value;
            //        Figure fig = LoadingFigure(newStuff);
            //        listFigure.Add(fig);
            //    }
            //    LoadGroups(listFigure, matches[matches.Count - 1].Value);
            //    RedrawEverything(listFigure, indexFigure, false, mainCanvas);
            //}
        }

        private void LoadPLT(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog op = new OpenFileDialog();
            //op.Filter = "Corel PLT| *.plt";
            //Nullable<bool> result = op.ShowDialog();
            //if (result == true)
            //{
            //    foreach (Figure fig in listPltFigure)
            //        fig.RemoveFigure(mainCanvas);
            //    listPltFigure.Clear();
            //    StreamReader reader = new StreamReader(op.OpenFile());
            //    string text = reader.ReadToEnd();
            //    text = text.Replace("\r\n", "");
            //    text = text.Replace(";", "");
            //    text = text.Replace("PD", " ");
            //    text = text.Replace(",", " ");
            //    string pattern = @"([0-9]| |-)+";
            //    Regex rgx = new Regex(pattern);
            //    Vector vect;
            //    MatchCollection matches = rgx.Matches(text);
            //    List <List<Point>> pts = new List<List<Point>>();
            //    double minX = Double.MaxValue;
            //    double minY = -999999;
            //    for (int i = 0; i < matches.Count;i++ )
            //    {
            //        string newStuff = matches[i].Value;
            //        pattern = @" ";
            //        String[] elements = Regex.Split(newStuff, pattern);
            //        double del = 22;
            //        pts.Add(new List<Point>());
            //        for (int j = 0; j < elements.Length; j += 2)
            //        {
            //            Point newP = new Point(Double.Parse(elements[j]) / del, Double.Parse(elements[j + 1]) / del);
            //            if(newP.X < minX)
            //            {
            //                minX = newP.X;
            //            }
            //            if(newP.Y > minY)
            //            {
            //                minY = newP.Y;
            //            }
            //            pts[i].Add(newP);
            //        }
            //    }
            //    vect = new Vector(minX -500, minY - 800);
            //    for(int i = 0; i< pts.Count;i++)
            //    {
            //        listPltFigure.Add(new Figure(mainCanvas));
            //        for(int j = 0; j < pts[i].Count;j++)
            //        {
            //            Point newP = new Point((pts[i][j].X - vect.X), (-pts[i][j].Y - vect.Y));
            //            listPltFigure[listPltFigure.Count - 1].AddPoint(newP, OptionColor.colorPltFigure, false,false, 8);
            //        }
            //    }
            //}
        }

        private void DeletePLT(object sender, RoutedEventArgs e)
        {
            //foreach(Figure fig in listPltFigure)
            //    fig.RemoveFigure(mainCanvas);
            //listPltFigure.Clear();
            
            //listPltFigure.Add(new Figure(mainCanvas));
        }

        private void DeleteFigureClick(object sender, RoutedEventArgs e)
        {
            //deletedGroup.Clear();
            //if (listFigure[indexFigure].points.Count > 0)
            //{
            //    List<Figure> group = new List<Figure>(listFigure[indexFigure].groupFigures);
            //    foreach (Figure fig in group)
            //    {
            //        fig.groupFigures.Clear();
            //        deletedGroup.Add(DeepCopyFigure(fig));
            //        fig.ClearFigure();
            //    }
            //    restore_button.IsEnabled = true;
            //    RedrawEverything(listFigure, indexFigure, false, mainCanvas);
            //    ClearStatusBar();
            //}
        }

        private void RestoreFigureClick(object sender, RoutedEventArgs e)
        {
            //restore_button.IsEnabled = false;
            //listFigure[indexFigure].ChangeFigureColor(OptionColor.colorInactive, false);
            //List<Figure> group = new List<Figure>(deletedGroup);
            //foreach (Figure fig in deletedGroup)
            //{
            //    fig.groupFigures.Clear();
            //    foreach (Figure figGroup in group)
            //        fig.groupFigures.Add(figGroup);
            //    listFigure.Add(fig);
            //}
            //indexFigure = listFigure.IndexOf(deletedGroup[0]);
            //deletedGroup = new List<Figure>();
            //RedrawEverything(listFigure, indexFigure, false, mainCanvas);
            //DrawOutsideRectangles(true, false, mainCanvas);
            //ShowPositionStatus(listFigure[indexFigure], true, false);
        }

        private void CopyFigureClick(object sender, RoutedEventArgs e)
        {
            //copyGroup = new List<Figure>();
            //if (listFigure[indexFigure].points.Count > 0)
            //{
            //    List<Figure> group = new List<Figure>(listFigure[indexFigure].groupFigures);
            //    foreach (Figure fig in group)
            //    {
            //        copyGroup.Add(DeepCopyFigure(fig));
            //    }
            //}
        }

        private void PasteFigureClick(object sender, RoutedEventArgs e)
        {
            //List<Figure> group = new List<Figure>(copyGroup);
            //foreach (Figure fig in copyGroup)
            //{
            //    fig.ChangeFigureColor(OptionColor.colorInactive, false);
            //    fig.groupFigures.Clear();
            //    foreach (Figure figGroup in group)
            //        fig.groupFigures.Add(figGroup);
            //    listFigure.Add(fig);
            //}
            //copyGroup = new List<Figure>();
            //DrawOutsideRectangles(true, false, mainCanvas);
        }

        private void CopyFigureFromClick(object sender, RoutedEventArgs e)
        {
            //SaveFileDialog saveFile = new SaveFileDialog();
            //saveFile.Filter = "ell files (*.ell)|*.ell";
            //Nullable<bool> result = saveFile.ShowDialog();
            //if (result == true)
            //{
            //    string dots = "";
            //    foreach(Figure fig in listFigure[indexFigure].groupFigures)
            //        dots += SavingFigures(fig);
            //    StreamWriter writer = new StreamWriter(saveFile.OpenFile());
            //    writer.WriteLine(dots);
            //    writer.Dispose();
            //    writer.Close();
            //}
        }

        private void PasteFigureFromClick(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog op = new OpenFileDialog();
            //op.Filter = "ell files| *.ell";
            //Nullable<bool> result = op.ShowDialog();
            //if (result == true)
            //{
            //    StreamReader reader = new StreamReader(op.OpenFile());
            //    string text = reader.ReadToEnd();
            //    string pattern = @"([0-9]| |-|,|C|A|L)+!";
            //    Regex rgx = new Regex(pattern);
            //    MatchCollection matches = rgx.Matches(text);
            //    List<Figure> group = new List<Figure>();
            //    for (int i = 0; i < matches.Count; i++)
            //    {
            //        string newStuff = matches[i].Value;
            //        Figure fig = LoadingFigure(newStuff);
            //        group.Add(fig);
            //        fig.ChangeFigureColor(OptionColor.colorActive, false);
            //        listFigure.Add(fig);
            //    }
            //    foreach(Figure fig in group)
            //    {
            //        fig.groupFigures.Clear();
            //        foreach (Figure newFig in group)
            //            fig.groupFigures.Add(newFig);
            //    }
            //    listFigure[indexFigure].ChangeFigureColor(OptionColor.colorInactive, false);
            //    indexFigure = listFigure.IndexOf(group[0]);
                
            //    RedrawEverything(listFigure, indexFigure, false, mainCanvas);
            //    DrawOutsideRectangles(true, false, mainCanvas);
            //}
        }

        private void RefreshImageClick(object sender, RoutedEventArgs e)
        {
            //RedrawEverything(listFigure, indexFigure, false, mainCanvas);
            //DrawOutsideRectangles(true, false, mainCanvas);
        }

        private void CreatePltClick(object sender, RoutedEventArgs e)
        {
            //foreach (Figure fig in listFigure[indexFigure].groupFigures)
            //{
            //    Figure newFig = DeepCopyFigure(fig);
            //    newFig.ChangeFigureColor(OptionColor.colorPltFigure, false);
            //    foreach (Shape sh in newFig.InvShapes)
            //        mainCanvas.Children.Remove(sh);
            //    newFig.InvShapes.Clear();
            //    newFig.DictionaryInvLines.Clear();
            //    listPltFigure.Add(newFig);
            //}
            //RedrawEverything(listFigure, indexFigure, false, mainCanvas);
            //DrawOutsideRectangles(true, false, mainCanvas);
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
            //if (listFigure[indexFigure].points.Count > 0)
            //{
            //    var SpecialWindow = new View.SpecialWindowWhenSelectedFigure(listFigure[indexFigure].groupFigures,
            //        unembroidLines, firstRec, lastRec, mainCanvas);
            //    SpecialWindow.ShowDialog();
            //    if (OptionMode.mode == Mode.modeDrawInColor)
            //    {
            //        CursorMenuDrawInColor();
            //    }
            //}
        }
    }    
}