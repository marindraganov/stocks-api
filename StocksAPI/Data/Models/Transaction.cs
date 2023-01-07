using System.Data.SqlClient;

namespace StocksAPI.Data
{
    public class Transaction
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int TypeID { get; set; }

        public int StockID { get; set; }

        public double StockPrice { get; set; }

        public double Quantity { get; set; }

        public DateTime Date { get; set; }

        internal void Read(SqlDataReader rs)
        {
            this.ID = (int)rs["ID"];
            this.UserID = (int)rs["UserID"];
            this.TypeID = (int)rs["TypeID"];
            this.StockID = (int)rs["StockID"];
            this.StockPrice = (double)rs["StockPrice"];
            this.Quantity = (double)rs["Quantity"];
            this.Date = (DateTime)rs["Date"];
        }
    }
}
