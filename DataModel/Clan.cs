using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WindowsFormsApp1.DataModel
{


    public partial class Clan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(13)]
        public string Name { get; set; }
        public byte NumbOfPlayers { get; set; }
        public int TotalTrophies { get; set; }
        public string IconName { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
