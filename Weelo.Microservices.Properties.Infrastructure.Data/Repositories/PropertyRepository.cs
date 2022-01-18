using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weelo.Microservices.Properties.Domain;
using Weelo.Microservices.Properties.Domain.DTOS;
using Weelo.Microservices.Properties.Domain.Interfaces.Repositories;
using Weelo.Microservices.Properties.Infrastructure.Data.Contexts;

namespace Weelo.Microservices.Properties.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Implements the CRUD operations for Property
    /// </summary>
    public class PropertyRepository : IBaseRepository<ParamsDTO, PaginationMetadataDTO, Property, Guid>
    {
        private readonly PropertyContext db;

        public PropertyRepository()
        {
            //db = _db;
            db = new PropertyContext();
        }
        public async Task<Property> AddAsync(Property entity)
        {
            entity.PropertyId = Guid.NewGuid();
            await db.Properties.AddAsync(entity);
            return entity;
        }

        public async Task<bool> EditAsync(Property entity)
        {
            var selectedProperty = await db.Properties.Where(x => x.PropertyId == entity.PropertyId).FirstOrDefaultAsync();

            if (selectedProperty != null)
            {
                selectedProperty.Name = entity.Name;
                selectedProperty.Address = entity.Address;
                selectedProperty.Price = entity.Price;
                selectedProperty.CodeInternal = entity.CodeInternal;
                selectedProperty.Year = entity.Year;

                db.Entry(selectedProperty).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                return true;
            }

            return false;
        }

        public async Task<IList<Property>> GetAllAsync(ParamsDTO paramsDto)
        {
            return await db.Properties.Where(x =>
                (!string.IsNullOrEmpty(paramsDto.Name)) ? x.Name.Contains(paramsDto.Name) : true
                && x.Price <= paramsDto.MaxPrice
                && x.Year <= paramsDto.MaxYear
                && x.Price >= paramsDto.MinPrice
                && x.Year >= paramsDto.MinYear
            )
                .Skip((paramsDto.Page - 1) * paramsDto.ItemsPerPage)
                .Take(paramsDto.ItemsPerPage)
                .ToListAsync();
        }

        public async Task<PaginationMetadataDTO> GetAllMetadataAsync(ParamsDTO paramsDto)
        {
            return new PaginationMetadataDTO(await db.Properties.Where(x =>
                (!string.IsNullOrEmpty(paramsDto.Name)) ? x.Name.Contains(paramsDto.Name) : true
                && x.Price <= paramsDto.MaxPrice
                && x.Views <= paramsDto.MaxViews
                && x.Year <= paramsDto.MaxYear
                && x.Price >= paramsDto.MinPrice
                && x.Views >= paramsDto.MinViews
                && x.Year >= paramsDto.MinYear
            ).CountAsync(), paramsDto.Page, paramsDto.ItemsPerPage);
        }

        public async Task<Property> GetByIdAsync(Guid entityId)
        {
            var selectedProperty = await db.Properties.Where(x => x.PropertyId == entityId).FirstOrDefaultAsync();
            return selectedProperty;
        }

        public async Task<bool> DeleteAsync(Guid entityId)
        {
            var selectedProperty = await db.Properties.Where(x => x.PropertyId == entityId).FirstOrDefaultAsync();

            if (selectedProperty != null)
            {
                db.Properties.Remove(selectedProperty);
                return true;
            }

            return false;
        }

        public async Task<int> saveAllChangesAsync()
        {
            return await db.SaveChangesAsync();
        }

        public Task<IList<Property>> GetAllByParentIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
