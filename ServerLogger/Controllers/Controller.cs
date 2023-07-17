using System.Text;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
    
    
    [HttpPost("screenshot")]
    public IActionResult Screenshot()
    {
        try
        {
            byte[] imageData;

            using (var reader = new BinaryReader(Request.Body))
                imageData = reader.ReadBytes((int)Request.ContentLength);

            var ipAddressAndTime = HttpContext.Connection.RemoteIpAddress + " " + DateTime.UtcNow;
            var screenshotPath = SaveScreenshot(ipAddressAndTime, imageData);

            _dataService.SaveClientScreenshot(ipAddressAndTime, screenshotPath);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private string SaveScreenshot(string ipAddressAndTime, byte[] imageData)
    {
        var screenshotsDirectory = "Screenshots"; 
        if (!Directory.Exists(screenshotsDirectory))
            Directory.CreateDirectory(screenshotsDirectory);

        var screenshotFileName = $"{ipAddressAndTime}_screenshot.jpg";
        var screenshotPath = Path.Combine(screenshotsDirectory, screenshotFileName);

        using (var image = Image.Load<Rgba32>(imageData))
        {
            image.Save(screenshotPath);
        }

        return screenshotPath;
    }
    
}