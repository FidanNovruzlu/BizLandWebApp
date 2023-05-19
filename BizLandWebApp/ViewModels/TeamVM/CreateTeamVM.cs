namespace BizLandWebApp.ViewModels.TeamVM
{
    public class CreateTeamVM
    {
        public string Name { get; set; } = null!;
        public string Job { get; set; } = null!;
        public IFormFile Image { get; set; } = null!;
    }
}
