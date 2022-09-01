using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crawler.Models;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Crawler.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async  Task<IActionResult> Index()
    {
        // 1
        var httpClient = new HttpClient();
        // All the html of the url
        var response = await httpClient.GetStringAsync("https://docs.microsoft.com/en-us/ef/core/");

        // 2
        var htmlDocumnet = new HtmlDocument();
        htmlDocumnet.LoadHtml(response);
        // 3
        var nodes = htmlDocumnet.DocumentNode.SelectNodes("//div[@class='content ']");
        // var test = htmlDocumnet.DocumentNode.Descendants("div").Where(x => x.GetAttributeValue("class", "content").Contains("")).ToList();
        var nodesData = nodes.Select(x => x.InnerHtml).ToList();
        // var header = nodes.Where(x => x.FirstChild.GetClasses()).ToList();
        ViewBag.crawlerData = nodesData;

        return View();
    }

    public async Task<IActionResult> Privacy()
    {
        // 1
        var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync("https://www.tutorialsteacher.com/mvc");
        // 2
        var htmlDocumnet = new HtmlDocument();
        htmlDocumnet.LoadHtml(response);
        // 3
        // var areaLink = htmlDocumnet.DocumentNode.Descendants("li").Where(node => !node.GetAttributeValue("class" , "").Contains("" ))





        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
