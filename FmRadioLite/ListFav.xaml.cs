using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private List<double> listaRadios { get; set; }
        private double frequencia { get; set; }

        public ListFav()
        {
            this.InitializeComponent();  
            frequencia = (double)ApplicationData.Current.LocalSettings.Values["frequencia"];
            listaRadios = (List<double>)ApplicationData.Current.LocalSettings.Values["listaRadios"];
            doLimpa();
            doList();
        }

        private void doList()
        {
            int count = listaRadios.Count();
            if (count > 0)
            {
                foreach (double i in listaRadios)
                {
                    count = 1;
                    doTextBlock(i);
                }
            }
            else
            {
                doTextBlock();
            }
        }

        private void doTextBlock(Double frec)
        {
            TextBlock text = new TextBlock();
            text.Name = "" + frec + "";
            text.Text = "" + frec + "";

            Principal.Children.Add(text);
        }

        private void doTextBlock()
        {
            TextBlock text = new TextBlock();
            text.Name = "Não existem rádios guardadas";
            text.Text = "Não existem rádios guardadas";

            Principal.Children.Add(text);
        }

        private void doLimpa()
        {
            Principal.Children.Clear();
        }

    }
}
