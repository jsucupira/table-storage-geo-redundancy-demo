using System;

namespace Model.Archiver
{
    [Serializable]
    public class Archive
    {
        public Archive()
        {
            ArchiveId = Guid.NewGuid().ToString();
        }

        public string Action { get; set; }
        public string ArchiveId { get; set; }
        public string Object { get; set; }
        public string Type { get; set; }
    }
}