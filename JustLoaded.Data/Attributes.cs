namespace JustLoaded.Data;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class DataIncludeAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class DataExcludeAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class DataAssetAttribute : Attribute { }
