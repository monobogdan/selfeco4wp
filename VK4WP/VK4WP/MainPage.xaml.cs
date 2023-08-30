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

namespace VK4WP
{
    public sealed partial class MainPage : Page
    {

        private void DefaultErrorHandler(string msg)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MessageDialog dlg = new MessageDialog(msg, "Ошибочка вышла :(");
                dlg.ShowAsync();
            }); 
        }

        private void InitializeServices()
        {
            Data.LocalMusicManager.Initialize();
            //Data.LongpollManager longpoll = new Data.LongpollManager();
            //longpoll.Run();
        }

        private DispatcherTimer updateTimer;
        private bool busyState;
        private Data.Conversations.Root prevDataset;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            InitializeServices();

            header.SetTitle("Диалоги");
            header.AttachMenuToDrawer(drawer);

            updateTimer = Utils.AttachUpdateTimer(RequestUpdate);
            RequestUpdate();
        }

        private void UpdateUI(Data.Conversations.Root root)
        {
            contentMain.Children.Clear();

            int num = 0;
            foreach (Data.Conversations.Item item in root.response.items)
            {
                if(item.conversation.peer.type == "user")
                {
                    UI.DialogView dialog = new UI.DialogView();
                    dialog.SetInfo(root, item, num, false);

                    contentMain.Children.Add(dialog);
                    num++;
                }
            }

            if (root.response.unread_count > 0)
                header.SetTitle(string.Format("Диалоги ({0} непрочитанных)", root.response.unread_count));
        }

        public void RequestUpdate()
        {
            if (!busyState)
            {
                header.SetLoadingState(true);
                busyState = true;

                VKAPI.Instance.RequestMethod<Data.Conversations.Root>("messages.getConversations", (Data.Conversations.Root root) =>
                {
                    Utils.SubmitOnUiThread(() =>
                    {
                        header.SetLoadingState(false);

                        bool needUpdate = true;

                        if (prevDataset != null)
                        {
                            needUpdate = false;

                            for (int i = 0; i < root.response.items.Count; i++)
                            {
                                if (root.response.items[i].last_message.id != prevDataset.response.items[i].last_message.id)
                                    needUpdate = true;
                            }
                        }

                        prevDataset = root;

                        if (needUpdate)
                            UpdateUI(root);

                        busyState = false;
                    });
                }, DefaultErrorHandler, "&extended=1&count=50");
            }
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            updateTimer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            updateTimer.Stop();
        }
    }
}
