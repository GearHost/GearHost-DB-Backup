using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using NDesk.Options;

namespace GearHost.Database.Backup
{
    public static class Program
    {
        private static string defaultDownloadsPath;

        private static string apiKey;
        private static string downloadsPath;
        private static string dbName;
        private static bool showHelp;

        private static long ToUnixTimestamp(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }

        private static void Init()
        {
            defaultDownloadsPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Downloads");
            if (!Directory.Exists(defaultDownloadsPath))
                Directory.CreateDirectory(defaultDownloadsPath);
        }

        private static bool ParseInput(string[] args)
        {
            var p = new OptionSet() {
                { "apiKey=", "your GearHost API key.", v => apiKey = v },
                { "db=", "your GearHost database name", v => dbName = v },
                { "path=", "local path to store backups.", v => downloadsPath = v },
                { "help", "displays help.", v => showHelp = v != null },
             };

            try
            {
                p.Parse(args);

                downloadsPath = downloadsPath ?? defaultDownloadsPath;

                if (!showHelp)
                {
                    if (string.IsNullOrEmpty(apiKey)) throw new Exception("-apiKey option is missing");
                    if (string.IsNullOrEmpty(dbName)) throw new Exception("-db option is missing");
                }
            }
            catch (Exception e)
            {
                Console.Write("ghbackup: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `ghbackup --help' for more information.");
                return false;
            }
            return true;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("usage example:");
            Console.WriteLine(@"ghbackup.exe -apiKey=123abc -dbName=products -path=C:\Backups");
            Console.WriteLine("apiKey and dbName are required, path is optional. Default location is Downloads directory under the app folder.");
        }

        private static bool ExecBackup()
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", string.Format("bearer {0}", apiKey));
                DatabasesDTO dto = null;
                try
                {
                    var databasesJson = webClient.DownloadString("https://api.gearhost.com/v1/databases");
                    dto = new JavaScriptSerializer().Deserialize<DatabasesDTO>(databasesJson);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error downloading API data. Check your API key");
                    return false;
                }

                var database = dto.databases.FirstOrDefault(d => string.Compare(d.name, dbName, StringComparison.OrdinalIgnoreCase) == 0);
                if (database == null)
                {
                    Console.WriteLine("Database not found. Check your database name.");
                    return false;
                }
                Console.WriteLine("Database found. Executing backup...");
                string localFileName = dbName + "_" + DateTime.Now.ToUnixTimestamp() + ".zip";
                string localPath = Path.Combine(downloadsPath, localFileName);

                webClient.DownloadFile(string.Format("https://api.gearhost.com/v1/databases/{0}/backup", database.id), localPath);
            }

            return true;
        }

        public static void Main(string[] args)
        {
            Init();

            if (!ParseInput(args))
                return;

            if (showHelp)
            {
                ShowHelp();
                return;
            }

            if (!ExecBackup())
            {
                Console.WriteLine("Aborting...");
            }
            else
            {
                Console.WriteLine("Backup successfully downloaded.");
            }
        }
    }
}