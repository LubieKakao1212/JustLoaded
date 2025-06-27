using JustLoaded.Filesystem;

namespace JustLoaded.Data;

public interface IAssetData {

    ModAssetPath Path { get; }

    IAssetData Clone();
    
}