using NRules.Fluent.Dsl;
using ValidationPOC.Domains;

namespace ValidationPOC.Rules.WellPoint
{
    [Description("This rule enforces a claim is not in the no touch list")]
    public class NoTouchRule : Rule
    {
        public override void Define()
        {
            Claim claim = null;
            When()
                .Match(() => claim, (c) => c.EnhancedData.GetBoolean("IsNoTouch") == true);
            Then()
                .Do((ctx) => claim.AddError("This claim cannot be proceeded because it is in NoTouch list."));
        }
    }
}
