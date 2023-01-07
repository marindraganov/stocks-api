namespace StocksAPI.Controllers.Models
{
    public class TransactionData
    {
        public int TypeID { get; set; }

        public int StockID { get; set; }

        public double StockPrice { get; set; }

        public double Quantity { get; set; }

        public DateTime Date { get; set; }
    }
}
