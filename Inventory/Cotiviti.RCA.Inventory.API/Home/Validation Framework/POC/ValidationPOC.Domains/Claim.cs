namespace ValidationPOC.Domains
{
    public class Claim: Entity
    {
        public int CompanyID { get; set; }
        public string ClaimNum { get; set; }

        public string ClaimAuthor { get; set; }

        public string User001 { get; set; }

        public string User002 { get; set; }

        public string User003 { get; set; }

    }
}
