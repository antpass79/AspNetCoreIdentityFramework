namespace Globe.Identity.Shared.Models
{
    public class GlobePolicy
    {
        public GlobePolicy()
        {
        }
        public GlobePolicy(string name, GlobeClaim[] claims)
        {
            Name = name;
            Claims = claims;
        }

        public string Name { get; set; }
        public GlobeClaim[] Claims { get; set; }
    }
}
