using System.ComponentModel.DataAnnotations;

namespace BetWalletApi.Models.Common
{
    public abstract class Entity
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime LastModified { get; set;}
    }
}
