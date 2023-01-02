namespace StocksAPI.Data
{
    public class StocksService
    {
        private readonly DbPersister _dbPersister;
        private readonly Dictionary<int, FavoriteStock> _inMemoryFavorites;

        public StocksService(DbPersister dbPersister)
        {
            _dbPersister = dbPersister;
            _inMemoryFavorites = GetFavoritesDictionary(_dbPersister.GetFavorites());
        }

        public IEnumerable<FavoriteStock> GetFavorites(int userID)
        {
            return _inMemoryFavorites.Values.Where(v => v.UserID == userID);
        }

        internal void AddFavorite(int stockID, int userID)
        {
            var added = _dbPersister.AddFavoriteStock(new FavoriteStock
            {
                StockID = stockID,
                UserID = userID
            });

            _inMemoryFavorites.Add(added.ID, added);
        }

        internal void RemoveFavorite(int stockID, int userID)
        {
            var favorite = _inMemoryFavorites.Where(v => 
                v.Value.UserID == userID && 
                v.Value.StockID == stockID)
                .Select(f => f.Value).FirstOrDefault();

            if(favorite != null) 
            {
                _dbPersister.RemoveFavoriteStock(favorite.ID);
                _inMemoryFavorites.Remove(favorite.ID);
            } 
        }

        private Dictionary<int, FavoriteStock> GetFavoritesDictionary(IEnumerable<FavoriteStock> favorites)
        {
            var result = new Dictionary<int, FavoriteStock>();

            foreach (var favorite in favorites)
            {
                result.Add(favorite.ID, favorite);
            }

            return result;
        }
    }
}
