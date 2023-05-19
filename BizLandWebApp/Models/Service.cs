using System.ComponentModel.DataAnnotations;

namespace BizLandWebApp.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string IconName { get; set; } = null!;
        [Required]
        public string Description { get; set; }=null!;
    }
}
