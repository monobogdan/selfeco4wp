using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace VK4WP
{
    public static class Utils
    {

        public static void ShowError(string msg)
        {
            SubmitOnUiThread(() =>
            {
                MessageDialog dlg = new MessageDialog(msg, "Ошибочка вышла :(");
                dlg.ShowAsync();
            });
        }

        public static async void SubmitOnUiThread(Action act)
        {
            await App.Instance.RootFrame.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(act));
        }

        public static Windows.UI.Xaml.DispatcherTimer AttachUpdateTimer(Action act)
        {
            Windows.UI.Xaml.DispatcherTimer timer = new Windows.UI.Xaml.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (object sender, object e) => { act(); };

            return timer;
        }
    }
}
