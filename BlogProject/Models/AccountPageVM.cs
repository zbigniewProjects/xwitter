namespace BlogProject.MVC.Models
{
    public class AccountPageVM
    {
        public uint UserId { get; set; }
        public uint TotalNumberOfPosts { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Username { get; set; }
        public string? ShortInfo { get; set; }

    }
}
