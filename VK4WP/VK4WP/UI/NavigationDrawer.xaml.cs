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
using Windows.UI.Xaml.Media.Animation;

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236

namespace VK4WP.UI
{
    public sealed partial class NavigationDrawer : UserControl
    {
        struct GestureState
        {
            public float InitialX, InitialY;
        }

        private bool isOpen;
        private GestureState swipeGestureState;

        public NavigationDrawer()
        {
            this.InitializeComponent();

            isOpen = false;
            swipeGestureState = new GestureState();
        }

        public void SetTitle(string title)
        {

        }

        public void OpenDrawer()
        {
            if (!isOpen)
                sbOpen.Begin();
            else
                sbClose.Begin();

            isOpen = !isOpen;
        }

        public bool IsOpen()
        {
            return isOpen;
        }

        public void AttachGestureDetector(Page page)
        {
            page.PointerPressed += (object sender, PointerRoutedEventArgs e) =>
            {
                var point = e.GetCurrentPoint(null);
                swipeGestureState.InitialX = (float)point.Position.X;
                swipeGestureState.InitialY = (float)point.Position.Y;
            };

            page.PointerReleased += (object sender, PointerRoutedEventArgs e) =>
            {
                var point = e.GetCurrentPoint(null);

                Log.WriteLine("released");
                if (e.GetCurrentPoint(null).Position.X - swipeGestureState.InitialX > 64)
                    OpenDrawer();
            };
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnTapNews(object sender, TappedRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Hi!");
        }

        private void OnNavigateMusic(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MusicPage), null);
            OpenDrawer();
        }

        private void OnTapMessenger(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MainPage), null);
            OpenDrawer();
        }
    }
}
