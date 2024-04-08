using System.Configuration;

namespace CryTraCtor.Helpers;

public static class GetFileFromId
{
    public static string? GetFilepathFromId(string id)
    {
        return id switch
        {
            _ => ConfigurationManager.AppSettings["CaptureFilePath"]
        };
    }
}