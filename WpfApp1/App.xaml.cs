using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;
namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            Trace.TraceError("UI App crash On : " + DateTime.Now.ToString());
            Trace.WriteLine(e.Exception.StackTrace);
            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
