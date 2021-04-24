using DotBadge;

namespace TestHub.Api.Badge
{
    public class BadgeGenerator
    {
        public static string GenerateBadge(int? testsCount, int failedTests, decimal? coverage)
        {
            var bp = new BadgePainter();
            var covPart = string.Empty;
            
            if (!testsCount.HasValue || testsCount == 0)
            {                
                return bp.DrawSVG("test-hub.io", $"no tests", ColorScheme.LightGray, Style.Flat);                
            }

            if (coverage.HasValue)
            {
                covPart = $" | coverage: {coverage.Value.ToString("0.##")}%";
            }

            if (failedTests == 0)
            {
                return bp.DrawSVG("test-hub.io", $"all {testsCount} tests passing" + covPart, ColorScheme.Green, Style.Flat);
            }
            else
            {
                return bp.DrawSVG("test-hub.io", $" {failedTests} out of {testsCount} tests failing" + covPart, ColorScheme.Red, Style.Flat);
            }
        }
    }
}
