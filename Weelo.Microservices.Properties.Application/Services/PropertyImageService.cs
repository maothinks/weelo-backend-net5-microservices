using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weelo.Microservices.Properties.Application.Interfaces;
using Weelo.Microservices.Properties.Domain;
using Weelo.Microservices.Properties.Domain.DTOS;
using Weelo.Microservices.Properties.Domain.Interfaces;
using Weelo.Microservices.Properties.Domain.Interfaces.Repositories;

namespace Weelo.Microservices.Properties.Application.Services
{
    public class PropertyImageService : IBaseService<ParamsDTO, PaginationMetadataDTO, PropertyImage, Guid>
    {
        private readonly IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyImage, Guid> propertyImageRepository;

        public PropertyImageService(IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyImage, Guid> _propertyImageRepository)
        {
            propertyImageRepository = _propertyImageRepository;
        }
        public async Task<PropertyImage> AddAsync(PropertyImage entity)
        {
            if (entity == null)
                throw new ArgumentNullException("The 'Property Image' is required");

            var propertyResult = await propertyImageRepository.AddAsync(entity);
            await propertyImageRepository.saveAllChangesAsync();
            return propertyResult;
        }

        public async Task<bool> EditAsync(PropertyImage entity)
        {
            if (entity == null)
                throw new ArgumentNullException("The 'Property' is required to edit");

            await propertyImageRepository.EditAsync(entity);
            await propertyImageRepository.saveAllChangesAsync();

            return true;
        }

        public async Task<IList<PropertyImage>> GetAllAsync(ParamsDTO paramsDto)
        {
            return await propertyImageRepository.GetAllAsync(paramsDto);
        }
        public async Task<IList<PropertyImage>> GetAllByPropertyIdAsync(Guid propertyId)
        {
            return await propertyImageRepository.GetAllByParentIdAsync(propertyId);
        }


        public async Task<PaginationMetadataDTO> GetAllMetadataAsync(ParamsDTO paramsDto)
        {
            return await propertyImageRepository.GetAllMetadataAsync(paramsDto);
        }

        public async Task<PropertyImage> GetByIdAsync(Guid entityId)
        {
            return await propertyImageRepository.GetByIdAsync(entityId);
        }

        public async Task<bool> DeleteAsync(Guid entityId)
        {
            await propertyImageRepository.DeleteAsync(entityId);
            await propertyImageRepository.saveAllChangesAsync();

            return true;
        }
    }
}
