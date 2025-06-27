using JustLoaded.Filesystem;

namespace JustLoaded.Data;

public interface IPatch {

    /// <summary>
    /// Checks if this patch is compatible with a given <paramref name="assetData"/>
    /// <example>A Json specific patch should return false, for XML assetData</example>
    /// </summary>
    bool IsCompatible(IAssetData assetData);
    
    /// <summary>
    /// Checks if given patch should be applied to a given <paramref name="assetData"/>
    /// <example>Path</example>
    /// </summary>
    bool ShouldApply(IAssetData assetData);

    /// <summary>
    /// Executes the patching operation on a given <paramref name="assetData"/> by mutating it
    /// </summary>
    void Apply(IAssetData assetData);

    /// <summary>
    /// Executes the patching operation on a copy of a given <paramref name="assetData"/> without doing changes to the original
    /// </summary>
    IAssetData CopyApply(IAssetData assetData) {
        var assetCpy = assetData.Clone();
        Apply(assetCpy);
        return assetCpy;
    }
}

//TODO add JustLoaded.Data to build workflow
public interface IPatch<in TAssetData> : IPatch where TAssetData : IAssetData {
    
    /// <inheritdoc cref="IPatch.IsCompatible"/>
    bool IsCompatible(TAssetData assetData);
    
    /// <inheritdoc cref="IPatch.ShouldApply"/>
    public bool ShouldApply(TAssetData assetData);
    
    /// <inheritdoc cref="IPatch.Apply"/>
    public void Apply(TAssetData assetData);

    bool IPatch.IsCompatible(IAssetData assetData) => assetData is TAssetData asset && IsCompatible(asset);
    
    bool IPatch.ShouldApply(IAssetData assetData) => ShouldApply((TAssetData)assetData);
    
    void IPatch.Apply(IAssetData assetData) => Apply((TAssetData)assetData);
}