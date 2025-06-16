using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Xml.Serialization;
using Ukol_v1.Models;

namespace Ukol_v1
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<AutoSummary> AutoSummaries { get; set; } = new();

        public ICommand LoadXmlCommand { get; }

        public MainViewModel()
        {
            LoadXmlCommand = new RelayCommand(_ => LoadXmlFile());
        }

        private void LoadXmlFile()
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

                    AutoSummaries.Clear();

                    if (automobilData?.Auta == null || automobilData.Auta.Count == 0)
                    {
                        MessageBox.Show("Soubor je prázdný nebo neobsahuje žádná auta.");
                        return;
                    }

                    var summaries = automobilData.GetSummaries();

                    if (summaries == null || summaries.Count == 0)
                    {
                        MessageBox.Show("Souhrny nelze vytvořit – data jsou neplatná.");
                        return;
                    }

                    foreach (var summary in summaries)
                        AutoSummaries.Add(summary);
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
