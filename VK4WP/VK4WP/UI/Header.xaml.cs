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

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236

namespace VK4WP.UI
{
    public sealed partial class Header : UserControl
    {
        public Header()
        {
            this.InitializeComponent();

            SetTitle("Temporary");
        }

        public void SetTitle(string title)
        {
            fadeOut.Begin();

            EventHandler<object> deleg = null;
            fadeOut.Completed += deleg = (object sender, object e) =>
            {
                headerTitle.Text = title;
                fadeIn.Begin();

                fadeOut.Completed -= deleg;
            };
        }

        public void SetLoadingState(bool enabled)
        {
            progress.IsActive = enabled;
        }

        public void AttachMenuToDrawer(NavigationDrawer drawer)
        {
            menuButton.Tapped += (object sender, TappedRoutedEventArgs args) =>
            {
                drawer.OpenDrawer();
            };
        }
    }
}
