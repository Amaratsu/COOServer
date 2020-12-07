namespace COO.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Base;

    using static Validation.Cat;

    public class Cat : DeletableEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
