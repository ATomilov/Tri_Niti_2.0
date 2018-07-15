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
using System.Windows.Markup;
using System.Xml;

namespace ТриНитиДизайн
{
    public partial class MainWindow : Window
    {
        public void CloseAllTabs()
        {
            expander1.IsExpanded = false;
            expander1.Visibility = Visibility.Collapsed;
            if (tabControl1.Visibility == Visibility.Visible)
                tabControl1.Visibility = Visibility.Hidden;
            if (tabControl2.Visibility == Visibility.Visible)
                tabControl2.Visibility = Visibility.Hidden;
        }

        //public void SetToDefault()
        //{
        //    OptionDrawLine.strokeThickness = 1;
        //    OptionDrawLine.strokeThicknessMainRec = 2;
        //    OptionDrawLine.invisibleStrokeThickness = 10;
        //    OptionDrawLine.sizeRectangle = 8;
        //    OptionDrawLine.oneDotCornerDistance = 3;
        //    OptionDrawLine.oneDotMiddleDistance = 4;
        //    OptionDrawLine.sizeRectangleForScale = 10;
        //    OptionDrawLine.sizeRectangleForRotation = 15;
        //    OptionDrawLine.cursorModeRectangleDistance = 10;
        //    OptionGrid.scaleMultiplier = 1;
        //    ScaleTransform scaleTransform = new ScaleTransform(OptionGrid.scaleMultiplier, OptionGrid.scaleMultiplier);
        //    mainCanvas.RenderTransform = scaleTransform;
        //    previousViewList.Clear();
        //    PreviousView firstView = new PreviousView(1, 0, 0);
        //    previousViewList.Add(firstView);
        //}

        //public string SavingFigures(Figure fig)
        //{
        //    string dots = "";
        //    if (fig.points.Count > 0)
        //    {
        //        dots += fig.points[0].X;
        //        dots += " ";
        //        dots += fig.points[0].Y;
        //        dots += " ";
        //        for (int j = 1; j < fig.points.Count; j++)
        //        {
        //            Shape sh;
        //            fig.DictionaryPointLines.TryGetValue(fig.points[j - 1], out sh);
        //            if (sh is Path)
        //            {
        //                Tuple<Point, Point> contP;
        //                fig.shapeControlPoints.TryGetValue(fig.points[j - 1], out contP);
        //                if (sh.MinHeight == 5)
        //                {
        //                    dots += "C";
        //                    dots += " ";
        //                    dots += contP.Item1.X;
        //                    dots += " ";
        //                    dots += contP.Item1.Y;
        //                    dots += " ";
        //                    dots += contP.Item2.X;
        //                    dots += " ";
        //                    dots += contP.Item2.Y;
        //                }
        //                else if(sh.MinHeight == 10)
        //                {
        //                    dots += "A";
        //                    dots += " ";
        //                    dots += contP.Item1.X;
        //                    dots += " ";
        //                    dots += contP.Item1.Y;
        //                }
        //            }
        //            else
        //                dots += "L";
        //            dots += " ";
        //            dots += fig.points[j].X;
        //            dots += " ";
        //            dots += fig.points[j].Y;
        //            dots += " ";
        //        }
        //        dots += "!";
        //    }
        //    return dots;
        //}

        //public string SaveGroups(List<Figure> FigureList)
        //{
        //    string groups = "";
        //    int[] array = new int[FigureList.Count];
        //    int groupNum = 1;
        //    for (int i = 0; i < FigureList.Count; i++)
        //    {
        //        if (array[i] == 0 && FigureList[i].points.Count > 0)
        //        {
        //            foreach (Figure fig in FigureList[i].groupFigures)
        //            {
        //                array[FigureList.IndexOf(fig)] = groupNum;
        //            }
        //            groupNum++;
        //        }
        //    }
        //    for (int i = 0; i < array.Length; i++ )
        //    {
        //        if (array[i] != 0)
        //        {
        //            groups += " ";
        //            groups += array[i];
        //        }
        //    }
        //    groups += " ";
        //    groups += "!";
        //    return groups;
        //}

        //public void LoadGroups(List<Figure> FigureList, string groups)
        //{
        //    string pattern = @" ";
        //    String[] elements = Regex.Split(groups, pattern);
        //    int[] groupsNum = new int[FigureList.Count];
        //    for (int i = 1; i < elements.Length - 1; i++)
        //    {
        //        groupsNum[i - 1] = Int32.Parse(elements[i]);
        //    }
        //    for(int i = 0; i < FigureList.Count; i++)
        //    {
        //        FigureList[i].groupFigures.Clear();
        //        int currentNum = groupsNum[i];
        //        for (int j = 0; j < groupsNum.Length; j++)
        //        {
        //            if(currentNum == groupsNum[j])
        //            {
        //                FigureList[i].groupFigures.Add(FigureList[j]);
        //            }
        //        }
        //    }
        //}

        //public Figure DeepCopyFigure(Figure figureToCopy)
        //{
        //    Figure newFigure = new Figure(mainCanvas);
        //    {
        //        for (int i = 0; i < figureToCopy.points.Count; i++)
        //        {
        //            Point p = figureToCopy.points[i];
        //            if (i != figureToCopy.points.Count - 1 || figureToCopy.points.Count == 1)
        //            {
        //                Shape sh;
        //                figureToCopy.DictionaryPointLines.TryGetValue(p, out sh);
        //                Tuple<Point, Point> contP;
        //                figureToCopy.shapeControlPoints.TryGetValue(p, out contP);
        //                Shape newSh = DeepCopy(sh);
        //                newFigure.AddShape(newSh, p, contP);
        //            }
        //            newFigure.points.Add(p);
        //        }
        //        newFigure.pointStart = figureToCopy.points[0];
        //        newFigure.pointEnd = figureToCopy.points[figureToCopy.points.Count - 1];
        //    }
        //    return newFigure;
        //}

