using System.Data;
using System.Data.SqlClient;

namespace StocksAPI.Data
{
    public class DbPersister
    {
        private string connectionString;

        public DbPersister(IConfiguration config)
        {
            this.connectionString = GetConnectionString(config);
        }

        internal IEnumerable<User> GetUsers()
        {   
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmdProc = new SqlCommand("dbo.prcGetUsers", con))
            {
                cmdProc.CommandType = CommandType.StoredProcedure;
                con.Open();

                var reader = cmdProc.ExecuteReader();

                var users = new List<User>();
                using (reader)
                {
                    while (reader.Read())
                    {
                        var user = new User();
                        user.Read(reader);
                        users.Add(user);
                    }
                }

                return users;
            }
        }

        internal User AddUser(User user)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmdProc = new SqlCommand("prcAddUser", con))
            {
                cmdProc.CommandType = CommandType.StoredProcedure;
                cmdProc.Parameters.AddWithValue("@FirstName", user.FisrtName);
                cmdProc.Parameters.AddWithValue("@LastName", user.LastName);
                cmdProc.Parameters.AddWithValue("@Email", user.Email);
                cmdProc.Parameters.AddWithValue("@Password", user.Password);

                con.Open();

                var reader = cmdProc.ExecuteReader();

                var addedUser = new User();
                using (reader)
                {
                    reader.Read();
                    addedUser.Read(reader);
                }

                return addedUser;
            }
        }

        internal User EditUser(User user)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmdProc = new SqlCommand("prcEditUser", con))
            {
                cmdProc.CommandType = CommandType.StoredProcedure;
                cmdProc.Parameters.AddWithValue("@ID", user.ID);
                cmdProc.Parameters.AddWithValue("@FirstName", user.FisrtName);
                cmdProc.Parameters.AddWithValue("@LastName", user.LastName);
                cmdProc.Parameters.AddWithValue("@AvatarID", user.AvatarID);

                con.Open();

                var reader = cmdProc.ExecuteReader();

                var addedUser = new User();
                using (reader)
                {
                    reader.Read();
                    addedUser.Read(reader);
                }

                return addedUser;
            }
        }

        internal IEnumerable<FavoriteStock> GetFavorites()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmdProc = new SqlCommand("dbo.prcGetFavoriteStocks", con))
            {
                cmdProc.CommandType = CommandType.StoredProcedure;
                con.Open();

                var reader = cmdProc.ExecuteReader();

                var supplies = new List<FavoriteStock>();

                using (reader)
                {
                    while (reader.Read())
                    {
                        var favorite = new FavoriteStock();
                        favorite.Read(reader);
                        supplies.Add(favorite);
                    }
                }

                return supplies;
            }
        }

        internal FavoriteStock AddFavoriteStock(FavoriteStock favorite)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmdProc = new SqlCommand("prcAddFavoriteStock", con))
            {
                cmdProc.CommandType = CommandType.StoredProcedure;
                cmdProc.Parameters.AddWithValue("@UserID", favorite.UserID);
                cmdProc.Parameters.AddWithValue("@StockID", favorite.StockID);

                con.Open();

                var reader = cmdProc.ExecuteReader();

                var addedFavorite = new FavoriteStock();

                using (reader)
                {
                    reader.Read();
                    
                    addedFavorite.Read(reader);
                }

                return addedFavorite;
            }
        }

        internal bool RemoveFavoriteStock(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmdProc = new SqlCommand("prcRemoveFavoriteStock", con))
            {
                cmdProc.CommandType = CommandType.StoredProcedure;
                cmdProc.Parameters.AddWithValue("@ID", id);

                con.Open();

                cmdProc.ExecuteNonQuery();

                return true;
            }
        }

        public string GetConnectionString(IConfiguration config)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "den1.mssql8.gear.host";
            builder.UserID = "fusiondb1";
            builder.Password = config["dbPass"];
            builder.InitialCatalog = "fusiondb1";

            return builder.ConnectionString;
        }
    }
}