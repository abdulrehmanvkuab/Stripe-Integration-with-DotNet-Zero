namespace Arch.Tenants.Dashboard.Dto
{
    public class MemberActivity
    {
        public string Name { get; set; }
        public string Earnings { get; set; }
        public long Cases { get; set; }
        public long Closed { get; set; }
        public string Rate { get; set; }
        
        public string ProfilePictureName { get; set; }

        public MemberActivity(string name, string earnings, long cases, long closed, string rate, string profilePictureName)
        {
            Name = name;
            Earnings = earnings;
            Cases = cases;
            Closed = closed;
            Rate = rate;
            ProfilePictureName = profilePictureName;
        }
    }
}