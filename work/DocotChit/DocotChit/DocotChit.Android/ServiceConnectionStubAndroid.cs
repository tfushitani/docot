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

        public ServiceConnectionStubAndroid(Activity activity)
        {
            Console.WriteLine("ÅyDebugÅzÇQ");
            Intent serviceToStart = new Intent(activity, typeof(DocotService));
            activity.BindService(serviceToStart, connection, Bind.AutoCreate);
        }

        public override string Getstr()
        {
            return connection.GetFormattedTimestamp();
        }


        public override void RegisterLatitudeLongtude()
        {
            connection.RegisterLatitudeLongtude();
        }


        public override bool IsUserInfoRegistered()
        {
            return connection.IsUserInfoRegistered();
        }

        public override void SetUserPreferences(string deviceId, string nickname)
        {
            connection.SetUserPreferences(deviceId, nickname);
        }

        public override void RemoveUserPreference()
        {
            connection.RemoveUserPreference();
        }

    }
}