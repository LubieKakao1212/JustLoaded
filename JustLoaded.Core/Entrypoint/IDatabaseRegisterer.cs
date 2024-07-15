using JustLoaded.Content.Database;

namespace JustLoaded.Core;

public interface IDatabaseRegisterer {

    public void RegisterDatabases(IDatabaseRegistrationContext context);

}