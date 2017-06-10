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
        double CurrentAngle = 0;
        double TotalAngle = 0;
        double StartAngle;
        int IndexFigure;
        int SecondGladFigure;
        bool startDrawing = true;
        bool deleteControlLine = false;
        bool isResizeRegim = false;
        bool isRotateRegim = false;
        List<Figure> ListFigure;
        List<Figure> ListPltFigure;

        Figure ChoosingRectangle;
        Figure ControlLine;
        Figure SetkaFigure;
        Shape changedLine;
        Point FarTransformRectangle;
        List<Point> ChosenPts;
        List<Point> CoordinatesOfTransformRectangles = new List<Point>();
        List<Figure> LinesForGlad = new List<Figure>();
        List<Figure> ControlFigures = new List<Figure>();
        List<Figure> TatamiFigures = new List<Figure>();
    }
}