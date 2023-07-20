using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using Color = System.Drawing.Color;

namespace ServerLogger.Controllers;

[ApiController]
[Route("api")]
public class WorkActivityController : ControllerBase
{
    public WorkActivityController(DataService dataService)
    {
        _dataService = dataService;
    }

    private readonly DataService _dataService;

    [HttpPost("workactivity")]
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
        
        return Ok();
    }


    [HttpPost("screenshot/{width:int}/{height:int}")]
    public async Task<IActionResult> Screenshot(int width, int height)
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                await Request.Body.CopyToAsync(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                var ipAddressAndTime = HttpContext.Connection.RemoteIpAddress + " " + DateTime.UtcNow;

                var screenshotPath = await SaveScreenshot(imageData, width, height);
                _dataService.SaveClientScreenshot(ipAddressAndTime, screenshotPath);

                return Ok();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private async Task<string> SaveScreenshot(byte[] imageData, int width, int height)
    {
        var screenshotsDirectory = "wwwroot/Screenshots";
        if (!Directory.Exists(screenshotsDirectory))
            Directory.CreateDirectory(screenshotsDirectory);

        var screenshotFileName = $"{Guid.NewGuid()}_screenshot.png";
        var screenshotPath = Path.Combine(screenshotsDirectory, screenshotFileName);


        using var image = GetDataPicture(width, height, imageData);
        image.Save(screenshotPath, ImageFormat.Png);


        return screenshotPath;
    }

    public Bitmap GetDataPicture(int w, int h, byte[] data)
    {
        var pic = new Bitmap(w, h, PixelFormat.Format32bppArgb);

        int arrayIndex = 0;
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                Color c = Color.FromArgb(
                    data[arrayIndex],
                    data[arrayIndex + 1],
                    data[arrayIndex + 2],
                    data[arrayIndex + 3]
                );
                pic.SetPixel(x, y, c);

                arrayIndex += 4;
            }
        }

        return pic;
    }
}