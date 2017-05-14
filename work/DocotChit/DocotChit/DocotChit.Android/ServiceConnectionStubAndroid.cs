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
    class ServiceConnectionStubAndroid:ServiceConnectionStub
    {
        DocotServiceConnection connection = new DocotServiceConnection();

        public override string Getstr()
        {
            return connection.GetFormattedTimestamp();


        }
    }
}