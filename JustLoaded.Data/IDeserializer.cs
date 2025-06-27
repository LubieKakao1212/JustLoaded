namespace JustLoaded.Data;

public interface IDeserializer<in TAssetData> where TAssetData : IAssetData {

    public T? Deserialize<T>(TAssetData data);
    
    public object? Deserialize(TAssetData data, Type targetType);
    
}