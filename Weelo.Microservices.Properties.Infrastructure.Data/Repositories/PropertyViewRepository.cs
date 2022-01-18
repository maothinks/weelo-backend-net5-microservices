using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weelo.Microservices.Properties.Domain.DTOS;
using Weelo.Microservices.Properties.Domain.Interfaces.Repositories;
using Weelo.Microservices.Properties.Infrastructure.Data.Contexts;

namespace Weelo.Microservices.Properties.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Implements the CRUD operations for Property Views
    /// </summary>
    public class PropertyViewsRepository : IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyView, Guid>
    {
        private readonly PropertyContext db;

        public PropertyViewsRepository()
        {
            //db = _db;
            db = new PropertyContext();
        }
        public async Task<PropertyView> AddAsync(PropertyView entity)
        {
            entity.PropertyViewId = Guid.NewGuid();
            await db.PropertyViews.AddAsync(entity);
            return entity;
        }

        public async Task<bool> EditAsync(PropertyView entity)
        {
            var selectedPropertyViews = await db.PropertyViews.Where(x => x.PropertyViewId == entity.PropertyViewId).FirstOrDefaultAsync();

            if (selectedPropertyViews != null)
            {
                db.Entry(selectedPropertyViews).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                return true;
            }

            return false;
        }

        public async Task<IList<PropertyView>> GetAllAsync(ParamsDTO paramsDto)
        {
            return await db.PropertyViews
                .Skip((paramsDto.Page - 1) * paramsDto.ItemsPerPage)
                .Take(paramsDto.ItemsPerPage)
                .ToListAsync();
        }

        public async Task<IList<PropertyView>> GetByPropertyIdAsync(Guid id)
        {
            return await db.PropertyViews.Where(x => x.PropertyId == id)
                .ToListAsync();
        }

        public async Task<IList<PropertyView>> GetAllByParentIdAsync(Guid id)
        {
            return await db.PropertyViews.Where(x => x.PropertyId == id)
                .ToListAsync();
        }

        public async Task<PaginationMetadataDTO> GetAllMetadataAsync(ParamsDTO paramsDto)
        {
            return new PaginationMetadataDTO(await db.PropertyViews.CountAsync(), paramsDto.Page, paramsDto.ItemsPerPage);
        }

        public async Task<PropertyView> GetByIdAsync(Guid entityId)
        {
            var selectedProperty = await db.PropertyViews.Where(x => x.PropertyViewId == entityId).FirstOrDefaultAsync();
            return selectedProperty;
        }

        public async Task<bool> DeleteAsync(Guid entityId)
        {
            var selectedPropertyView = await db.PropertyViews.Where(x => x.PropertyViewId == entityId).FirstOrDefaultAsync();

            if (selectedPropertyView != null)
            {
                db.PropertyViews.Remove(selectedPropertyView);
                return true;
            }

            return false;
        }

        public async Task<int> saveAllChangesAsync()
        {
            return await db.SaveChangesAsync();
        }
    }
}
