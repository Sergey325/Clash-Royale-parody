using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindowsFormsApp1.DataModel
{
    public class PlrInfo
    {
        [Key]
        public int PlrId { get; set; }
        public int Trophies { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public bool Online { get; set; } = false;
        public string IconName { get; set; }

        [ForeignKey("PlrId")]
        public virtual Player Player { get; set; }
    }
}
