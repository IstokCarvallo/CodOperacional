using System.Data;
using Microsoft.Data.SqlClient;

using APISegura.Common.Extensions;
using Application.DTOs.Dashboard;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories.Dashboard;

public class DashboardRepository : IDashboardRepository
{
    private readonly IConfiguration _config;

    public DashboardRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<DashboardUltimoCuartelDto>> ObtenerUltimosCuartelesAsync()
    {
        List<DashboardUltimoCuartelDto> lista = [];

        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        using var command = new SqlCommand("FProc_Dashboard_UltimosCuarteles",connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new DashboardUltimoCuartelDto
            {
                Codigo = reader.GetSafeString("Codigo"),
                Nombre = reader.GetSafeString("Nombre"),
                FechaActualizacion = reader.GetSafeDateTime("FechaActualizacion")
            });
        }
        return lista;
    }

    public async Task<IEnumerable<DashboardUltimaPlantaDto>> ObtenerUltimasPlantasAsync()
    {
        List<DashboardUltimaPlantaDto> lista = [];

        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        using var command = new SqlCommand("FProc_Dashboard_UltimasPlantas", connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new DashboardUltimaPlantaDto
            {
                Codigo = reader.GetSafeString("Codigo"),
                Nombre = reader.GetSafeString("Nombre"),
                FechaActualizacion = reader.GetSafeDateTime("FechaActualizacion")
            });
        }
        return lista;
    }

    public async Task<IEnumerable<DashboardCajasPorCodigoDto>> ObtenerCajasPorCodigoAsync()
    {
        List<DashboardCajasPorCodigoDto> lista = [];

        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        using var command = new SqlCommand("FProc_Dashboard_CajasPorCodigoOperacional", connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new DashboardCajasPorCodigoDto
            {
                Especie = reader.GetSafeString("Especie"),
                CodigoOperacional = reader.GetSafeString("CodigoOperacional"),
                CantidadCajas = reader.GetSafeInt32("CantidadCajas")
            });
        }
        return lista;
    }

    public async Task<IEnumerable<DashboardTotalCajasPorEspecieDto>> ObtenerTotalCajasHoyAsync()
    {
        List<DashboardTotalCajasPorEspecieDto> lista = [];

        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        using var command = new SqlCommand("FProc_Dashboard_TotalCajasHoy",connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new DashboardTotalCajasPorEspecieDto
            {
                Especie = reader.GetSafeString("Especie"),
                TotalCajasHoy = reader.GetSafeInt32("TotalCajasHoy")
            });
        }

        return lista;
    }

    public async Task<int> ObtenerTotalPalletsHoyAsync()
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        using var command = new SqlCommand("FProc_Dashboard_TotalPalletsHoy",connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        object? result = await command.ExecuteScalarAsync();

        return result != null ? Convert.ToInt32(result) : 0;
    }
}