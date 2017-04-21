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
        public void CloseAllTabs()
        {
            expander1.IsExpanded = false;
            if (tabControl1.Visibility == Visibility.Visible)
                tabControl1.Visibility = Visibility.Hidden;
            if (tabControl2.Visibility == Visibility.Visible)
                tabControl2.Visibility = Visibility.Hidden;
        }

        public void SetToDefault()
        {
            OptionSetka.MasshtabSetka = 0;
            OptionSetka.isDrawSetka = false;
            OptionSetka.isDotOnGrid = false;
            OptionSetka.Masshtab = 1;
            OptionSetka.Angle = 0;
            ScaleTransform scaleTransform = new ScaleTransform(OptionSetka.Masshtab, OptionSetka.Masshtab);
            MainCanvas.LayoutTransform = scaleTransform;
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
