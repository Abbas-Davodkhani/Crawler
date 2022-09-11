using System;
using System.Collections.Generic;
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

    public async Task<IActionResult> Index()
    {
        #region 1
        // 1
        // var httpClient = new HttpClient();
        // // All the html of the url
        // var response = await httpClient.GetStringAsync("https://docs.microsoft.com/en-us/ef/core/");

        // // 2
        // var htmlDocumnet = new HtmlDocument();
        // htmlDocumnet.LoadHtml(response);
        // // 3
        // var nodes = htmlDocumnet.DocumentNode.SelectNodes("//div[@class='content ']");
        // // var test = htmlDocumnet.DocumentNode.Descendants("div").Where(x => x.GetAttributeValue("class", "content").Contains("")).ToList();
        // var nodesData = nodes.Select(x => x.InnerHtml).ToList();
        // // var header = nodes.Where(x => x.FirstChild.GetClasses()).ToList();
        // ViewBag.crawlerData = nodesData;
        #endregion

        // var node1 = htmlDocumnet.DocumentNode.SelectNodes("//tr[@class='odd']");
        // var node2 = htmlDocumnet.DocumentNode.SelectNodes("//tr[@class='even']");




        // var tds = node;
        // var nodes = node1.Concat(node2);

        // var nodesData = nodes.Select(x => x.InnerText).ToList();
        // // List<Type> lats = new List<Type>();
        // int id = 0;
        // List<Quake> quakes = new List<Quake>();
        // foreach (var item in nodesData)
        // {
        //     var nodeData = item.Replace(" ","").Split("\n");
        //     id++;
        //     Quake quake = new Quake();
        //     quake.Id = id;
        //     quake.DateTime = nodeData[0].ToString();
        //     quake.Deep = Convert.ToDouble(changePersianNumbersToEnglish(nodeData[1]));
        //     quake.Lat = Convert.ToDouble(changePersianNumbersToEnglish(nodeData[2]));
        //     quake.Lng = Convert.ToDouble(changePersianNumbersToEnglish(nodeData[3]));
        //     quake.Max = Convert.ToDouble(changePersianNumbersToEnglish(nodeData[4]));
        //     quake.Address = nodeData[5];

        //     quakes.Add(quake);

        // }

        // //var digits = changePersianNumbersToEnglish("6464161");
        // //ViewBag.Digits = digits;
        // id = 0;
        // ViewBag.QuakesResult = quakes;
        // 1
        var httpClient = new HttpClient();
        //var response = await httpClient.GetStringAsync("http://irsc.ut.ac.ir/index.php?lang=fa");
        // var response = await httpClient.GetStringAsync("https://www.ipma.pt/en/geofisica/sismicidade");
        // 2
        
        // 3
        // var areaLink = htmlDocumnet.DocumentNode.Descendants("tbody").Where(node => !node.GetAttributeValue("class", "").Contains(""));
        // var node1 = htmlDocumnet.DocumentNode.SelectNodes("//tr[@class='DataRow1_F']");
        // var node2 = htmlDocumnet.DocumentNode.SelectNodes("//tr[@class='DataRow2_F']");

        // var divs = htmlDocumnet.DocumentNode.Descendants("div")
        // .Where(node => node.GetAttributeValue("id", "")
        // .Equals("divID2")).First();

        var response = await httpClient.GetStringAsync("https://earthquakes.bgs.ac.uk/earthquakes/recent_uk_events.html");

        var htmlDocumnet = new HtmlDocument();
        htmlDocumnet.LoadHtml(response);
        var trs = htmlDocumnet.DocumentNode.Descendants("tr").ToList();

        List<GlobalQuake> globalQuakes = new List<GlobalQuake>();
        var id = 0;
        foreach (var tr in trs.Skip(1))
        {
            var trText = tr.InnerText;
            var trData = trText.Replace(" ", "").Split("\n");
            id++;
            GlobalQuake globalQuake = new GlobalQuake();
            globalQuake.Id = id;
            globalQuake.Date = trData[1];
            globalQuake.TimeUTC = trData[2];
            globalQuake.Lat = Convert.ToDouble(trData[3]);
            globalQuake.Lon = Convert.ToDouble(trData[4]);
            globalQuake.Depth = Convert.ToDouble(trData[5]);
            globalQuake.Mag = Convert.ToDouble(trData[6]);
            globalQuake.Int = trData[7];
            globalQuake.Region = trData[8];
            globalQuake.Comment = trData[9];

            globalQuakes.Add(globalQuake);

        }
        ViewBag.Result = globalQuakes;
        return View();

        // var table = htmlDocumnet.DocumentNode.SelectNodes("//table[@class='bodyTable']");
        // var data = table.Select(x => x.InnerLength).ToList();

        // foreach (var div in divs)
        // {

        // }
        // var table = div[0].Descendants("").ToList();
        //  var tbody = getBetween(table , "<tbody>", "</tbody>");

        // return View();
    }

    public static string getBetween(string strSource, string strStart, string strEnd)
    {
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int Start, End;
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }

        return "";
    }
    public async Task<IActionResult> Privacy()
    {

        return View();
    }

    private string changePersianNumbersToEnglish(string input)
    {
        string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

        for (int j = 0; j < persian.Length; j++)
            input = input.Replace(persian[j], j.ToString());

        return input;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
