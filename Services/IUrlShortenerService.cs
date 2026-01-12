using System;
using Shorty.Web.Models;

namespace Shorty.Web.Services;

public interface IUrlShortenerService
{
    Task<string> EncurtarUrlAsync(string urlOriginal, string? userId = null);
    Task<string?> ObterUrlOriginalAsync(string codigo, string? ip = null, string? userAgent = null);
    Task<List<UrlMappings>> ObterLinksPorUsuarioAsync(string userId);
    Task<UrlMappings?> ObterDetalhesComLogsAsync(int id);
    Task<bool> ExcluirUrlAsync(int id, string userId);
    Task<bool> AtualizarTituloAsync(int id, string novoTitulo, string userId);
}
