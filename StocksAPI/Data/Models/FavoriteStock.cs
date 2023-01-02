using System.Data.SqlClient;

namespace StocksAPI.Data
{
    public class FavoriteStock
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int StockID { get; set;}

        internal void Read(SqlDataReader rs)
        {
            this.ID = (int)rs["ID"];
            this.UserID = (int)rs["UserID"];
            this.StockID = (int)rs["StockID"];
        }
    }
}
