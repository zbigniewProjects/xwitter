namespace BlogProject.MVC.Utils
{
    public static class ViewUtils
    {
        public static string CalculateHowMuchTimePassed(DateTime date) 
        {
            TimeSpan elapsedTime = DateTime.UtcNow - date;

            string elapsedTimeString;
            if (elapsedTime.Days > 0)
                elapsedTimeString = elapsedTime.Days == 1 ? $"{elapsedTime.Days} day ago" : $"{elapsedTime.Days} days ago";
            else if (elapsedTime.Hours > 0)
                elapsedTimeString = elapsedTime.Hours == 1 ? $"{elapsedTime.Hours} hour ago" : $"{elapsedTime.Hours} hours ago";
            else if (elapsedTime.Minutes > 0)
                elapsedTimeString = elapsedTime.Minutes == 1 ? $"{elapsedTime.Minutes} minute ago" : $"{elapsedTime.Minutes} minutes ago";
            else
                elapsedTimeString = "Less than a minute ago";

            return elapsedTimeString;
        }
    }
}
