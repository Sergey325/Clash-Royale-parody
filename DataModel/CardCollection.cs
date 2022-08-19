using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WindowsFormsApp1.DataModel
{
    public partial class CardCollection
    {
        [Key]
        public int Id { get; set; }
        public byte Barrel { get; set; } = 1;
        public byte Cannon { get; set; } = 1;
        public byte ElectroSpirit { get; set; } = 0;
        public byte Fireball { get; set; } = 1;
        public byte GobGang { get; set; } = 1;
        public byte Graveyard { get; set; } = 0;
        public byte IceGolem { get; set; } = 0;
        public byte IceSpirit { get; set; } = 0;
        public byte IceWiz { get; set; } = 0;
        public byte Log { get; set; } = 0;
        public byte MegaKnight { get; set; } = 0;
        public byte Musk { get; set; } = 0;
        public byte PEKKA { get; set; } = 1;
        public byte Prince { get; set; } = 0;
        public byte Princess { get; set; } = 0;
        public byte Rocket { get; set; } = 0;
        public byte Skellies { get; set; } = 0;
        public byte Tesla { get; set; } = 0;
        public byte Tornado { get; set; } = 0;
        public byte Valk { get; set; } = 1;
        public byte DarkPrince { get; set; } = 0;
        public byte Balloon { get; set; } = 0;
        public byte Bandit { get; set; } = 0;
        public byte Arrows { get; set; } = 1;
        public byte eBarbs { get; set; } = 1;
        public byte MP { get; set; } = 1;
        public byte Minions { get; set; } = 0;
        public byte Zap { get; set; } = 0;
        public byte SkeletonDragons { get; set; } = 0;
        public byte BattleHealer { get; set; } = 1;
        public byte Hog { get; set; } = 1;
        public byte BarbBarrel { get; set; } = 1;

        public virtual ICollection<Player> Player { get; set; }
    }
}