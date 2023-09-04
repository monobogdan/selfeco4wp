using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WinPhoneYT
{
    public static class AnimationGenerator
    {

        public static void SmoothSwipeX(System.Windows.UIElement element, float xFrom, float xTo)
        {
            ExponentialEase ease = new ExponentialEase();
            ease.Exponent = 4;

            DoubleAnimation anim0 = new DoubleAnimation();
            anim0.EasingFunction = ease;
            anim0.To = xTo;
            anim0.From = xFrom;
            anim0.Duration = new Duration(TimeSpan.FromSeconds(4));

            TranslateTransform tt = new TranslateTransform();
            tt.X = xFrom;
            tt.Y = 0;
            element.RenderTransform = tt;

            Storyboard.SetTarget(anim0, tt);
            Storyboard.SetTargetProperty(anim0, new PropertyPath("(X)"));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(anim0);

            storyboard.Begin();
            
        }
    }
}
