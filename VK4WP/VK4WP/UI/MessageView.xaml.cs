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
    public sealed partial class MessageView : UserControl
    {
        private Data.Messages.Item item;

        public MessageView()
        {
            this.InitializeComponent();
        }

        public void SetInfo(Data.Messages.Item item, bool isMy)
        {
            this.item = item;

            text.Text = item.text;

            if (isMy)
                ((SolidColorBrush)panel.Background).Color = Windows.UI.Color.FromArgb(255, 45, 137, 239);

            if (text.Text.Length < 0)
                item.text = "< Вложение > (пока не поддерживается)";
        }

        private void OnCopyMessage(object sender, TappedRoutedEventArgs e)
        {
            // No API for clipboard :(
        }
    }
}
