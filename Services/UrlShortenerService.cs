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

    public async Task<string> EncurtarUrlAsync(string urlOriginal, string? userId = null)
    {
        var codigo = await GerarCodigoUnicoAsync();

        var novoMapeamento = new UrlMappings
        {
            OriginalUrl = urlOriginal,
            Code = codigo,
            CreatedAt = DateTime.UtcNow,
            ClickCount = 0,
            UserId = userId
        };

        _context.UrlMappings.Add(novoMapeamento);
        await _context.SaveChangesAsync();

        return codigo;
    }

    public async Task<string?> ObterUrlOriginalAsync(string codigo, string? ip = null, string? userAgent = null)
    {
        var mapeamento = await _context.UrlMappings.FirstOrDefaultAsync(u => u.Code == codigo);
        if (mapeamento == null) return null;

        mapeamento.ClickCount++;
        
        await _context.SaveChangesAsync(); 

        try
        {
            var log = new UrlAcessLog
            {
                UrlMappingId = mapeamento.Id,
                DataAcesso = DateTime.UtcNow,
                IpAddress = ip,
                UserAgent = userAgent
            };

            _context.UrlAcessLogs.Add(log);
            
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO AO SALVAR LOG: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"DETALHE DO ERRO: {ex.InnerException.Message}");
            }
        }

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

    public async Task<List<UrlMappings>> ObterLinksPorUsuarioAsync(string userId)
    {
        return await _context.UrlMappings.Where(u => u.UserId == userId).OrderByDescending(u => u.Id).ToListAsync();
    }

    public async Task<UrlMappings?> ObterDetalhesComLogsAsync(int id)
    {
        return await _context.UrlMappings.Include(u => u.UrlAcessLogs).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> ExcluirUrlAsync(int id, string userId)
    {
        var urlParaDeletar = await _context.UrlMappings.FirstOrDefaultAsync(u => u.Id == id);

        if(urlParaDeletar == null || urlParaDeletar.UserId != userId){
            return false;
        }

        _context.UrlMappings.Remove(urlParaDeletar);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AtualizarTituloAsync(int id, string novoTitulo, string userId)
    {
        var url = await _context.UrlMappings.FirstOrDefaultAsync(u => u.Id == id);

        if(url == null || url.UserId != userId) return false;

        url.Title = novoTitulo;
        await _context.SaveChangesAsync();

        return true;
    }

}
