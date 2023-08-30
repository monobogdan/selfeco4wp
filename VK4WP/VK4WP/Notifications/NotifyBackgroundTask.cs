using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK4WP.Notifications
{
    public static class NotificationUtils
    {
        public static void ShowMessageNotify(string author, string msg)
        {
            var xmlDoc = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastText02);
            Log.WriteLine(xmlDoc.GetXml());
            xmlDoc.GetElementsByTagName("text")[0].InnerText = "Новое сообщение!";
            xmlDoc.GetElementsByTagName("text")[1].InnerText = msg;

            Windows.UI.Notifications.ToastNotification notification = new Windows.UI.Notifications.ToastNotification(xmlDoc);
            Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(notification);
        }
    }
}
