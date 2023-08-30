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

using VK4WP.Data;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace VK4WP
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class DialogPage : Page
    {
        private Data.Conversations.Profile profile;

        private DispatcherTimer timer;
        private bool busyFlag;
        private Data.Messages.Root prevDataset;

        public DialogPage()
        {
            this.InitializeComponent();

            timer = Utils.AttachUpdateTimer(RequestUpdate);
            
        }
        

        private void DefaultErrorHandler(string msg)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MessageDialog dlg = new MessageDialog(msg, "Ошибочка вышла :(");
                dlg.ShowAsync();
            });
        }

        private void UpdateHeader()
        {
            name.Text = profile.first_name + " " + profile.last_name;
        }

        private void UpdateUI(Data.Messages.Root root)
        {
            contentMain.Children.Clear();

            root.response.items.Reverse();
            foreach(Data.Messages.Item item in root.response.items)
            {
                UI.MessageView msgView = new UI.MessageView();
                msgView.SetInfo(item, item.from_id != profile.id);

                contentMain.Children.Add(msgView);
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += (object sender, object e) =>
            {
                scroller.ChangeView(0, scroller.ScrollableHeight, 1.0f);
                timer.Stop();
            };
            timer.Start();
        }

        private void RequestUpdate()
        {
            if (!busyFlag)
            {
                busyFlag = true;

                VKAPI.Instance.RequestMethod<Data.Messages.Root>("messages.getHistory", (Data.Messages.Root root) =>
                {
                    Utils.SubmitOnUiThread(() =>
                    {
                        bool needUpdate = true;

                        if (prevDataset != null)
                        {
                            needUpdate = false;

                            for(int i = 0; i < root.response.items.Count; i++)
                            {
                                if (root.response.items[i].id != prevDataset.response.items[i].id)
                                    needUpdate = true;
                            }
                        }

                        prevDataset = root;

                        if (needUpdate)
                            UpdateUI(root);

                        busyFlag = false;
                    });
                }, DefaultErrorHandler, "&user_id={0}&count=200", profile.id);
            }
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            profile = (Data.Conversations.Profile)e.Parameter;

            UpdateHeader();
            RequestUpdate();

            timer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            timer.Stop();
        }

        private void OnScrollerChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }

        private void OnSendMessage(object sender, RoutedEventArgs e)
        {
            string msg = Uri.EscapeUriString(message.Text);
            message.Text = "";

            VKAPI.Instance.PureRequest("messages.send", (string response) =>
            {
                RequestUpdate();
            }, DefaultErrorHandler, "&user_id={0}&random_id={1}&message={2}", profile.id, new Random().Next(), msg);
        }
    }
}
