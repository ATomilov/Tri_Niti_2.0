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

        public MainWindow()
        {
            InitializeComponent();
            CopyGroup = new List<Figure>();
            ControlLine = new Figure(MainCanvas);
            DeletedGroup = new List<Figure>();
            ChosenPts = new List<Point>();
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
            ListFigure = new List<Figure>();
            ListFigure.Add(new Figure(MainCanvas));
            ListPltFigure = new List<Figure>();
            ListPltFigure.Add(new Figure(MainCanvas));
            IndexFigure = 0;
            tabControl1.Visibility = Visibility.Hidden;
            tabControl2.Visibility = Visibility.Hidden;
            /// Курсоры
            System.Windows.Resources.StreamResourceInfo sri = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Hand.cur", UriKind.Relative));
            HandCursor = new Cursor(sri.Stream);
            System.Windows.Resources.StreamResourceInfo sri1 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Normal.cur", UriKind.Relative));
            NormalCursor = new Cursor(sri1.Stream);
            System.Windows.Resources.StreamResourceInfo sri2 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Sword.cur", UriKind.Relative));
            SwordCursor = new Cursor(sri2.Stream);
            System.Windows.Resources.StreamResourceInfo sri3 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\ArrowInLineRegim.cur", UriKind.Relative));
            ArrowCursor = new Cursor(sri3.Stream);
            System.Windows.Resources.StreamResourceInfo sri4 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Zoom-in.cur", UriKind.Relative));
            ZoomInCursor = new Cursor(sri4.Stream);
            System.Windows.Resources.StreamResourceInfo sri5 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Zoom-out.cur", UriKind.Relative));
            ZoomOutCursor = new Cursor(sri5.Stream);

            mainGrid.Cursor = NormalCursor;
            MainCanvas.Cursor = NormalCursor;


            //ScaleTransform scaleTransform = new ScaleTransform(2, 2);
            //scaleTransform.CenterX = MainCanvas.Width / 2;
            //scaleTransform.CenterY = MainCanvas.Height / 2;
            //MainCanvas.RenderTransform = scaleTransform;
        }


        private void EditButtonEvent(object sender, RoutedEventArgs e)
        {
            if (OptionRegim.regim == Regim.RegimCursor)
            {
                IndexFigure = ListFigure.IndexOf(ListFigure[IndexFigure].groupFigures[0]);
            }
            ExitFromRisuiRegim();
            Edit_Menu.IsEnabled = false;
            CloseAllTabs();
            ListFigure[IndexFigure].PointsCount.Clear();
            OptionRegim.regim = Regim.RegimDraw;
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            ShowPositionStatus(ListFigure[IndexFigure], false, false);
            DrawFirstAndLastRectangle();
            ChangeFiguresColor(ListFigure, MainCanvas);
            MainCanvas.Cursor = HandCursor;
            ShowPositionStatus(ListFigure[IndexFigure], true, false);
        }

        private void CurcorButtonEvent(object sender, RoutedEventArgs e)
        {
            if(DeletedGroup.Count > 0)
                restore_button.IsEnabled = true;
            else
                restore_button.IsEnabled = false;
            ExitFromRisuiRegim();
            Edit_Menu.IsEnabled = true;
            CloseAllTabs();
            ListFigure[IndexFigure].PointsCount.Clear();
            OptionRegim.regim = Regim.RegimCursor;
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            ChangeFiguresColor(ListFigure, MainCanvas);
            DrawOutsideRectangles(true, false, MainCanvas);
            MainCanvas.Cursor = NormalCursor;
        }
    }
}
