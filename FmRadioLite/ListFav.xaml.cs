using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FmRadioLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListFav : Page
    {
        private String listaRadios { get; set; }
        private double frequencia { get; set; }
        private String lang { get; set; }

        public ListFav()
        {
            this.InitializeComponent();  
            var frequencia = readFile("Freq");
            var listaRadios = readFile("lista");
            var lang = readFile("language");
            doLimpa();
            doList();
        }

        private void doList()
        {
            doTitle(lang);
            if (listaRadios.Equals(""))
            {
            }
            else
            {
                ordena();
                doTextBlock();
            }
        }

        private void ordena()
        {
            String result = "";
            String[] listatemp;
            listatemp = listaRadios.Split('|');
            Array.Sort<String>(listatemp);
            foreach (String freq in listatemp)
            {
                result += freq + "|";
            }
            listaRadios = result;
        }

        private void doTitle(String lang)
        {
            String texto = "";
            if (lang.Equals("en") || lang.Equals(""))
            {
                texto = "Favourite List: \n";
            }
            else if (lang.Equals("pt"))
            {
                texto = "Lista de Favoritos: \n";
            }
            TextBlock text = new TextBlock();
            text.Name = "Lista";
            text.Text = texto;

            Principal.Children.Add(text);
        }

        private String[] transformStringToArray(String radios)
        {
            String[] lista;
            lista = radios.Split('|');
            return lista; 
        }

        private void doTextBlock(String freq)
        {
            TextBlock text = new TextBlock();
            text.Name = freq;
            text.Text = freq;

            Principal.Children.Add(text);
        }

        private void doTextBlock()
        {
            String[] Lista;
            Lista = listaRadios.Split('|');
            foreach (String fre in Lista)
            {
                doTextBlock(fre);
            }
        }

        private void doLimpa()
        {
            Principal.Children.Clear();
        }

        private async System.Threading.Tasks.Task<String> readFile(String name)
        {
            Windows.Storage.StorageFolder storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sample = await storageFolder.GetFileAsync(name + ".txt");

            string text = await Windows.Storage.FileIO.ReadTextAsync(sample);

            return text;
        }

        private async void writeFile(String name, String text)
        {
            Windows.Storage.StorageFolder storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sample = await storageFolder.GetFileAsync(name + ".txt");

            await Windows.Storage.FileIO.WriteTextAsync(sample, text);
        }

    }
}
