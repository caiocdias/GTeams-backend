using GTeams_backend.Data;
using GTeams_backend.Dtos.DateDimensionDtos;
using GTeams_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace GTeams_backend.Services;

public class DateDimensionService(AppDbContext dbContext)
{
    public async Task<DateDimensionReturnDto> AddDateDimensionAsync(DateDimensionRegisterDto dateDimensionRegisterDto)
    {
        if (dateDimensionRegisterDto.StartDate > dateDimensionRegisterDto.EndDate)
            throw new ArgumentException("Start date cannot be greater than end date");

        DateDimension dateDimension = new DateDimension
        {
            Name = dateDimensionRegisterDto.Name,
            IsActive = dateDimensionRegisterDto.IsActive,
            StartDate = dateDimensionRegisterDto.StartDate,
            EndDate = dateDimensionRegisterDto.EndDate,
            TotalBusinessDays = dateDimensionRegisterDto.TotalBusinessDays
        };
        
        dbContext.DateDimensions.Add(dateDimension);
        await dbContext.SaveChangesAsync();
        
        return new DateDimensionReturnDto
        {
            Id = dateDimension.Id,
            Name = dateDimension.Name,
            IsActive = dateDimension.IsActive,
            StartDate = dateDimension.StartDate,
            EndDate = dateDimension.EndDate,
            TotalBusinessDays = dateDimension.TotalBusinessDays,
            Teams = dateDimension.Teams
        };
    }

    public async Task<List<DateDimensionReturnDto>> GetAllDateDimensionsAsync()
    {
        List<DateDimension> dateDimensions = await dbContext.DateDimensions
            .Include(dd => dd.Teams)
            .ToListAsync();

        return dateDimensions.Select(dd => new DateDimensionReturnDto
        {
            Id = dd.Id,
            Name = dd.Name,
            IsActive = dd.IsActive,
            StartDate = dd.StartDate,
            EndDate = dd.EndDate,
            TotalBusinessDays = dd.TotalBusinessDays,
            Teams = dd.Teams
        }).ToList();
    }
}