using System.Data.SqlClient;

namespace StocksAPI.Data
{
    public class User
    {
        public int ID { get; set; }

        public string FisrtName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int AvatarID { get; set; }

        internal void Read(SqlDataReader rs)
        {
            this.ID = (int)rs["ID"];
            this.FisrtName = (string)rs["FirstName"];
            this.LastName = (string)rs["LastName"];
            this.Email = (string)rs["Email"];
            this.Password = (string)rs["Password"];
            this.AvatarID = (int)rs["AvatarID"];
        }
    }
}
