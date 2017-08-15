using System;
using System.Windows.Forms;

namespace ShadowSenseDemo.Helpers
{
    public static class Execute
    {
        private static Action<System.Action> executor = action => action();

        /// <summary>
        /// Initializes the framework using the current dispatcher.
        /// </summary>
        public static void InitializeWithDispatcher()
        {
            var dispatcher = System.Windows.Application.Current.Dispatcher;
            executor = action => {
                if (dispatcher.CheckAccess())
                    action();
                else dispatcher.BeginInvoke(action);
            };
        }

        /// <summary>
        /// Executes the action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void OnUIThread(this System.Action action)
        {
            executor(action);
        }
    }
    public class WindowsDialogs : IWindowsDialogs
    {
        public string[] ShowOpenFileDialog(string caption, string filter = null, string initialDirectory = null)
        {
            string[] filename = null;
            Execute.OnUIThread(() =>
            {
                var ofd = new Microsoft.Win32.OpenFileDialog
                {
                    Title = caption,
                    CheckFileExists = true,
                    RestoreDirectory = true,
                    InitialDirectory = initialDirectory,
                    Multiselect = true,
                    Filter = filter
                };

                if (ofd.ShowDialog() == true)
                    filename = ofd.FileNames;
            });
            return filename;
        }
    }
}
