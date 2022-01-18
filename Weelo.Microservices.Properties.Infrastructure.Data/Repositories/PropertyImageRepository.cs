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
    /// Implements the CRUD operations for Property Image
    /// </summary>
    public class PropertyImageRepository : IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyImage, Guid>
    {
        private readonly PropertyContext db;

        public PropertyImageRepository()
        {
            //db = _db;
            db = new PropertyContext();
        }
        public async Task<PropertyImage> AddAsync(PropertyImage entity)
        {
            entity.PropertyImageId = Guid.NewGuid();
            await db.PropertyImages.AddAsync(entity);
            return entity;
        }

        public async Task<bool> EditAsync(PropertyImage entity)
        {
            var selectedPropertyImage = await db.PropertyImages.Where(x => x.PropertyImageId == entity.PropertyImageId).FirstOrDefaultAsync();

            if (selectedPropertyImage != null)
            {
                selectedPropertyImage.Enabled = entity.Enabled;
                selectedPropertyImage.FilePath = entity.FilePath;
                selectedPropertyImage.PropertyId = entity.PropertyId;

                db.Entry(selectedPropertyImage).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                return true;
            }

            return false;
        }

        public async Task<IList<PropertyImage>> GetAllAsync(ParamsDTO paramsDto)
        {
            return await db.PropertyImages
                .Skip((paramsDto.Page - 1) * paramsDto.ItemsPerPage)
                .Take(paramsDto.ItemsPerPage)
                .ToListAsync();
        }

        public async Task<IList<PropertyImage>> GetAllByParentIdAsync(Guid id)
        {
            return await db.PropertyImages.Where(x => x.PropertyId == id)
                .ToListAsync();
        }

        public async Task<PaginationMetadataDTO> GetAllMetadataAsync(ParamsDTO paramsDto)
        {
            return new PaginationMetadataDTO(await db.PropertyImages.CountAsync(), paramsDto.Page, paramsDto.ItemsPerPage);
        }

        public async Task<PropertyImage> GetByIdAsync(Guid entityId)
        {
            var selectedProperty = await db.PropertyImages.Where(x => x.PropertyImageId == entityId).FirstOrDefaultAsync();
            return selectedProperty;
        }

        public async Task<bool> DeleteAsync(Guid entityId)
        {
            var selectedPropertyImage = await db.PropertyImages.Where(x => x.PropertyImageId == entityId).FirstOrDefaultAsync();

            if (selectedPropertyImage != null)
            {
                db.PropertyImages.Remove(selectedPropertyImage);
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
