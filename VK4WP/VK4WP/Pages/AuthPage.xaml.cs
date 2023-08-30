using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace VK4WP.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            this.InitializeComponent();

            appVer.Text = string.Format("Версия приложения: {0}", VersionInfo.Version);
            progress.Visibility = Visibility.Collapsed;
        }

        private void NavigateToDialogs()
        {
            Utils.SubmitOnUiThread(() =>
            {
                ((Frame)Window.Current.Content).Navigate(typeof(MainPage), null);
            });
            
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Data.Config.Instance.IsAuthorized())
                NavigateToDialogs();
            
        }

        private void OnBeginAuth(object sender, RoutedEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            authButton.IsEnabled = false;

            Data.VKAPI.Instance.DirectAuth(name.Text, password.Text, (bool isOK, string tokenOrErrorMessage, string uid) =>
            {
                if(isOK)
                {
                    Data.Config.Instance.SetParameter<string>("usertoken", tokenOrErrorMessage);

                    NavigateToDialogs();
                }
                else
                {
                    Utils.SubmitOnUiThread(() =>
                    {
                        MessageDialog dlg = new MessageDialog("При авторизации произошла ошибка: " + tokenOrErrorMessage);
                        dlg.ShowAsync();

                        authButton.IsEnabled = true;
                    });
                    
                }

                
            });
        }
    }
}
