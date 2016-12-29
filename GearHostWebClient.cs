using System;
using System.Net;
using System.Threading;

namespace GearHost.Database.Backup
{
    public class GearHostWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = Timeout.Infinite;
            return w;
        }
    }
}