        //public Shape DeepCopy(Shape element)
        //{
        //    string shapestring = XamlWriter.Save(element);
        //    StringReader stringReader = new StringReader(shapestring);
        //    XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
        //    Shape DeepCopyobject = (Shape)XamlReader.Load(xmlTextReader);
        //    return DeepCopyobject;
        //}

        //public Figure LoadingFigure(string newStuff)
        //{
        //    Figure fig = new Figure(mainCanvas);
        //    string pattern = @" ";
        //    String[] elements = Regex.Split(newStuff, pattern);
        //    Point p = new Point(Double.Parse(elements[0]), Double.Parse(elements[1]));
        //    fig.AddPoint(p, OptionColor.colorInactive, false, true, OptionDrawLine.sizeRectangle);
        //    int j = 2;
        //    while (!elements[j].Equals("!"))
        //    {
        //        if (elements[j].Equals("L"))
        //        {
        //            p = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
        //            fig.AddPoint(p, OptionColor.colorInactive, false, true, OptionDrawLine.sizeRectangle);
        //        }
        //        else if (elements[j].Equals("C"))
        //        {
        //            Point firstContPoint = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
        //            Point secondContPoint = new Point(Double.Parse(elements[j + 3]), Double.Parse(elements[j + 4]));
        //            j += 4;
        //            Point p1 = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
        //            Shape sh = GeometryHelper.SetBezier(OptionColor.colorInactive, p, firstContPoint, secondContPoint, p1, mainCanvas);
        //            fig.AddShape(sh, p, new Tuple<Point, Point>(firstContPoint, secondContPoint));
        //            p = p1;
        //            fig.points.Add(p);
        //            fig.pointEnd = fig.points[fig.points.Count - 1];
        //        }
        //        else if (elements[j].Equals("A"))
        //        {
        //            Point contPoint = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
        //            j += 2;
        //            Point p1 = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
        //            Shape sh = GeometryHelper.SetArc(OptionColor.colorInactive, p, p1, contPoint, mainCanvas);
        //            fig.AddShape(sh, p, new Tuple<Point, Point>(contPoint, new Point()));
        //            p = p1;
        //            fig.points.Add(p);
        //            fig.pointEnd = fig.points[fig.points.Count - 1];
        //        }
        //        j += 3;
        //    }
        //    return fig;
        //}

        //private void ClearEverything(bool isListEmpty)
        //{
        //    listFigure.Clear();
        //    if(!isListEmpty)
        //        listFigure.Add(new Figure(mainCanvas));
        //    mainCanvas.Children.Clear();
        //    listPltFigure.Clear();
        //    OptionMode.mode = Mode.modeNull;
        //    OptionColor.colorNewBackground = Brushes.LightSkyBlue;
        //    OptionColor.colorNewActive = Brushes.Violet;
        //    tatamiFigures.Clear();
        //    indexFigure = 0;
        //    Edit_Menu.IsEnabled = false;
        //    deletedGroup = new List<Figure>();
        //    copyGroup = new List<Figure>();
        //    firstSatinFigure = -1;
        //    secondSatinFigure = -1;
        //    CloseAllTabs();
        //    gridFigure.RemoveFigure(mainCanvas);
        //    SetToDefault();
        //    mainCanvas.Cursor = defaultCursor;
        //    ClearStatusBar();
        //    panTransform = new TranslateTransform();
        //    zoomTransform = new ScaleTransform();
        //    bothTransforms = new TransformGroup();
        //    bothTransforms.Children.Add(panTransform);
        //    bothTransforms.Children.Add(zoomTransform);
        //    mainCanvas.RenderTransform = bothTransforms;
        //    SetGrid();
        //    foreach (Line ln in centerLines)
        //    {
        //        ln.StrokeThickness = OptionDrawLine.strokeThickness;
        //        mainCanvas.Children.Add(ln);
        //    }
        //}        

        //public bool ShowAcceptMessage(int choice)
        //{
        //    string sMessageBoxText = "Соединить?";
        //    string sCaption = "Стежки";
        //    if(choice == 0)
        //    {
        //        sMessageBoxText = "Создать цепочку стежков?";
        //    }
        //    if(choice == 1)
        //    {
        //        sMessageBoxText = "Создать татами?";
        //    }
        //    if (choice == 2)
        //    {
        //        sMessageBoxText = "Создать стежки?";
        //        if(OptionMode.mode == Mode.modeTatami)
        //        {
        //            sCaption = "Татами";
        //        }
        //        if (OptionMode.mode == Mode.modeRunStitch)
        //        {
        //            sCaption = "Цепочка стежков";
        //        }
        //        if (OptionMode.mode == Mode.modeSatin)
        //        {
        //            sCaption = "Гладь";
        //        }

        //    }
        //    MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;

        //    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox);

        //    switch (rsltMessageBox)
        //    {
        //        case MessageBoxResult.OK:
        //            return true;

        //        case MessageBoxResult.Cancel:
        //            return false;
        //    }
        //    return false;
        //}
    }
}
