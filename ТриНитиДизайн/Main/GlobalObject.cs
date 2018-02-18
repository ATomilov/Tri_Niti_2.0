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

namespace ТриНитиДизайн
{
    public partial class MainWindow : Window
    {
        //Курсоры
        Cursor HandCursor;
        Cursor NormalCursor;
        Cursor SwordCursor;
        Cursor ArrowCursor;
        Cursor ZoomInCursor;
        Cursor ZoomOutCursor;
        Cursor OneToOneCursor;
        Cursor CenterCursor;
        Cursor prevCursor;
        Regim prevRegim;
        double t;
        string pathToFile;
        int IndexFigure;
        int TempIndexFigure;
        int FirstGladFigure;
        int SecondGladFigure;
        bool areGladPointsInversed;
        bool startDrawing = true;
        Rectangle firstRec;
        Rectangle lastRec;
        Rectangle chRec;
        Line lastLine;
        List<Figure> ListFigure;
        List<Figure> TempListFigure;
        List<Figure> ListPltFigure;
        List<Rectangle> transRectangles;
        List<Rectangle> invisibleRectangles;
        Figure ChoosingRectangle;
        Figure ControlLine;
        Figure SetkaFigure;
        List<Figure> CopyGroup;
        List<Figure> DeletedGroup;
        Shape changedLine;
        Point prevPoint;
        List<Point> ChosenPts;
        List<Point> CoordinatesOfTransformRectangles = new List<Point>();
        List<Figure> TempLinesForGlad = new List<Figure>();
        List<Figure> LinesForGlad = new List<Figure>();
        List<Figure> ControlFigures = new List<Figure>();
        List<Figure> TatamiFigures = new List<Figure>();
        List<ChangedShape> listChangedShapes = new List<ChangedShape>();
        List<Line> centerLines = new List<Line>();
        List<Line> otshitLines = new List<Line>();
        List<PreviousView> PreviousViewList = new List<PreviousView>();

        TranslateTransform panTransform;
        ScaleTransform zoomTransform;
        TransformGroup bothTransforms;
    }
}