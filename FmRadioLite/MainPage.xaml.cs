using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Radios;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FmRadioLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private String listaRadios { get; set; }
        private double frequencia { get; set; }
        private String nome { get; set; }
        private String musica { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            frequencia = (double)ApplicationData.Current.LocalSettings.Values["frequencia"];
            listaRadios = (String)ApplicationData.Current.LocalSettings.Values["listaRadios"];
            nome = (String)ApplicationData.Current.LocalSettings.Values["nome"];
            musica = (String)ApplicationData.Current.LocalSettings.Values["musica"];

            frequencia = 104.3;
            nome = "M80";
            musica = "A tua mãe tem pitelhos no cu";

            doFill();
        }

        private void changeRadioIcon()
        {
            String[] ListTemp = null;
            if (listaRadios=="empty" || listaRadios=="")
            {
                listaRadios = "";
                Favoritos.Content = "&#xE734;";
            }
            else if (!(listaRadios == "empty") || !(listaRadios == ""))
            {
                ListTemp = listaRadios.Split('|');
                if (ListTemp.Contains(""+frequencia+""))
                {
                    Favoritos.Content = "&#xE735;";
                }
                else
                {
                    Favoritos.Content = "&#xE734;";
                }
            }
        }

        private void changeRadioPlayIcon()
        {
            /*
                if(radio.IsOn)
                {
                    Play.Content = "&#xE769;";
                }
                else
                {
                    Play.Content = "&#xE768;";
                }
            */
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ListFav));
        }

        private void Favoritos_Click(object sender, RoutedEventArgs e)
        {
            changeRadioIcon();
            String[] ListTemp;
            if (listaRadios.Equals("empty") || listaRadios.Equals(""))
            {
                listaRadios = "";
                Favoritos.Content = "&#xE734;";
                listaRadios = "" + frequencia + "|";
            }
            else
            {
                ListTemp = listaRadios.Split('|');
                listaRadios = "";
                if (ListTemp.Contains("" + frequencia + ""))
                {
                    foreach (String temp in ListTemp){
                        if (!Template.Equals("" + frequencia + ""))
                        {
                            listaRadios += "" + frequencia + "|"; 
                        }
                    }
                }
                else
                {
                    listaRadios += "" + frequencia + "|";
                    Favoritos.Content = "&#xE734;";
                }
            }
        }

        private void doFill()
        {
            Frequencia.Text = "" + frequencia + "";
            Musica.Text = musica;
            Nome.Text = nome;
        }

        private void Definiçoes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Speaker_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
