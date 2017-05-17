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
    class DocotServiceBinder:Binder
    {
        public DocotServiceBinder(DocotService service)
        {
            this.Service = service;
        }

        public void RegisterLatitudeLongtude()
        {
            Service.RegisterLatitudeLongtude();
        }

        public string GetFormattedTimestamp()
        {
            return Service?.GetFormattedTimestamp();
        }

        public bool IsUserInfoRegistered()
        {
            return Service.IsUserInfoRegistered();
        }

        public void SetUserPreferences(string deviceId, string nickname)
        {
            Service.SetUserPreferences(deviceId, nickname);

        }

        public void RemoveUserPreference()
        {
            Service.RemoveUserPreference();
        }

        public DocotService Service { get; private set; }
    }
}