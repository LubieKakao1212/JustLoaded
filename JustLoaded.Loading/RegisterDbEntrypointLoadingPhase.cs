using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;

namespace JustLoaded.Loading;

public class RegisterDbEntrypointLoadingPhase : EntrypointLoadingPhase<IDatabaseRegisterer> {
    
    protected override void HandleEntrypointFor(Mod mod, IDatabaseRegisterer entrypoint, ModLoaderSystem modLoader) {
        //Dirty hack
        entrypoint.RegisterDatabases((IDatabaseRegistrationContext)modLoader.MasterDb);
    }
    
}