namespace CryTraCtor.APi.Services;

public class PcapStorageConfig(IConfiguration configuration) : IFileStorageConfig
{
    public string CaptureFileDirectory { get; set; } = configuration["FileStorageDirectory"] ?? string.Empty;
}