using System;
using System.Windows.Threading;

namespace CSharpCodeGenerator.Models
{
    internal static class Updates
    {
        public static void WaitFor(TimeSpan time, DispatcherPriority priority)
        {
            DispatcherTimer timer = new DispatcherTimer(priority);
            timer.Tick += new EventHandler(OnDispatched);
            timer.Interval = time;
            DispatcherFrame dispatcherFrame = new DispatcherFrame(false);
            timer.Tag = dispatcherFrame;
            timer.Start();
            Dispatcher.PushFrame(dispatcherFrame);
        }
        public static void OnDispatched(object sender, EventArgs args)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Tick -= new EventHandler(OnDispatched);
            timer.Stop();
            DispatcherFrame frame = (DispatcherFrame)timer.Tag;
            frame.Continue = false;
        }
    }
}
