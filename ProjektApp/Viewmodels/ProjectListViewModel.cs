namespace ProjektApp.Viewmodels
{
    public class ProjectListViewModel
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int MemberCount { get; set; }
        public bool IsCurrentUserMember { get; set; }

        public bool IsOwner { get; set; }
    }
}
