using System;
using System.Collections.Generic;
using System.Text;

namespace DocotChit
{
    public abstract class ServiceConnectionStub
    {
        public abstract string Getstr();

        public abstract bool IsUserInfoRegistered();

        public abstract void RegisterLatitudeLongtude();

        public abstract void SetUserPreferences(string deviceId, string nickname);

        public abstract void RemoveUserPreference();
    }
}
