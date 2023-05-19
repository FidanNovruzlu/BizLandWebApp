namespace BizLandWebApp.ViewModels.TeamVM
{
    public class UpdateTeamVM
    {
        public string Name { get; set; } = null!;
        public string Job { get; set; } = null!;
        public string? ImageName { get; set; }
        public IFormFile?  Image { get; set; }
    }
}
