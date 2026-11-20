
using SQLite;

namespace laba1_Lavreniuk_FIT2_4.Models;

public class SyncQueueItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string EntityType { get; set; } = "Student";

    public string Operation { get; set; } = string.Empty;

    public string PayloadJson { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
