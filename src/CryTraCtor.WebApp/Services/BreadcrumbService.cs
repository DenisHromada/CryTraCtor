using MudBlazor;

namespace CryTraCtor.WebApp.Services;

public class BreadcrumbService : IBreadcrumbService
{
    public List<BreadcrumbItem> Items { get; private set; } = [];

    public event Action? OnBreadcrumbsChanged;

    public void SetBreadcrumbs(List<BreadcrumbItem> items)
    {
        Items = items;
        OnBreadcrumbsChanged?.Invoke();
    }
}
