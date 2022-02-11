using Bogus;
using System.Collections.Generic;
using ValidationPOC.Domains;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ValidationPOC.RuleEngine
{
    /// <summary>
    /// A class that generates random fact objects
    /// <para>
    /// We use the Bogus fake library to generate fake data. More detailed information
    /// can be seen at https://github.com/bchavez/Bogus
    /// </para>
    /// </summary>
    static class FactGenerator
    {
        // simulated enhance data
        private readonly static string[] enhancedDataJson =
        {
            "{\"IsNoTouch\": true, \"IsCOB\": true}",
            "{\"IsNoTouch\": true, \"IsCOB\": false}",
            "{\"IsNoTouch\": false, \"IsCOB\": true}",
            "{\"IsNoTouch\": false, \"IsCOB\": false}"
        };

        // setup the claim faker and configure fake rules for public properties
        private readonly static Faker<Claim> claimFaker = new Faker<Claim>()
                .StrictMode(false)
                .RuleFor(c => c.ClaimAuthor, f => f.Name.FullName())
                .RuleFor(c => c.ClaimNum, f => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.User001, f => f.Random.AlphaNumeric(20))
                .RuleFor(c => c.User002, f => f.Random.AlphaNumeric(20))
                .RuleFor(c => c.User003, f => f.Random.AlphaNumeric(20))
                .RuleFor(c => c.EnhancedData, f => JObject.Parse(f.PickRandom(enhancedDataJson)));

        /// <summary>
        /// Generate an instance of <seealso cref="Claim"/> based on the fake rules
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <returns>An intance of <seealso cref="Claim"/></returns>
        internal static Claim GenerateClaimFact(int companyId)
        {
            Claim claim = claimFaker.Generate();
            claim.CompanyID = companyId;
            // debug the generated claim object
            System.Console.WriteLine(JsonConvert.SerializeObject(claim, Formatting.Indented));
            return claim;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        internal static IEnumerable<Claim> GenerateClaimListFact(int count, int companyId)
        {
            IEnumerable<Claim> claims = claimFaker.Generate(count);
            foreach(Claim claim in claims)
            {
                claim.CompanyID = companyId;
                // debug the generated claim object
                System.Console.WriteLine(JsonConvert.SerializeObject(claim, Formatting.Indented));
            }
            return claims;
        }
    }
}
