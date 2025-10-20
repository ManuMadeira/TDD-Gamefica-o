using System;
using System.Linq;
using FluentAssertions;
using Gamification.Domain.Awards.Models;
using Xunit;

namespace Gamification.Domain.Tests.Awards
{
    public class ApiSafetyTests
    {
        [Fact]
        public void API_nao_expoe_setters_perigosos()
        {
            var t1 = typeof(BadgeAward);
            var props1 = t1.GetProperties().Where(p => p.SetMethod != null && p.SetMethod.IsPublic);
            props1.Should().BeEmpty("model properties should not expose public setters");

            var t2 = typeof(RewardLog);
            var props2 = t2.GetProperties().Where(p => p.SetMethod != null && p.SetMethod.IsPublic);
            props2.Should().BeEmpty("model properties should not expose public setters");
        }
    }
}
