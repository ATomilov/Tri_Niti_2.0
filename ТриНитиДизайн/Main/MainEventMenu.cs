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
using System.Drawing;
using Microsoft.Win32;
using Path = System.Windows.Shapes.Path;
using CorelDRAW;

namespace ТриНитиДизайн
{
    public partial class MainWindow
    {
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog op = new SaveFileDialog();
            op.Filter = "Проект Три Нити Дизайн| *.tri";
            op.ShowDialog();
        }


        private void NewProject(object sender, RoutedEventArgs e)
        {
            ListFigure.Clear();
            ListFigure.Add(new Figure(MainCanvas));
            MainCanvas.Children.Clear();
            OptionRegim.regim = Regim.RegimDraw;
            OptionRegim.oldRegim = Regim.RegimFigure;
            TatamiFigures.Clear();
            IndexFigure = 0;
            SecondGladFigure = -1;
            CloseAllTabs();
            SetToDefault();
            MainCanvas.Cursor = NormalCursor;
        }

        private void LoadPLT(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Corel PLT| *.plt";
            op.ShowDialog();
            CorelDRAW.Application cdr = new CorelDRAW.Application();
            cdr.OpenDocument(op.FileName, 1);
            cdr.ActiveDocument.ExportBitmap(
                op.FileName+".png",
                VGCore.cdrFilter.cdrPNG,
                VGCore.cdrExportRange.cdrCurrentPage,
                VGCore.cdrImageType.cdrRGBColorImage,
                0, 0, cdr.ActiveDocument.ResolutionX, cdr.ActiveDocument.ResolutionY,
                VGCore.cdrAntiAliasingType.cdrNoAntiAliasing,
                false,
                true,
                true,
                false,
                VGCore.cdrCompressionType.cdrCompressionNone,
                null).Finish();
            cdr.ActiveDocument.Close();
            cdr.Quit();
            //Image BodyImage = new Image
            //{
            //    Width = moonBitmap.Width,
            //    Height = moonBitmap.Height,
            //    Name = BodyName,
            //    Source = new BitmapImage(new Uri(moonBitmap.UriSource.ToString(), UriKind.Relative)),
            //};
        }

        private void DeleteFigureClick(object sender, RoutedEventArgs e)
        {
            ListFigure[IndexFigure].ClearFigure();
            RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Проект Три Нити Дизайн| *.tri";
            op.ShowDialog();
        }
        private void OpenWindowsColor(object sender, RoutedEventArgs e)
        {
            WindowColors window = new WindowColors();
            window.ShowDialog();
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
    }
}