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
        Cursor handCursor;
        Cursor defaultCursor;
        Cursor swordCursor;
        Cursor arrowCursor;
        Cursor zoomInCursor;
        Cursor zoomOutCursor;
        Cursor oneToOneCursor;
        Cursor centerCursor;
        Cursor prevCursor;
        Mode prevMode;
        double t;
        string pathToFile;
        int indexFigure;
        int tempIndexFigure;
        int firstSatinFigure;
        int secondSatinFigure;
        bool areSatinPointsInversed;
        bool startDrawing = false;
        Rectangle firstRec;
        Rectangle lastRec;
        Rectangle chRec;
        Line lastLine;
        List<Figure> listFigure;
        List<Figure> tempListFigure;
        List<Figure> listPltFigure;
        List<Rectangle> transRectangles;
        List<Rectangle> invisibleRectangles;
        Figure choosingRectangle;
        Figure controlLine;
        Figure gridFigure;
        List<Figure> copyGroup;
        List<Figure> deletedGroup;
        Shape changedLine;
        Point prevPoint;
        List<Point> chosenPts;
        List<Point> coordinatesOfTransformRectangles = new List<Point>();
        List<Figure> tempSatinLines = new List<Figure>();
        List<Figure> linesForSatin = new List<Figure>();
        List<Figure> controlFigures = new List<Figure>();
        List<Figure> tatamiFigures = new List<Figure>();
        List<ChangedShape> listChangedShapes = new List<ChangedShape>();
        List<Line> centerLines = new List<Line>();
        List<Line> unembroidLines = new List<Line>();
        List<PreviousView> previousViewList = new List<PreviousView>();

        Image mainBMP;
        Image transparentBMP;
        Image gridBMP;

        TranslateTransform panTransform;
        ScaleTransform zoomTransform;
        TransformGroup bothTransforms;
    }
}