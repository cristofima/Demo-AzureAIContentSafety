using Azure.AI.ContentSafety;

namespace AzureAIContentSafety.API.Utils
{
    public static class SeverityEvaluator
    {
        public static (int, int, int, int) Evaluate(IReadOnlyList<ImageCategoriesAnalysis> categoriesAnalysis)
        {
            var hate = categoriesAnalysis.FirstOrDefault(a => a.Category == ImageCategory.Hate)?.Severity ?? 0;
            var selfHarm = categoriesAnalysis.FirstOrDefault(a => a.Category == ImageCategory.SelfHarm)?.Severity ?? 0;
            var sexual = categoriesAnalysis.FirstOrDefault(a => a.Category == ImageCategory.Sexual)?.Severity ?? 0;
            var violence = categoriesAnalysis.FirstOrDefault(a => a.Category == ImageCategory.Violence)?.Severity ?? 0;

            return (hate, selfHarm, sexual, violence);
        }

        public static (int, int, int, int) Evaluate(IReadOnlyList<TextCategoriesAnalysis> categoriesAnalysis)
        {
            var hate = categoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.Hate)?.Severity ?? 0;
            var selfHarm = categoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.SelfHarm)?.Severity ?? 0;
            var sexual = categoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.Sexual)?.Severity ?? 0;
            var violence = categoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.Violence)?.Severity ?? 0;

            return (hate, selfHarm, sexual, violence);
        }
    }
}
