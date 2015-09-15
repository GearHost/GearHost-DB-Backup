using System;

namespace GearHost.Database.Backup
{
    public class DatabaseDTO
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string plan { get; set; }
        public string type { get; set; }
        public long size { get; set; }
        public bool locked { get; set; }
        public DateTime dateCreated { get; set; }
    }

    public class DatabasesDTO
    {
        public DatabaseDTO[] databases { get; set; }
    }
}