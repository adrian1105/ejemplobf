using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vivaldi.View
{
    /// <summary>
    /// Lógica de interacción para HuellasControl.xaml
    /// </summary>
    public partial class HuellasControl : UserControl
    {
        public static HuellasControl AppSoporte;
        public HuellasControl()
        {
            InitializeComponent();
            AppSoporte = this;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // ... Load this site.
            wbHuellas.Navigated += new NavigatedEventHandler(wbMain_Navigated);
            this.wbHuellas.Navigate("http://huellas.grupoasd.com.co:8080/SeguimientoIcfes/faces/login.xhtml");
        }


        void wb_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string script = "document.body.style.overflow ='hidden'";
            WebBrowser wb = (WebBrowser)sender;
            wbHuellas.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }

        void wbMain_Navigated(object sender, NavigationEventArgs e)
        {
            SetSilent(wbHuellas, true); // make it silent
        }


        public static void SetSilent(WebBrowser browser, bool silent)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");

            // get an IWebBrowser2 from the document
            IOleServiceProvider sp = browser.Document as IOleServiceProvider;
            if (sp != null)
            {
                Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }

        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        }
    }
}
