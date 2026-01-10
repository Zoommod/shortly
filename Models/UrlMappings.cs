using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace Shorty.Web.Models;

public class UrlMappings
{
    public int Id { get; set;}
    
    [Required]
    public string OriginalUrl { get; set; }
    public string Code { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int ClickCount { get; set;} = 0;
    public string? UserId { get; set; }
}
