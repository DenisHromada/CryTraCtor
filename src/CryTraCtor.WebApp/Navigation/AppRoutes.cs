using MudBlazor;

namespace CryTraCtor.WebApp.Navigation;

public static class AppRoutes
{
    private const string FilesSegment = "files";
    private const string FileAnalysisSegment = "file-analysis";
    private const string TrafficParticipantsSegment = "traffic-participants";
    private const string DnsSegment = "dns";
    private const string KnownDomainsSegment = "known-domains";
    private const string BitcoinSegment = "bitcoin";
    private const string CryptoProductsSegment = "crypto-products";
    private const string StatusSegment = "status";

    public const string HomePage = "/";

    public static List<BreadcrumbItem> GetHomePageBreadcrumbs() =>
        [new("Home", href: HomePage, disabled: true)];

    public const string FilesPage = $"/{FilesSegment}";
    public static string FilesUrl() => FilesPage;

    public static List<BreadcrumbItem> GetFilesPageBreadcrumbs() =>
        [new("Files", href: FilesUrl(), disabled: true)];

    public const string FileAnalysisPage = $"/{FilesSegment}/{{fileId:guid}}/{FileAnalysisSegment}";

    public static string FileAnalysisUrl(Guid fileId) =>
        $"/{FilesSegment}/{fileId}/{FileAnalysisSegment}";

    public static List<BreadcrumbItem> GetFileAnalysisPageBreadcrumbs(Guid fileId) =>
    [
        new("Files", href: FilesUrl()),
        new("File Analysis", href: FileAnalysisUrl(fileId), disabled: true)
    ];

    public const string TrafficParticipantsPage =
        $"{FileAnalysisPage}/{{fileAnalysisId:guid}}/{TrafficParticipantsSegment}";

    public static string TrafficParticipantsUrl(Guid fileId, Guid fileAnalysisId) =>
        $"{FileAnalysisUrl(fileId)}/{fileAnalysisId}/{TrafficParticipantsSegment}";

    public static List<BreadcrumbItem> GetTrafficParticipantsPageBreadcrumbs(Guid fileId, Guid fileAnalysisId) =>
    [
        new("Files", href: FilesUrl()),
        new("File Analysis", href: FileAnalysisUrl(fileId)),
        new("Traffic Participants", href: TrafficParticipantsUrl(fileId, fileAnalysisId), disabled: true)
    ];

    public const string TrafficParticipantDnsPage = $"{TrafficParticipantsPage}/{{participantId:guid}}/{DnsSegment}";

    public static string TrafficParticipantDnsUrl(Guid fileId, Guid fileAnalysisId, Guid participantId) =>
        $"{TrafficParticipantsUrl(fileId, fileAnalysisId)}/{participantId}/{DnsSegment}";

    public static List<BreadcrumbItem> GetTrafficParticipantDnsPageBreadcrumbs(Guid fileId, Guid fileAnalysisId,
        Guid participantId) =>
        BuildChildBreadcrumbs(
            GetTrafficParticipantsPageBreadcrumbs(fileId, fileAnalysisId),
            "DNS Overview",
            TrafficParticipantDnsUrl(fileId, fileAnalysisId, participantId));

    public const string TrafficParticipantKnownDomainsPage =
        $"{TrafficParticipantsPage}/{{participantId:guid}}/{KnownDomainsSegment}";

    public static string TrafficParticipantKnownDomainsUrl(Guid fileId, Guid fileAnalysisId, Guid participantId) =>
        $"{TrafficParticipantsUrl(fileId, fileAnalysisId)}/{participantId}/{KnownDomainsSegment}";

    public static List<BreadcrumbItem> GetTrafficParticipantKnownDomainsPageBreadcrumbs(Guid fileId,
        Guid fileAnalysisId, Guid participantId) =>
        BuildChildBreadcrumbs(
            GetTrafficParticipantsPageBreadcrumbs(fileId, fileAnalysisId),
            "Known Domains",
            TrafficParticipantKnownDomainsUrl(fileId, fileAnalysisId, participantId));

    public const string TrafficParticipantBitcoinPage =
        $"{TrafficParticipantsPage}/{{participantId:guid}}/{BitcoinSegment}";

    public static string TrafficParticipantBitcoinUrl(Guid fileId, Guid fileAnalysisId, Guid participantId) =>
        $"{TrafficParticipantsUrl(fileId, fileAnalysisId)}/{participantId}/{BitcoinSegment}";

    public static List<BreadcrumbItem> GetTrafficParticipantBitcoinPageBreadcrumbs(Guid fileId, Guid fileAnalysisId,
        Guid participantId) =>
        BuildChildBreadcrumbs(
            GetTrafficParticipantsPageBreadcrumbs(fileId, fileAnalysisId),
            "Bitcoin Overview",
            TrafficParticipantBitcoinUrl(fileId, fileAnalysisId, participantId));

    public const string CryptoProductsPage = $"/{CryptoProductsSegment}";
    public static string CryptoProductsUrl() => CryptoProductsPage;

    public static List<BreadcrumbItem> GetCryptoProductsPageBreadcrumbs() =>
        [new("Crypto Products", href: CryptoProductsUrl(), disabled: true)];

    public const string KnownDomainsPage = $"/{KnownDomainsSegment}";
    public static string KnownDomainsUrl() => KnownDomainsPage;

    public static List<BreadcrumbItem> GetKnownDomainsPageBreadcrumbs() =>
        [new("Known Domains", href: KnownDomainsUrl(), disabled: true)];

    public const string StatusPage = $"/{StatusSegment}";
    public static string StatusUrl() => StatusPage;

    public static List<BreadcrumbItem> GetStatusPageBreadcrumbs() =>
        [new("Status", href: StatusUrl(), disabled: true)];

    private static List<BreadcrumbItem> BuildChildBreadcrumbs(
        List<BreadcrumbItem> parentBreadcrumbs,
        string currentPageText,
        string currentPageUrl)
    {
        var breadcrumbs = new List<BreadcrumbItem>(parentBreadcrumbs);

        if (breadcrumbs.Count != 0)
        {
            var parentItem = breadcrumbs[^1];
            breadcrumbs.RemoveAt(breadcrumbs.Count - 1);

            breadcrumbs.Add(new BreadcrumbItem(parentItem.Text, href: parentItem.Href, disabled: false));
        }

        breadcrumbs.Add(new BreadcrumbItem(currentPageText, href: currentPageUrl, disabled: true));
        return breadcrumbs;
    }
}
