namespace Aimless.ModLoader.Content.Database.Execeptions
{
    public class UnsupportedContentTypeException : ApplicationException
    {
        public readonly Type targetType;
        public readonly Type[] supportedTypes;

        public UnsupportedContentTypeException(Type targetType, Type[] supportedTypes) : base(
            $"Database does not support Type: {targetType}. " +
            $"Supported Types: {supportedTypes.Aggregate("", (result, current) => result + ", " + current.ToString())}")
        {
            this.targetType = targetType;
            this.supportedTypes = supportedTypes;
        }
    }
}
