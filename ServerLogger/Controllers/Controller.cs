using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace ServerLogger.Controllers;

[ApiController]
[Route("api/workactivity")]
public class WorkActivityController : ControllerBase
{

    
    public WorkActivityController(DataService dataService)
    {
        _dataService = dataService;
    }
    
    static public List<string> Requests = new List<string>();
    private readonly DataService _dataService;

    [HttpPost]
    public async Task<IActionResult> Callback()
    {
        string request;

        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            request = await reader.ReadToEndAsync();

        var splitData = request.Split('\n', 3);

        var dataApplication = splitData[0];
        var dataWindows = splitData[1];
        var dataActiveWindow = splitData[2];
        
        
        var ipAddressAndTime = HttpContext.Connection.RemoteIpAddress + " " + DateTime.UtcNow;


        
        _dataService.AddProcess(ipAddressAndTime, dataApplication);
        _dataService.AddWindow(ipAddressAndTime, dataWindows);
        _dataService.SetActiveWindow(ipAddressAndTime, dataActiveWindow);
        
        
        
        Requests.Add(request);

        return Ok();
    }
}