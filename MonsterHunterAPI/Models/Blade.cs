﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonsterHunterAPI.Models
{
    public class Blade
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("ID")]
        public int ?ParentID { get; set; }
        [ForeignKey("ID")]
        public int? ChildID { get; set; }
        [Required]
        public string WeaponClass { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        [Required]
        public int RawDamage { get; set; }

        public string ElementType { get; set; }

        public int ElementDamage { get; set; }
        [Required]
        public int Sharpness { get; set; }
        [Required]
        public int Rarity { get; set; }
        [Required]
        public int Affinity { get; set; }
        [Required]
        public int Slots { get; set; }
        [Required]
        public int Defense { get; set; }
        
        public List<Material> Materials { get; set; }

    }
}
