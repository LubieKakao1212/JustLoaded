using Aimless.ModLoader.Content.Database;

namespace Aimless.ModLoader.Core;

public interface IDatabaseRegisterer {

    public void RegisterDatabases(IDatabaseRegistrationContext context);

}