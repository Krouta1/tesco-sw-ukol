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
        }

        private void LoadXmlFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "Vyberte XML soubor s auty",
                Filter = "XML files (*.xml)|*.xml"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    XmlSerializer serializer = new(typeof(Automobily));
                    using StreamReader sr = new(openFileDialog.FileName, Encoding.UTF8);
                    Automobily? automobilData = serializer.Deserialize(sr) as Automobily;

                    if (automobilData?.Auta == null || automobilData.Auta.Count == 0)
                    {
                        MessageBox.Show("Soubor je prázdný nebo neobsahuje žádná auta.");
                        AutoDataGrid.ItemsSource = null;
                        return;
                    }

                    List<AutoSummary> summaries = automobilData.GetSummaries();

                    if (summaries == null || summaries.Count == 0)
                    {
                        MessageBox.Show("Souhrny nelze vytvořit – data jsou neplatná.");
                        AutoDataGrid.ItemsSource = null;
                        return;
                    }

                    AutoDataGrid.ItemsSource = summaries;
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show("Chybný formát XML souboru: " + ex.Message);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Chyba při čtení souboru: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Neočekávaná chyba: " + ex.Message);
                }
            }
        }
    }
}