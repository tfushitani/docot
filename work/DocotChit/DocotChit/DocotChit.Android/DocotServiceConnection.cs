using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DocotChit.Droid
{
    class DocotServiceConnection : Java.Lang.Object, IServiceConnection
    {

        public DocotServiceBinder Binder { get; private set; }

        public bool IsConnected { get; private set; }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            Binder = service as DocotServiceBinder;

            Console.WriteLine("ÅyDebugÅz" + name.ClassName);

            IsConnected = this.Binder != null;
        }

        public string GetFormattedTimestamp()
        {
            Console.WriteLine("ÅyDebugÅzÇP");

            if (Binder == null)
            {
                return "";
            }
            else
            { 
                return Binder.GetFormattedTimestamp();
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
        }
    }
}