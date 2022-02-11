using NRules.Fluent.Dsl;
using ValidationPOC.Domains;

namespace ValidationPOC.Rules.Gateway
{
    [Description("This rule enforces a valid COB flag")]
    public class COBRule : Rule
    {
        public override void Define()
        {
            Claim claim = null;
            // define match conditions
            When()
                .Match(() => claim, (c) => c.EnhancedData.GetBoolean("IsCOB") == true);
            // define actions that triggered if above conditions can be matched
            Then()
                .Do((ctx) => claim.AddError("COB is invalid for Gateway claims."));
        }
    }
}
