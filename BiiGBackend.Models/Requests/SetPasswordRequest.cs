using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BiiGBackend.Models.Requests
{
    public class SetPasswordRequest
    {
        [NotNull, Required]
        public Guid? UserId { get; set; }
        [NotNull, Required]
        public string? Password { get; set; }
        [NotNull, Required, Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}
