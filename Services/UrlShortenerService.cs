using System;
using Shorty.Web.Data;
using Shorty.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Shorty.Web.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly AppDbContext _context;
    private const string CaracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private readonly Random _random = new Random();

    public UrlShortenerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> EncurtarUrlAsync(string urlOriginal)
    {
        var existente = await _context.UrlMappings.FirstOrDefaultAsync(u => u.OriginalUrl == urlOriginal);
        
        if(existente != null)
        {
            return existente.Code;
        }

        var codigo = await GerarCodigoUnicoAsync();

        var novoMapeamento = new UrlMappings
        {
            OriginalUrl = urlOriginal,
            Code = codigo,
            CreatedAt = DateTime.UtcNow,
            ClickCount = 0
        };

        _context.UrlMappings.Add(novoMapeamento);
        await _context.SaveChangesAsync();

        return codigo;
    }

    public async Task<string> ObterUrlOriginalAsync(string codigo)
    {
        var mapeamento = await _context.UrlMappings.FirstOrDefaultAsync(u => u.Code == codigo);
        if (mapeamento == null) return null;

        mapeamento.ClickCount++;
        await _context.SaveChangesAsync();

        return mapeamento.OriginalUrl;
    }

    private async Task<string> GerarCodigoUnicoAsync()
    {
        while (true)
        {
            char[] chars = new char[6];
            for (int i = 0; i < 6; i++)
            {
                chars[i] = CaracteresPermitidos[_random.Next(CaracteresPermitidos.Length)];
            }
            var codigoGerado = new string(chars);

            bool existe = await _context.UrlMappings.AnyAsync(u => u.Code == codigoGerado);

            if (!existe)
            {
                return codigoGerado;
            }
        }
    }




}
