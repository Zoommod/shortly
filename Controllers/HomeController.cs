using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
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

        string? userId = null;
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }

        var codigo = await _service.EncurtarUrlAsync(urlOriginal, userId);

        var dominio = $"{Request.Scheme}://{Request.Host}";
        var urlCurta = $"{dominio}/{codigo}";

        ViewBag.UrlCurta = urlCurta;
        ViewBag.UrlOriginal = urlOriginal;

        return View("Index");
    }

    [HttpGet("/{codigo}")]
    public async Task<IActionResult> Redirecionar(string codigo)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers["User-Agent"].ToString();

        var urlOriginal = await _service.ObterUrlOriginalAsync(codigo, ip, userAgent);

        if(urlOriginal == null)
        {
            return NotFound();
        }

        return Redirect(urlOriginal);
    }

    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var meusLinks = await _service.ObterLinksPorUsuarioAsync(userId);
        return View(meusLinks);
    }

    [Authorize]
    public async Task<IActionResult> Analytics(int id)
    {
        var urlDetails = await _service.ObterDetalhesComLogsAsync(id);

        if(urlDetails == null) return NotFound();

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if(urlDetails.UserId != userId)
        {
            return Forbid();
        }

        return View(urlDetails);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Excluir(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var sucesso = await _service.ExcluirUrlAsync(id, userId);

        if (sucesso)
        {
            TempData["Messagem"] = "Link excluído com sucesso!";
        }
        else
        {
            TempData["Erro"] = "Erro ao excluir o link. Tente novamente";
        }

        return RedirectToAction("Dashboard");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Renomear(int id, string novoTitulo)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if(userId == null) return RedirectToAction("Login", "Account");

        var sucesso = await _service.AtualizarTituloAsync(id, novoTitulo, userId);

        if (sucesso)
        {
            TempData["Mensagem"] = "Link renomado com sucesso!";
        }
        else
        {
            TempData["Erro"] = "Erro ao renomear o link.";
        }

        return RedirectToAction("Dashboard");
    }
}
