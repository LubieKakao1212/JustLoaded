using JustLoaded.Content.Database;

namespace JustLoaded.Core.Loading;

public class DefaultDatabaseRegistrationEntrypointLoadingPhase : EntrypointLoadingPhase<IDatabaseRegisterer> {
    
    protected override void HandleEntrypointFor(Mod mod, IDatabaseRegisterer entrypoint, ModLoaderSystem modLoader) {
        //Dirty hack
        entrypoint.RegisterDatabases((IDatabaseRegistrationContext)modLoader.MasterDb);
    }
    
}