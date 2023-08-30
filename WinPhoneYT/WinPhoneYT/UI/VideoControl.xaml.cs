using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WinPhoneYT.UI
{
    public partial class VideoControl : UserControl
    {
        public VideoInfo info;

        public VideoControl()
        {
            InitializeComponent();
        }

        private string TrailingZero(int val)
        {
            return val < 10 ? "0" + val : val.ToString();
        }

        public void SetInfo(VideoInfo info)
        {
            this.info = info;

            id.Text = info.title;
            if (info.description != null)
                desc.Text = info.description.Replace('\n', ' ');
            else
                desc.Text = "";

            if (desc.Text.Length > 64)
                desc.Text = desc.Text.Substring(0, 64);

            int mins = info.lengthSeconds / 60;
            int hours = mins / 60;
            time.Text = TrailingZero(mins) + ":" + TrailingZero(hours);
        }

        public void SetPreview(string url)
        {
            if (AppSettings.Instance.LoadPreviews)
            {
                System.Windows.Media.Imaging.BitmapImage img = new System.Windows.Media.Imaging.BitmapImage(new Uri(url));
                preview.Source = img;
                
            }
            else
            {
                preview.Visibility = Visibility.Collapsed;
            }
        }

        public void AnimateFade(bool fadeIn)
        {
            if (fadeIn)
                appearAnimation.Begin();
            else
                disappearAnimation.Begin();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AnimateFade(true);
        }
    }
}
