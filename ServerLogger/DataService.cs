using System.Collections.Generic;
using System.Linq;

public class DataService
{
    private readonly Dictionary<string, List<string>> processListByipAddressAndTime;
    private readonly Dictionary<string, List<string>> windowListByipAddressAndTime;
    private readonly Dictionary<string, string> activeWindowByipAddressAndTime;
    private readonly Dictionary<string, string> screenshotsByipAddressAndTime;

    public DataService()
    {
        processListByipAddressAndTime = new Dictionary<string, List<string>>();
        windowListByipAddressAndTime = new Dictionary<string, List<string>>();
        activeWindowByipAddressAndTime = new Dictionary<string, string>();
    }

    public void AddProcess(string ipAddressAndTime, string process)
    {
        if (!processListByipAddressAndTime.ContainsKey(ipAddressAndTime))
        {
            processListByipAddressAndTime[ipAddressAndTime] = new List<string>();
        }

        processListByipAddressAndTime[ipAddressAndTime].Add(process);
    }
    
    public void SaveClientScreenshot(string ipAddressAndTime, string path)
    {
        screenshotsByipAddressAndTime[ipAddressAndTime] = path;
    }
    

    public void AddWindow(string ipAddressAndTime, string window)
    {
        if (!windowListByipAddressAndTime.ContainsKey(ipAddressAndTime))
        {
            windowListByipAddressAndTime[ipAddressAndTime] = new List<string>();
        }

        windowListByipAddressAndTime[ipAddressAndTime].Add(window);
    }

    public void SetActiveWindow(string ipAddressAndTime, string window)
    {
        activeWindowByipAddressAndTime[ipAddressAndTime] = window;
    }

    public List<string> GetProcessList(string ipAddressAndTime)
    {
        if (processListByipAddressAndTime.ContainsKey(ipAddressAndTime))
        {
            return processListByipAddressAndTime[ipAddressAndTime];
        }

        return new List<string>();
    }

    public List<string> GetWindowList(string ipAddressAndTime)
    {
        if (windowListByipAddressAndTime.ContainsKey(ipAddressAndTime))
        {
            return windowListByipAddressAndTime[ipAddressAndTime];
        }

        return new List<string>();
    }

    public string GetActiveWindow(string ipAddressAndTime)
    {
        if (activeWindowByipAddressAndTime.ContainsKey(ipAddressAndTime))
        {
            return activeWindowByipAddressAndTime[ipAddressAndTime];
        }

        return string.Empty;
    }
    
    public Dictionary<string, Dictionary<string, List<string>>> GetAllData()
    {
        var allData = new Dictionary<string, Dictionary<string, List<string>>>();

        foreach (var ipAddressAndTime in processListByipAddressAndTime.Keys)
        {
            var data = new Dictionary<string, List<string>>();
            data["ProcessList"] = GetProcessList(ipAddressAndTime);
            data["WindowList"] = GetWindowList(ipAddressAndTime);
            data["ActiveWindow"] = new List<string> { GetActiveWindow(ipAddressAndTime) };

            allData[ipAddressAndTime] = data;
        }

        return allData;
    }
    
}