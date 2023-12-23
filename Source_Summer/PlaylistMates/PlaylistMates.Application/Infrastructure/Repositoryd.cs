using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PlaylistMates.Application.Documents;

namespace PlaylistMates.Application.Infrastructure
{
    /// <summary>
    /// Generisches Repository für alle Documents
    /// </summary>
    public class Repositoryd<TDocument, TKey> where TDocument : IDocument<TKey>
    {
        protected readonly IMongoCollection<TDocument> _coll;

        public IMongoQueryable<TDocument> Queryable => _coll.AsQueryable();

        public Repositoryd(IMongoCollection<TDocument> coll)
        {
            _coll = coll;
        }

        public virtual void InsertOne(TDocument element) => _coll.InsertOne(element);

        public virtual void DeleteOne(TKey id) => _coll.DeleteOne(Builders<TDocument>.Filter.Eq("_id", id));

        public virtual void UpdateOne(TDocument element) => _coll.ReplaceOne(Builders<TDocument>.Filter.Eq("_id", element.Id), element);

        public virtual TDocument? FindById(TKey id)
        {
            var result = _coll.Find(Builders<TDocument>.Filter.Eq("_id", id));
            return result.FirstOrDefault();
        }

        public virtual void DeleteAll() => _coll.DeleteMany(Builders<TDocument>.Filter.Empty);
    }
}