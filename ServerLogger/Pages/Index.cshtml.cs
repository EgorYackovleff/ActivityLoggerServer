using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServerLogger.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataService _dataService;

    public Dictionary<string, Dictionary<string, List<string>>> AllData { get; set; }

    public IndexModel(ILogger<IndexModel> logger, DataService dataService)
    {
        _logger = logger;
        _dataService = dataService;
    }

    public void OnGet()
    {
        AllData = _dataService.GetAllData();
    }
}
