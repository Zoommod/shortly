using System;

namespace Shorty.Web.Models;

public class UrlAcessLog
{
    public int Id { get; set; }
    public DateTime DataAcesso { get; set; } = DateTime.UtcNow;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public int UrlMappingId { get; set; }
    public UrlMappings urlMapping { get; set; } = null!;

}
