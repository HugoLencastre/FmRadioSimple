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
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI;

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
        private Boolean IsSpeakerOn { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            var theme = ApplicationData.Current.LocalSettings.Values["theme"].ToString();

            TitleBar(theme);

            StatusBar(theme);

            var frequencia = readFile("Freq");
            var listaRadios = readFile("lista");
            var lang = readFile("language");

            IsSpeakerOn = false;
            turnSpeakerOFF();

            doFill();
        }

        #region StatusTitleBar and TitleBar

        //Actualy change the color of the title bar in pc, to green

        private void TitleBar(String theme)
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                if (theme.Equals("Light"))
                {
                    var view = ApplicationView.GetForCurrentView();
                    view.TitleBar.BackgroundColor = Colors.White;
                    view.TitleBar.ForegroundColor = Colors.Black;

                    view.TitleBar.ButtonBackgroundColor = Colors.White;
                    view.TitleBar.ButtonForegroundColor = Colors.Black;

                    view.TitleBar.ButtonHoverBackgroundColor = Colors.LightGray;
                    view.TitleBar.ButtonHoverForegroundColor = Colors.Black;

                    view.TitleBar.ButtonPressedBackgroundColor = Colors.LightGray;
                    view.TitleBar.ButtonPressedForegroundColor = Colors.Black;

                    view.TitleBar.ButtonInactiveBackgroundColor = Colors.White;
                    view.TitleBar.ButtonInactiveForegroundColor = Colors.Black;

                    view.TitleBar.InactiveBackgroundColor = Colors.White;
                    view.TitleBar.InactiveForegroundColor = Colors.Black;
                }
                else
                {
                    var view = ApplicationView.GetForCurrentView();
                    view.TitleBar.BackgroundColor = Colors.Black;
                    view.TitleBar.ForegroundColor = Colors.White;

                    view.TitleBar.ButtonBackgroundColor = Colors.Black;
                    view.TitleBar.ButtonForegroundColor = Colors.White;

                    view.TitleBar.ButtonHoverBackgroundColor = Colors.DarkGray;
                    view.TitleBar.ButtonHoverForegroundColor = Colors.White;

                    view.TitleBar.ButtonPressedBackgroundColor = Colors.DarkGray;
                    view.TitleBar.ButtonPressedForegroundColor = Colors.White;

                    view.TitleBar.ButtonInactiveBackgroundColor = Colors.Black;
                    view.TitleBar.ButtonInactiveForegroundColor = Colors.White;

                    view.TitleBar.InactiveBackgroundColor = Colors.Black;
                    view.TitleBar.InactiveForegroundColor = Colors.White;
                }
            }
        }
        //Actualy change the color of the Status bar in smartphone, to grey (i want to put transparent)

        private /*async*/ void StatusBar(String theme)
        {
            // Verifica se é telemóvel
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                if (theme.Equals("Dark"))
                {
                    var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                    //await statusbar.HideAsync(); 
                    // ^ Caso queiram tirar a barra descomentem a 
                    //linha e comentem as 3 seguintes e tirem o comentário de async
                    statusbar.BackgroundColor = Windows.UI.Colors.Black;
                    statusbar.BackgroundOpacity = 1;
                    statusbar.ForegroundColor = Windows.UI.Colors.White;
                }
                else
                {
                    var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                    //await statusbar.HideAsync(); 
                    // ^ Caso queiram tirar a barra descomentem a 
                    //linha e comentem as 3 seguintes e tirem o comentário de async
                    statusbar.BackgroundColor = Windows.UI.Colors.White;
                    statusbar.BackgroundOpacity = 1;
                    statusbar.ForegroundColor = Windows.UI.Colors.Black;
                }
            }
        }

        private async void ShowStatusBar()
        {
            //verifica se é telemóvel
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusbar.ShowAsync();
                //statusbar.BackgroundColor = Windows.UI.Colors.Transparent;
                //statusbar.BackgroundOpacity = 1;
                //statusbar.ForegroundColor = Windows.UI.Colors.Red;
            }
        }
        #endregion

        #region Design Change

        private void doFill()
        {
            Frequencia.Text = "" + frequencia + "";
            Musica.TextWrapping = TextWrapping.Wrap;
        }

        private void changeFavIcon(String sera)
        {
            if (sera.Equals("sim"))
            {
                Favoritos.Tag = "sim";
                Favoritos.Icon = new SymbolIcon(Symbol.SolidStar);
            }            
            else
            {
                Favoritos.Tag = "nao";
                Favoritos.Icon = new SymbolIcon(Symbol.OutlineStar);
            }
        }

        private void changePlayIcon()
        {
            if ((string)Play.Tag == "play")
            {
                Play.Tag = "pause";
                Play.Symbol = Symbol.Pause;
            }
            else if ((string)Play.Tag == "pause")
            {
                Play.Tag = "play";
                Play.Symbol = Symbol.Play;
            }
        }

        private void changeSpeakerIcon()
        {
            if (IsSpeakerOn)
            {
                Speaker.Label = "Speaker Off";
                Speaker.Icon= new SymbolIcon(Symbol.Mute);
            }
            else
            {
                Speaker.Label = "Speaker On";
                Speaker.Icon = new SymbolIcon(Symbol.Volume);
            }
        }

        #endregion

        #region 4 Botões Superiores

        private void Favoritos_Click(object sender, RoutedEventArgs e)
        {
            if (isOnFav(frequencia))
            {
                changeFavIcon("nao");
                removeRadio(frequencia);
            }
            else
            {
                changeFavIcon("sim");
                adicionaRadio(frequencia);
            }
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            writeFile("lista", listaRadios);
            Frame.Navigate(typeof(ListFav));
        }

        private void Speaker_Click(object sender, RoutedEventArgs e)
        {
            changeSpeakerIcon();
            if (IsSpeakerOn)
            {
                IsSpeakerOn = false;
                turnSpeakerOFF();
            }
            else
            {
                IsSpeakerOn = true;
                turnSpeakerON();
            }
        }

        private void Definiçoes_Click(object sender, RoutedEventArgs e)
        {
            writeFile("lista", listaRadios);
            //Frame.Navigate(typeof(Settings));
        }

        #endregion

        #region 3 Botões inferiores

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Random num = new Random();
            frequencia = num.Next(); //Next menor 
            if (isOnFav(frequencia))
            {
                changeFavIcon("sim");
            }
            else
            {
                changeFavIcon("nao");
            }
            doFill();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            changePlayIcon();
            if (Play.Tag.Equals("pause"))
            {
                stopMusic();
            }
            else
            {
                playMusic(frequencia);
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Random num = new Random();
            frequencia = num.Next(); //Next maior
            if (isOnFav(frequencia))
            {
                changeFavIcon("sim");
            }
            else
            {
                changeFavIcon("nao");
            }
            doFill();
        }

        #endregion

        #region Funções Auxiliares

        private Boolean isOnFav(double freq)
        {
            Boolean result = new Boolean();
            String[] lista;
            if (listaRadios.Contains("|"))
            {
                lista = listaRadios.Split('|');
                if (lista.Contains("" + freq))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        private void adicionaRadio(double frequencia)
        {
            listaRadios += "" + frequencia + "|";
            writeFile("lista",listaRadios);
        }

        private void removeRadio(double frequencia)
        {
            String[] lista;
            String tempList = "";
            if (listaRadios.Contains("|"))
            {
                lista = listaRadios.Split('|');

                foreach (String fre in lista)
                {
                    if (!(fre == "" + frequencia + ""))
                    {
                        tempList += "" + fre + "|";
                    }
                }
                listaRadios = tempList;
                writeFile("lista", listaRadios);
            }
            else
            {

            }
        }

        private void turnSpeakerOFF()
        {
            // not implemented
        }

        private void turnSpeakerON()
        {
            // not implemented 
        }

        private void stopMusic()
        {
            throw new NotImplementedException();
        }

        private void playMusic(double frequencia)
        {
            throw new NotImplementedException();
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

        #endregion
    }
}
