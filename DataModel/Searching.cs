using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindowsFormsApp1.DataModel
{
    public class Searching
    {
        [Key]
        public int PlayerId { get; set; }
        public int Trophies { get; set; }
        public int? OpponentId { get; set; }

        [ForeignKey("PlayerId")]
        public virtual Player Player { get; set; }
    }
}
