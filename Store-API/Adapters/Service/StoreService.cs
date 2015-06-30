using System;
using System.IO;
using Microsoft.Owin.Hosting;
using Store_API.Adapters.Configuration;
using Store_Core;
using Topshelf;

namespace Store_API.Adapters.Service
{
    public class StoreService : ServiceControl
    {
        private IDisposable _app;
        public bool Start(HostControl hostControl)
        {
            var configuration = StoreServerConfiguration.GetConfiguration();
            var uri = configuration.Address.Uri;
            Globals.HostName = uri.Host + ":" + uri.Port;
            _app = WebApp.Start<StartUp>(configuration.Address.Uri.AbsoluteUri);
            return true;
        }


        public bool Stop(HostControl hostControl)
        {
            _app.Dispose();
            return true;
        }

        public void Shutdown(HostControl hostcontrol)
        {
            return;
        }
    }
}
