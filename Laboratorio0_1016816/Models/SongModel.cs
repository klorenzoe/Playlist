using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Laboratorio0_1016816.Models
{
    public class SongModel
    {
        [Key]
        [Required]
        [DisplayName("Nombre")]
        public string name { get; set;}

        [Required]
        [DisplayName("Duración")]
        public string duration { get; set; }

        [Required]
        public TimeSpan durationSeconds { get; set; }

        [Required]
        [DisplayName("Intérprete")]
        public string singer { get; set; }

        [Required]
        public string SoundPath;
    }
}