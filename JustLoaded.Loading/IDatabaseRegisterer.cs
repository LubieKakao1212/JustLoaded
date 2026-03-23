using JustLoaded.Content.Database;

namespace JustLoaded.Loading;

public interface IDatabaseRegisterer {

    public void RegisterDatabases(IDatabaseRegistrationContext context);

}