using NRules.Fluent.Dsl;
using System;
using ValidationPOC.Domains;

namespace ValidationPOC.Rules.WellPoint
{
    [Description("This rule enforces COB rules")]
    public class COBRule : Rule
    {
        public override void Define()
        {
            Claim claim = null;
            When()
                .Match(() => claim, (c) => c.EnhancedData.GetBoolean("IsCOB") == true,
                       (c) => c.User002.Equals("123456", StringComparison.OrdinalIgnoreCase));
            Then()
                .Do((ctx) => claim.AddError($"A claim cannot be COB claim if the value of its User002 is {claim.User002}"));
        }
    }
}
