using JustLoaded.Content.Database;

namespace JustLoaded.Core.Entrypoint;

public interface IDatabaseRegisterer {

    public void RegisterDatabases(IDatabaseRegistrationContext context);

}