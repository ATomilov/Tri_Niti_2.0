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
        public MainWindow()
        {
            InitializeComponent();
            copyGroup = new List<Figure>();
            controlLine = new Figure(mainCanvas);
            deletedGroup = new List<Figure>();
            chosenPts = new List<Point>();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string sMessageBoxText = "Сохранить последний проект?";
            string sCaption = "Сообщение";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox);

            if (rsltMessageBox == MessageBoxResult.Yes)
                SaveProject(null, null);
            else if (rsltMessageBox == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listFigure = new List<Figure>();
            listFigure.Add(new Figure(mainCanvas));
            listPltFigure = new List<Figure>();
            listPltFigure.Add(new Figure(mainCanvas));
            indexFigure = 0;
            tabControl1.Visibility = Visibility.Hidden;
            tabControl2.Visibility = Visibility.Hidden;
            /// Курсоры
            System.Windows.Resources.StreamResourceInfo sri = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Hand.cur", UriKind.Relative));
            handCursor = new Cursor(sri.Stream);
            System.Windows.Resources.StreamResourceInfo sri1 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Normal.cur", UriKind.Relative));
            defaultCursor = new Cursor(sri1.Stream);
            System.Windows.Resources.StreamResourceInfo sri2 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Sword.cur", UriKind.Relative));
            swordCursor = new Cursor(sri2.Stream);
            System.Windows.Resources.StreamResourceInfo sri3 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\ArrowInLineMode.cur", UriKind.Relative));
            arrowCursor = new Cursor(sri3.Stream);
            System.Windows.Resources.StreamResourceInfo sri4 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Zoom-in.cur", UriKind.Relative));
            zoomInCursor = new Cursor(sri4.Stream);
            System.Windows.Resources.StreamResourceInfo sri5 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Zoom-out.cur", UriKind.Relative));
            zoomOutCursor = new Cursor(sri5.Stream);
            System.Windows.Resources.StreamResourceInfo sri6 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\onetoone.ico", UriKind.Relative));
            oneToOneCursor = new Cursor(sri6.Stream);
            System.Windows.Resources.StreamResourceInfo sri7 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\centercursor.ico", UriKind.Relative));
            centerCursor = new Cursor(sri7.Stream);

            mainGrid.Cursor = defaultCursor;
            mainCanvas.Cursor = defaultCursor;

            panTransform = new TranslateTransform();
            zoomTransform = new ScaleTransform();
            bothTransforms = new TransformGroup();
            bothTransforms.Children.Add(panTransform);
            bothTransforms.Children.Add(zoomTransform);
            mainCanvas.RenderTransform = bothTransforms;
            this.PreviewKeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }


        private void EditButtonEvent(object sender, RoutedEventArgs e)
        {
            //if (OptionMode.mode == Mode.modeCursor)
            //{
            //    indexFigure = listFigure.IndexOf(listFigure[indexFigure].groupFigures[0]);
            //}
            //ExitFromRisuimode();
            Edit_Menu.IsEnabled = false;
            CloseAllTabs();
            //listFigure[indexFigure].highlightedPoints.Clear();
            OptionMode.mode = Mode.modeDraw;
            RedrawScreen(listFigure, indexFigure, mainCanvas);
            //ShowPositionStatus(listFigure[indexFigure], false, false);
            //DrawFirstAndLastRectangle();
            //ChangeFiguresColor(listFigure, mainCanvas);
            mainCanvas.Cursor = handCursor;
        }

        private void CurcorButtonEvent(object sender, RoutedEventArgs e)
        {
            //if(deletedGroup.Count > 0)
            //    restore_button.IsEnabled = true;
            //else
            //    restore_button.IsEnabled = false;
            //ExitFromRisuimode();
            //Edit_Menu.IsEnabled = true;
            //CloseAllTabs();
            //listFigure[indexFigure].highlightedPoints.Clear();
            //OptionMode.mode = Mode.modeCursor;
            //for (int i = 0; i < listFigure.Count; i++)
            //{
            //    if (listFigure[i].preparedForTatami)
            //    {
            //        listFigure[i].LoadCurrentShapes();
            //    }
            //}
            //ShowPositionStatus(listFigure[indexFigure], true, false);
            //RedrawEverything(listFigure, indexFigure, false, mainCanvas);
            //ChangeFiguresColor(listFigure, mainCanvas);
            //DrawOutsideRectangles(true, false, mainCanvas);
            //mainCanvas.Cursor = defaultCursor;
        }
    }
}
