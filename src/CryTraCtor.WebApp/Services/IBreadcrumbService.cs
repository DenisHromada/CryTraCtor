using MudBlazor;

namespace CryTraCtor.WebApp.Services;

public interface IBreadcrumbService
{
    List<BreadcrumbItem> Items { get; }
    event Action? OnBreadcrumbsChanged;
    void SetBreadcrumbs(List<BreadcrumbItem> items);
}
