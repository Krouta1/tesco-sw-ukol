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

        public class FileDialogService : IFileDialogService
        {
            public string? OpenFile(string filter, string title)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = filter,
                    Title = title
                };

                return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
            }
        }

        public class MessageBoxService : IMessageBoxService
        {
            public void Show(string message)
            {
                MessageBox.Show(message);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Přidejte reálné implementace služeb
            var fileDialogService = new FileDialogService();
            var messageBoxService = new MessageBoxService();

            this.DataContext = new MainViewModel(fileDialogService, messageBoxService);
        }
    }
}