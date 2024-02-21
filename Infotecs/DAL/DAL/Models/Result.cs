using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infotecs.DAL.Models
{
    public class Result
    {
        [Key]
        public string filename { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime mindatetime { get; set; }
        public double avgtimeinseconds { get; set; }
        public double avgindicatorvalue { get; set; }
        public double medianindicatorvalue { get; set; }
        public double maxindicatorvalue { get; set; }
        public double minindicatorvalue { get; set; }
        public int totalrows { get; set; }
        public TimeSpan alltime { get; set; }
    }
}
