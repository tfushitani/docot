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

        public string GetFormattedTimestamp()
        {
            return Service?.GetFormattedTimestamp();
        }

        public DocotService Service { get; private set; }
    }
}