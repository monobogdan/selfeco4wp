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
    public sealed partial class DialogView : UserControl
    {
        public Data.Conversations.Item Item
        {
            get;
            set;
        }

        private Data.Conversations.Profile profile;

        public DialogView()
        {
            this.InitializeComponent();
        }

        private Data.Conversations.Profile GetProfile(Data.Conversations.Root root, Data.Conversations.Item item)
        {
            bool isUser = item.conversation.peer.id > 0;

            if (isUser)
            {
                foreach (Data.Conversations.Profile profile in root.response.profiles)
                {
                    if (profile.id == item.conversation.peer.id)
                        return profile;
                }

                return null;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private string GetName(Data.Conversations.Root root, Data.Conversations.Item item)
        {
            bool isUser = item.conversation.peer.id > 0;

            if(isUser)
            {
                foreach(Data.Conversations.Profile profile in root.response.profiles)
                {
                    if (profile.id == item.conversation.peer.id)
                        return profile.first_name + " " + profile.last_name;
                }

                return "Not found";
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private string GetAvatar(Data.Conversations.Root root, Data.Conversations.Item item)
        {
            bool isUser = item.conversation.peer.id > 0;

            if (isUser)
            {
                foreach (Data.Conversations.Profile profile in root.response.profiles)
                {
                    if (profile.id == item.conversation.peer.id)
                        return profile.photo_100;
                }

                return null;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void SetInfo(Data.Conversations.Root root, Data.Conversations.Item item, int posInList, bool showAnimation)
        {
            Item = item;

            profile = GetProfile(root, item);

            Windows.UI.Xaml.Media.Imaging.BitmapImage image = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            image.UriSource = new Uri(GetAvatar(root, item));

            avatar.Source = image;
            name.Text = GetName(root, item);
            
            lastMessage.Text = item.last_message.text;
            if (lastMessage.Text.Length == 0)
                lastMessage.Text = "< Вложение >";

            if (showAnimation)
            {
                TimeSpan time = sbShow.Children[0].Duration.TimeSpan + TimeSpan.FromMilliseconds(posInList * 300);
                sbShow.Children[0].Duration = time;
                sbShow.Begin();
            }
        }
        

        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(DialogPage), profile);
        }
    }
}
