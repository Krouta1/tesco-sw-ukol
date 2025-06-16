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
    public interface IFileDialogService
    {
        string? OpenFile(string filter, string title);
    }

    public interface IMessageBoxService
    {
        void Show(string message);
    }

    public class MainViewModel : ViewModelBase
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IMessageBoxService _messageBoxService;

        public ObservableCollection<AutoSummary> AutoSummaries { get; } = new();
        public ICommand LoadXmlCommand { get; }

        public MainViewModel(IFileDialogService fileDialogService, IMessageBoxService messageBoxService)
        {
            _fileDialogService = fileDialogService;
            _messageBoxService = messageBoxService;

            LoadXmlCommand = new RelayCommand(_ => LoadXmlFile());
        }

        public void LoadXmlFile()
        {
            string? fileName = _fileDialogService.OpenFile("XML files (*.xml)|*.xml", "Vyberte XML soubor s auty");

            if (string.IsNullOrEmpty(fileName))
                return;

            try
            {
                XmlSerializer serializer = new(typeof(Automobily));
                using StreamReader sr = new(fileName, Encoding.UTF8);
                Automobily? automobilData = serializer.Deserialize(sr) as Automobily;

                AutoSummaries.Clear();

                if (automobilData?.Auta == null || automobilData.Auta.Count == 0)
                {
                    _messageBoxService.Show("Soubor je prázdný nebo neobsahuje žádná auta.");
                    return;
                }

                var summaries = automobilData.GetSummaries();

                if (summaries == null || summaries.Count == 0)
                {
                    _messageBoxService.Show("Souhrny nelze vytvořit – data jsou neplatná.");
                    return;
                }

                foreach (var summary in summaries)
                    AutoSummaries.Add(summary);
            }
            catch (InvalidOperationException ex)
            {
                _messageBoxService.Show("Chybný formát XML souboru: " + ex.Message);
            }
            catch (IOException ex)
            {
                _messageBoxService.Show("Chyba při čtení souboru: " + ex.Message);
            }
            catch (Exception ex)
            {
                _messageBoxService.Show("Neočekávaná chyba: " + ex.Message);
            }
        }
    }

}
