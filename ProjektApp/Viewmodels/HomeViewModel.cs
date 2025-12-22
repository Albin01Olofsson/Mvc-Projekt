using Models;

namespace ProjektApp.Viewmodels
{
    public class HomeViewModel
    {
        public List<Profile>? PublicProfiles { get; set; }
        public List<Project> LatestProjects { get; set; } = new();
        public List<CV> CVs { get; set; } = new();


    }
}
