namespace FabToolKit.Tkx.Sentinel.Rest;

public static class SentinelEndpoints
{
    public const string PostJob = "/api/v1/jobs";
    public const string GetJobList = "/api/v1/jobs";
    public const string DownloadFileFromJob = "/api/v1/files/{jobId}/{filename}/attachment";
}
