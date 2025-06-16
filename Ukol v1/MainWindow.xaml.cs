using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using Ukol_v1.Models;

namespace Ukol_v1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}