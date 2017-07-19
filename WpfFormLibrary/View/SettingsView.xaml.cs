using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfFormLibrary.View
{

    public class DataObject
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
    }


    /// <summary>
    /// Interaction logic for StatusView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public SettingsView()
        {

            InitializeComponent();

        }

        private void CaptureConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate the connection name?
            // Somehow invoke the method on the viewmodel
        }

    }
    
}
