using System;

namespace Shorty.Web.Services;

public interface IUrlShortenerService
{
    Task<string> EncurtarUrlAsync(string urlOriginal);
    Task<string?> ObterUrlOriginalAsync(string codigo, string? ip = null, string? userAgent = null);
}
