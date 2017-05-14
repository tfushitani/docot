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
        public void OnServiceConnected(ComponentName name, IBinder service)
        {
        }

        public string GetFormattedTimestamp()
        {
            return "ÅyÇ†Ç†Ç†Ç†Åz";
        }

        public void OnServiceDisconnected(ComponentName name)
        {
        }
    }
}