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
using System.Windows.Shapes;

namespace ТриНитиДизайн.View
{
    /// <summary>
    /// Логика взаимодействия для JoinCursor.xaml
    /// </summary>
    public partial class JoinCursor : Window
    {
        public JoinCursor()
        {
            InitializeComponent();
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void chain_button_Click(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimCursorJoinChain;
            this.Close();
        }

        private void transposition_button_Click(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimCursorJoinTransposition;
            this.Close();
        }

        private void shift_dots_button_Click(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimCursorJoinShiftDots;
            this.Close();
        }

        private void shift_elements_button_Click(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimCursorJoinShiftElements;
            this.Close();
        }
    }
}
