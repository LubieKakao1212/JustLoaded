namespace Aimless.ModLoader.Content
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

        private ContentKey contentKey;
        private ContentKey? databaseKey;

        private TContent? value = null;
        private IContentDatabase? database = null;

        private MasterDatabase? masterDatabase;

        /// <param name="contentKey"></param>
        /// <param name="databaseKey">When null a default database for <typeparamref name="TContent"/> will be used</param>
        public DatabaseReference(ContentKey contentKey, ContentKey? databaseKey = null, MasterDatabase? masterDatabase = null)
        {
            this.contentKey = contentKey;
            this.databaseKey = databaseKey;
            this.masterDatabase = masterDatabase;
        }

        private TContent? AssertValue()
        {
            if (value != null)
            {
                return value;
            }
            else if (database == null)
            {
                masterDatabase ??= GlobalDatabases.Master;
                database = masterDatabase.GetDatabase<TContent>(databaseKey);
            }

            return value = database.GetContent<TContent>(contentKey);
        }
    }
}
