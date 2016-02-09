using System;
using System.Net;

namespace GearHost.Database.Backup
{
    public class GearHostWebClient : WebClient
    {
        private const int REQUEST_TIMEOUT_MINUTES = 60;

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = (int)TimeSpan.FromMinutes(REQUEST_TIMEOUT_MINUTES).TotalMilliseconds;
            return w;
        }
    }
}
