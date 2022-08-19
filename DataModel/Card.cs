using System.ComponentModel.DataAnnotations;

namespace WindowsFormsApp1.DataModel
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string Name { get; set; }

        [Required]
        public byte Elixir { get; set; }

        [Required]
        [MaxLength(10)]
        public string Rarity { get; set; }
        [Required]
        public string iconName { get; set; }
        [Required]
        public byte Arena { get; set; }
    }
}
