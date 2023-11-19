namespace DataImportingApi.Models
{
    public class ExpenseData
    {
        public decimal Total { get; set; }
        public string? CostCentre { get; set; }
        public decimal SalesTax { get; set; }
        public decimal TotalExcludingTax { get; set; }
    }
}
