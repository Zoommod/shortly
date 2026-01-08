using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shorty.Web.Models;
using Shorty.Web.Services;

namespace Shorty.Web.Controllers;

public class HomeController : Controller
{
    private readonly IUrlShortenerService _service;

    public HomeController(IUrlShortenerService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Encurtar(string urlOriginal)
    {
        if (string.IsNullOrEmpty(urlOriginal))
        {
            ViewBag.Erro = "A URL é obrigatória!";
            return View("Index");
        }

        var codigo = await _service.EncurtarUrlAsync(urlOriginal);

        var dominio = $"{Request.Scheme}://{Request.Host}";
        var urlCurta = $"{dominio}/{codigo}";

        ViewBag.UrlCurta = urlCurta;
        ViewBag.UrlOriginal = urlOriginal;

        return View("Index");
    }

    [HttpGet("/{codigo}")]
    public async Task<IActionResult> Redirecionar(string codigo)
    {
        var urlOriginal = await _service.ObterUrlOriginalAsync(codigo);

        if(urlOriginal == null)
        {
            return NotFound();
        }

        return Redirect(urlOriginal);
    }
}
