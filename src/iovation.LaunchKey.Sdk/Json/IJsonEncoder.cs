namespace iovation.LaunchKey.Sdk.Json
{
    public interface IJsonEncoder
    {
        TResult DecodeObject<TResult>(string data);
        string EncodeObject(object obj);
    }
}