namespace COO.Server.Features.Cats.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Validation.Cat;

    public class UpdateCatRequestModel
    {
        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
