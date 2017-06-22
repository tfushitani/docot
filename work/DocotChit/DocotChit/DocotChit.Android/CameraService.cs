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

    [Service]
    public class CameraService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}