using NRules.Fluent.Dsl;
using ValidationPOC.Domains;

namespace ValidationPOC.Rules.Gateway
{
    [Description("This rule enforces a valid company id.")]
    public class CompanyIdRule : Rule
    {
        public override void Define()
        {
            Claim claim = null;
            When()
                .Match(() => claim, (c) => c.CompanyID != 693);
            Then()
                .Do((ctx) => claim.AddError("The specified company id is invalid."));
        }
    }
}
