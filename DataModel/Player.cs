using System.ComponentModel.DataAnnotations;

namespace WindowsFormsApp1.DataModel
{


    public partial class Player
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        [Required]
        public string Nickname { get; set; }

        [Required]
        [MaxLength(15)]
        public string Password { get; set; }

        [Required]
        [MaxLength(6)]
        public string Position { get; set; }
        public int? CollectionId { get; set; }
        public byte? clanPosition { get; set; }
        public int? ClanId { get; set; }

        public virtual Clan Clan { get; set; }
        public virtual CardCollection Collection { get; set; }
    }
}
