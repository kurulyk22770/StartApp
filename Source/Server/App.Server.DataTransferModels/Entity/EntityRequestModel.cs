﻿namespace App.Server.DataTransferModels.Entity
{
    using System.ComponentModel.DataAnnotations;

    using App.Data.Models;
    using App.Server.Common.Mapping;

    public class EntityRequestModel : BaseModel<int>, IMapFrom<Entity>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string UserId { get; set; }
    }
}
