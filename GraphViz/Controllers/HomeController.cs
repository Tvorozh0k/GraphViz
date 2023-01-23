using GraphViz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Text;
using Graph;

namespace GraphViz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile formFile)
        {
            string data = "";

            try
            {
                if (formFile != null && formFile.Length != 0)
                {
                    ViewBag.Message = "Файл загружен успешно :)";

                    using (var reader = new StreamReader(formFile.OpenReadStream()))
                    {
                        data = reader.ReadToEndAsync().Result;
                    }
                }           
            }
            catch
            {
                ViewBag.Message = "Возникли ошибки при загрузке файла :(";
            }

            string fileCreatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataCreate.txt");
            string fileVizPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataViz.txt");

            using (StreamWriter file = new StreamWriter(fileCreatePath)) 
            {
                file.Write(data);
            }

            WorkGraph graph = new WorkGraph(fileCreatePath);

            Algorithm algorithm = new Algorithm(graph);
            algorithm.Print(fileVizPath);

            return View();
        }

        public ActionResult AddVertice()
        {
            return PartialView("AddVertice");
        }

        [HttpPost]
        public ActionResult AddVertice(Vertice v)
        {
            string name = v.name;

            string fileCreatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataCreate.txt");
            string fileVizPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataViz.txt");

            WorkGraph graph = new WorkGraph(fileCreatePath);

            int result = graph.AddVertice(name);

            if (result == 1) ModelState.AddModelError("name", "Вершина с таким названием уже существует");
            else
            {
                Algorithm algorithm = new Algorithm(graph);
                algorithm.Print(fileVizPath);
                graph.Print(fileCreatePath);
            } 

            return RedirectToAction("Index", "Home");
        }

        public ActionResult RemoveVertice()
        {
            return PartialView("RemoveVertice");
        }

        [HttpPost]
        public ActionResult RemoveVertice(Vertice v)
        {
            string name = v.name;

            string fileCreatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataCreate.txt");
            string fileVizPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataViz.txt");

            WorkGraph graph = new WorkGraph(fileCreatePath);

            int result = graph.RemoveVertice(name);

            if (result == 1) ModelState.AddModelError("name", "Вершина с таким названием не существует");
            else
            {
                Algorithm algorithm = new Algorithm(graph);
                algorithm.Print(fileVizPath);
                graph.Print(fileCreatePath);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddEdge()
        {
            return PartialView("AddEdge");
        }

        [HttpPost]
        public ActionResult AddEdge(Edge e)
        {
            string from = e.from;
            string to = e.to;

            string fileCreatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataCreate.txt");
            string fileVizPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataViz.txt");

            WorkGraph graph = new WorkGraph(fileCreatePath);

            int result = graph.AddEdge(from, to);

            if (result == 0) 
            {
                Algorithm algorithm = new Algorithm(graph);
                algorithm.Print(fileVizPath);
                graph.Print(fileCreatePath);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult RemoveEdge()
        {
            return PartialView("RemoveEdge");
        }

        [HttpPost]
        public ActionResult RemoveEdge(Edge e)
        {
            string from = e.from;
            string to = e.to;

            string fileCreatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataCreate.txt");
            string fileVizPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataViz.txt");

            WorkGraph graph = new WorkGraph(fileCreatePath);

            int result = graph.RemoveEdge(from, to);

            if (result == 0)
            {
                Algorithm algorithm = new Algorithm(graph);
                algorithm.Print(fileVizPath);
                graph.Print(fileCreatePath);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult GenerateGraph()
        {
            return PartialView("GenerateGraph");
        }

        [HttpPost]
        public ActionResult GenerateGraph(RandomGraph par)
        {
            int N = par.N, p = par.p;

            WorkGraph graph = new WorkGraph();

            for (int i = 1; i <= N; ++i)
                graph.AddVertice(i.ToString());

            Random rnd = new Random();

            for (int u = 1; u <= N; ++u)
                for (int v = 1; v <= N; ++v)
                    if (rnd.Next(0, 101) < p) graph.AddEdge(u.ToString(), v.ToString());

            string fileCreatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataCreate.txt");
            string fileVizPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataViz.txt");

            Algorithm algorithm = new Algorithm(graph);
            algorithm.Print(fileVizPath);
            graph.Print(fileCreatePath);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult GetFile()
        {
            string fileCreatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataCreate.txt");
            return File(System.IO.File.OpenRead(fileCreatePath), "application/octet-stream", Path.GetFileName(fileCreatePath));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
