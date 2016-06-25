﻿namespace App.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Entity : BaseModel<int>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
