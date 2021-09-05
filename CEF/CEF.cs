using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace UnityControls
{
    class CEF
    {
        private readonly ChromiumWebBrowser _chromeBrowser;

        public CEF()
        {
            CefSettings settings = new CefSettings();
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            Cef.Initialize(settings);

            _chromeBrowser = new ChromiumWebBrowser("http://localhost:6006");
        }

        public void Embed(Control page)
        {
            page.Controls.Add(_chromeBrowser);
            _chromeBrowser.Dock = DockStyle.Fill;
        }

        public void Load(string url)
        {
            _chromeBrowser.Load(url);
        }

        public static void Dispose()
        {
            Cef.Shutdown();
        }
    }
}
