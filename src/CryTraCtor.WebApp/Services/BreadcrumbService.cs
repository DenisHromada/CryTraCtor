using MudBlazor;

namespace CryTraCtor.WebApp.Services;

public class BreadcrumbService : IBreadcrumbService
{
    private List<BreadcrumbItem> _items = new();

    public List<BreadcrumbItem> Items => _items;

    public event Action? OnBreadcrumbsChanged;

    public void SetBreadcrumbs(List<BreadcrumbItem> items)
    {
        _items = items ?? new List<BreadcrumbItem>();
        OnBreadcrumbsChanged?.Invoke();
    }
}
