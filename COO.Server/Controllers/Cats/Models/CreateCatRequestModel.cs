using System.ComponentModel.DataAnnotations;

namespace COO.Server.Controllers.Cats.Models
{
    using static Data.Validation.Cat;

    public class CreateCatRequestModel
    {
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
