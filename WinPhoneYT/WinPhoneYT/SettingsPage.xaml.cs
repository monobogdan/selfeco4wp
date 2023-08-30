using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Storage;

namespace WinPhoneYT
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        RadioButton[] resRadios;

        public SettingsPage()
        {
            InitializeComponent();

            resRadios = new RadioButton[] { radioRes0, radioRes1, radioRes2, radioRes3, radioRes4, radioRes5 };
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            cbHistory.IsChecked = AppSettings.Instance.SaveHistory;
            cbPreview.IsChecked = AppSettings.Instance.LoadPreviews;

            resRadios[AppSettings.Instance.DesiredQuality].IsChecked = true;
        }

        private void OnCheckHistory(object sender, RoutedEventArgs e)
        {
            AppSettings.Instance.SaveHistory = (bool)cbHistory.IsChecked;
        }

        private void OnCheckPreview(object sender, RoutedEventArgs e)
        {
            AppSettings.Instance.LoadPreviews = (bool)cbPreview.IsChecked;
        }

        private void OnExited(object sender, RoutedEventArgs e)
        {
            AppSettings.Save();
        }

        private void OnPickResolution(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < resRadios.Length; i++)
            {
                if (resRadios[i] == sender)
                    AppSettings.Instance.DesiredQuality = i;
            }
        }
    }
}