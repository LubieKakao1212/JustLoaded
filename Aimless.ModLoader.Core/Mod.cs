using System.Security.Cryptography;

namespace Aimless.ModLoader.Core;

public class Mod<TMeta, TInit, TModLoaderSystem> where TModLoaderSystem : IModLoaderSystem where TMeta : ModMetadata where TInit : IModInitializer<TModLoaderSystem> {

    /// <summary>
    /// Metadata of the mod
    /// </summary>
    public TMeta Metadata { get; private set; }

    /// <summary>
    /// Initializer instance used to initialize the mod
    /// </summary>
    public TInit Initializer { get; private set; }

    public Mod(TMeta metadata, TInit initializer) {
        this.Metadata = metadata;
        this.Initializer = initializer;
    }
    
}