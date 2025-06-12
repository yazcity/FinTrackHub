namespace FinTrackHub.Common
{
    public class CategoryType
    {
        public const long Income = 3;
        public const long Expense = 5;

        public static readonly List<long> All = new() { Income, Expense };
    }
}
