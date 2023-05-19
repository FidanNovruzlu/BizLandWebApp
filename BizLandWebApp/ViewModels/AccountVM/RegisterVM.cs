using System.ComponentModel.DataAnnotations;

namespace BizLandWebApp.ViewModels.AccountVM
{
    public class RegisterVM
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; }= null!;
        [Required,MaxLength(15)]
        public string UserName { get; set; } = null!;
        [Required,MinLength(8),DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required,MinLength(8),Compare(nameof(Password)),DataType(DataType.Password)]
        public string ConfrimPassword { get; set; } = null!;
        [Required,EmailAddress]
        public string Email { get; set; } = null!;
    }
}
