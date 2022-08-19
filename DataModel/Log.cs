using System;
using System.ComponentModel.DataAnnotations;

namespace WindowsFormsApp1.DataModel
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public int? PlayerId { get; set; }
        public byte? ScoresPlayer { get; set; }
        public byte? ScoresOpponent { get; set; }
        public int? OpponentId { get; set; }
        public DateTime? time { get; set; }
        public int? Trophies { get; set; }
    }
}
