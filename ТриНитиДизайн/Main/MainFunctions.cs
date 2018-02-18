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

        public void SetToDefault()
        {
            OptionDrawLine.StrokeThickness = 1;
            OptionDrawLine.StrokeThicknessMainRec = 2;
            OptionDrawLine.InvisibleStrokeThickness = 10;
            OptionDrawLine.SizeWidthAndHeightRectangle = 8;
            OptionDrawLine.OneDotCornerDistance = 3;
            OptionDrawLine.OneDotMiddleDistance = 4;
            OptionDrawLine.SizeRectangleForScale = 10;
            OptionDrawLine.SizeRectangleForRotation = 15;
            OptionSetka.Masshtab = 1;
            ScaleTransform scaleTransform = new ScaleTransform(OptionSetka.Masshtab, OptionSetka.Masshtab);
            MainCanvas.RenderTransform = scaleTransform;
            PreviousViewList.Clear();
            PreviousView firstView = new PreviousView(1, 0, 0);
            PreviousViewList.Add(firstView);
        }

        public string SavingFigures(Figure fig)
        {
            string dots = "";
            if (fig.Points.Count > 0)
            {
                dots += fig.Points[0].X;
                dots += " ";
                dots += fig.Points[0].Y;
                dots += " ";
                for (int j = 1; j < fig.Points.Count; j++)
                {
                    Shape sh;
                    fig.DictionaryPointLines.TryGetValue(fig.Points[j - 1], out sh);
                    if (sh is Path)
                    {
                        Tuple<Point, Point> contP;
                        fig.DictionaryShapeControlPoints.TryGetValue(fig.Points[j - 1], out contP);
                        if (sh.MinHeight == 5)
                        {
                            dots += "C";
                            dots += " ";
                            dots += contP.Item1.X;
                            dots += " ";
                            dots += contP.Item1.Y;
                            dots += " ";
                            dots += contP.Item2.X;
                            dots += " ";
                            dots += contP.Item2.Y;
                        }
                        else if(sh.MinHeight == 10)
                        {
                            dots += "A";
                            dots += " ";
                            dots += contP.Item1.X;
                            dots += " ";
                            dots += contP.Item1.Y;
                        }
                    }
                    else
                        dots += "L";
                    dots += " ";
                    dots += fig.Points[j].X;
                    dots += " ";
                    dots += fig.Points[j].Y;
                    dots += " ";
                }
                dots += "!";
            }
            return dots;
        }

        public string SaveGroups(List<Figure> FigureList)
        {
            string groups = "";
            int[] array = new int[FigureList.Count];
            int groupNum = 1;
            for (int i = 0; i < FigureList.Count; i++)
            {
                if (array[i] == 0 && FigureList[i].Points.Count > 0)
                {
                    foreach (Figure fig in FigureList[i].groupFigures)
                    {
                        array[FigureList.IndexOf(fig)] = groupNum;
                    }
                    groupNum++;
                }
            }
            for (int i = 0; i < array.Length; i++ )
            {
                if (array[i] != 0)
                {
                    groups += " ";
                    groups += array[i];
                }
            }
            groups += " ";
            groups += "!";
            return groups;
        }

        public void LoadGroups(List<Figure> FigureList, string groups)
        {
            string pattern = @" ";
            String[] elements = Regex.Split(groups, pattern);
            int[] groupsNum = new int[FigureList.Count];
            for (int i = 1; i < elements.Length - 1; i++)
            {
                groupsNum[i - 1] = Int32.Parse(elements[i]);
            }
            for(int i = 0; i < FigureList.Count; i++)
            {
                FigureList[i].groupFigures.Clear();
                int currentNum = groupsNum[i];
                for (int j = 0; j < groupsNum.Length; j++)
                {
                    if(currentNum == groupsNum[j])
                    {
                        FigureList[i].groupFigures.Add(FigureList[j]);
                    }
                }
            }
        }

        public Figure DeepCopyFigure(Figure figureToCopy)
        {
            Figure newFigure = new Figure(MainCanvas);
            {
                for (int i = 0; i < figureToCopy.Points.Count; i++)
                {
                    Point p = figureToCopy.Points[i];
                    if (i != figureToCopy.Points.Count - 1 || figureToCopy.Points.Count == 1)
                    {
                        Shape sh;
                        figureToCopy.DictionaryPointLines.TryGetValue(p, out sh);
                        Tuple<Point, Point> contP;
                        figureToCopy.DictionaryShapeControlPoints.TryGetValue(p, out contP);
                        Shape newSh = DeepCopy(sh);
                        newFigure.AddShape(newSh, p, contP);
                    }
                    newFigure.Points.Add(p);
                }
                newFigure.PointStart = figureToCopy.Points[0];
                newFigure.PointEnd = figureToCopy.Points[figureToCopy.Points.Count - 1];
            }
            return newFigure;
        }

        public Shape DeepCopy(Shape element)
        {
            string shapestring = XamlWriter.Save(element);
            StringReader stringReader = new StringReader(shapestring);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            Shape DeepCopyobject = (Shape)XamlReader.Load(xmlTextReader);
            return DeepCopyobject;
        }

        public Figure LoadingFigure(string newStuff)
        {
            Figure fig = new Figure(MainCanvas);
            string pattern = @" ";
            String[] elements = Regex.Split(newStuff, pattern);
            Point p = new Point(Double.Parse(elements[0]), Double.Parse(elements[1]));
            fig.AddPoint(p, OptionColor.ColorSelection, false, true, OptionDrawLine.SizeWidthAndHeightRectangle);
            int j = 2;
            while (!elements[j].Equals("!"))
            {
                if (elements[j].Equals("L"))
                {
                    p = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
                    fig.AddPoint(p, OptionColor.ColorSelection, false, true, OptionDrawLine.SizeWidthAndHeightRectangle);
                }
                else if (elements[j].Equals("C"))
                {
                    Point firstContPoint = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
                    Point secondContPoint = new Point(Double.Parse(elements[j + 3]), Double.Parse(elements[j + 4]));
                    j += 4;
                    Point p1 = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
                    Shape sh = GeometryHelper.SetBezier(OptionColor.ColorSelection, p, firstContPoint, secondContPoint, p1, MainCanvas);
                    fig.AddShape(sh, p, new Tuple<Point, Point>(firstContPoint, secondContPoint));
                    p = p1;
                    fig.Points.Add(p);
                    fig.PointEnd = fig.Points[fig.Points.Count - 1];
                }
                else if (elements[j].Equals("A"))
                {
                    Point contPoint = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
                    j += 2;
                    Point p1 = new Point(Double.Parse(elements[j + 1]), Double.Parse(elements[j + 2]));
                    Shape sh = GeometryHelper.SetArc(OptionColor.ColorSelection, p, p1, contPoint, MainCanvas);
                    fig.AddShape(sh, p, new Tuple<Point, Point>(contPoint, new Point()));
                    p = p1;
                    fig.Points.Add(p);
                    fig.PointEnd = fig.Points[fig.Points.Count - 1];
                }
                j += 3;
            }
            return fig;
        }

        private void ClearEverything(bool isListEmpty)
        {
            ListFigure.Clear();
            if(!isListEmpty)
                ListFigure.Add(new Figure(MainCanvas));
            MainCanvas.Children.Clear();
            ListPltFigure.Clear();
            OptionRegim.regim = Regim.RegimNull;
            OptionColor.ColorNewBackground = Brushes.LightSkyBlue;
            OptionColor.ColorNewDraw = Brushes.Violet;
            TatamiFigures.Clear();
            IndexFigure = 0;
            Edit_Menu.IsEnabled = false;
            DeletedGroup = new List<Figure>();
            CopyGroup = new List<Figure>();
            FirstGladFigure = -1;
            SecondGladFigure = -1;
            CloseAllTabs();
            SetkaFigure.RemoveFigure(MainCanvas);
            SetToDefault();
            MainCanvas.Cursor = NormalCursor;
            ClearStatusBar();
            panTransform = new TranslateTransform();
            zoomTransform = new ScaleTransform();
            bothTransforms = new TransformGroup();
            bothTransforms.Children.Add(panTransform);
            bothTransforms.Children.Add(zoomTransform);
            MainCanvas.RenderTransform = bothTransforms;
            SetGrid();
            foreach (Line ln in centerLines)
            {
                ln.StrokeThickness = OptionDrawLine.StrokeThickness;
                MainCanvas.Children.Add(ln);
            }
        }        

        public bool ShowAcceptMessage(int choice)
        {
            string sMessageBoxText = "Соединить?";
            string sCaption = "Стежки";
            if(choice == 0)
            {
                sMessageBoxText = "Создать цепочку стежков?";
            }
            if(choice == 1)
            {
                sMessageBoxText = "Создать татами?";
            }
            if (choice == 2)
            {
                sMessageBoxText = "Создать стежки?";
                if(OptionRegim.regim == Regim.RegimTatami)
                {
                    sCaption = "Татами";
                }
                if (OptionRegim.regim == Regim.RegimCepochka)
                {
                    sCaption = "Цепочка стежков";
                }
                if (OptionRegim.regim == Regim.RegimGlad)
                {
                    sCaption = "Гладь";
                }

            }
            MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.OK:
                    return true;

                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}
