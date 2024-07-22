namespace JustLoaded.Content
{
    using Database.Extensions;
    using Database;
    
    // TODO Database refresh callback
    /// <summary>
    /// A reference to an entry from a <see cref="ContentDatabase{TContent}"/>
    /// </summary>
    /// <typeparam name="TContent">Type of content this reference relates to</typeparam>
    public struct DatabaseReference<TContent> where TContent : class {
        /// <summary>
        /// Renturns a value this reference points to or null if given database does not contein specified value
        /// </summary>
        /// <exception cref="ApplicationException">when requested datgabase could not be found</exception>
        public TContent? Value => AssertValue();

        private BoundContentKey<TContent> key;

        private TContent? _value = null;
        private IReadOnlyContentDatabase? _database = null;

        private IReadOnlyMasterDatabase? _masterDatabase;

        /// <param name="contentKey"></param>
        /// <param name="databaseKey">When null a default database for <typeparamref name="TContent"/> will be used</param>
        /// <param name="key"></param>
        /// <param name="masterDatabase"></param>
        public DatabaseReference(in BoundContentKey<TContent> key, IReadOnlyMasterDatabase? masterDatabase = null) {
            this.key = key;
            this._masterDatabase = masterDatabase;
        }

        private TContent? AssertValue()
        {
            if (_value != null)
            {
                return _value;
            }

            return _value = key.FetchContent(ref _database, _masterDatabase);
        }
    }
}
