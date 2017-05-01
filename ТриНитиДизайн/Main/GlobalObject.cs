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
        double MousePositionX;
        double MousePositionY;
        int IndexFigure;
        int SecondGladFigure;
        bool startDrawing = true;
        List<Figure> ListFigure;

        Figure ChoosingRectangle;
        Figure ControlLine;
        Figure SetkaFigure;
        Shape changedLine;
        List<Point> ChosenPts;
        List<Figure> LinesForGlad = new List<Figure>();
        List<Figure> ControlFigures = new List<Figure>();
        List<Figure> TatamiFigures = new List<Figure>();
    }
}