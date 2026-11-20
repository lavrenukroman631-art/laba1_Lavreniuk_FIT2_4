using System.Text.Json.Serialization;
using SQLite;

namespace laba1_Lavreniuk_FIT2_4.Models
{
    public class Student
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Ignore]
        [JsonIgnore]
        public string FullName { get; set; } = string.Empty;

        public string FullNameEncrypted { get; set; } = string.Empty;

        public string Group { get; set; } = string.Empty;

        public double AverageScore { get; set; }
    }
}
