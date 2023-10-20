namespace CQRSMicro.Product.Fuzzy
{
    public class FuzzyItem
    {
        public string Text { get; set; } = string.Empty;
        public string SearchString { get; set; } = string.Empty;
        public int Sscore { get; set; }
    }
}
