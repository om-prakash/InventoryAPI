using NRules.Fluent.Dsl;
using System;
using ValidationPOC.Domains;

namespace ValidationPOC.Rules.WellPoint
{

    [Description("This rule enforces the value of User001 must equal to the value of User003")]
    public class User001MustEqaulToUser003Rule : Rule
    {
        public override void Define()
        {
            // binding variable to matched fact
            Claim claim = null;
            When()
                .Match(() => claim, (c) => !c.User001.Equals(c.User003, StringComparison.OrdinalIgnoreCase));

            Then()
                .Do((ctx) => claim.AddError("User001 must equal to User003"));
        }
    }
}
