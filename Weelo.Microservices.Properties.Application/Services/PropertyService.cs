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
    public class PropertyService : IBaseService<ParamsDTO, PaginationMetadataDTO, Property, Guid>
    {
        private readonly IBaseRepository<ParamsDTO, PaginationMetadataDTO, Property, Guid> propertyRepository;

        public PropertyService(IBaseRepository<ParamsDTO, PaginationMetadataDTO, Property, Guid> _propertyRepository)
        {
            propertyRepository = _propertyRepository;
        }
        public async Task<Property> AddAsync(Property entity)
        {
            if (entity == null)
                throw new ArgumentNullException("The 'Property' is required");

            var propertyResult = await propertyRepository.AddAsync(entity);
            await propertyRepository.saveAllChangesAsync();
            return propertyResult;
        }

        public async Task<bool> EditAsync(Property entity)
        {
            if (entity == null)
                throw new ArgumentNullException("The 'Property' is required to edit");

            await propertyRepository.EditAsync(entity);
            await propertyRepository.saveAllChangesAsync();

            return true;
        }

        public async Task<IList<Property>> GetAllAsync(ParamsDTO paramsDto)
        {
            return await propertyRepository.GetAllAsync(paramsDto);
        }

        public async Task<PaginationMetadataDTO> GetAllMetadataAsync(ParamsDTO paramsDto)
        {
            return await propertyRepository.GetAllMetadataAsync(paramsDto);
        }

        public async Task<Property> GetByIdAsync(Guid entityId)
        {
            return await propertyRepository.GetByIdAsync(entityId);
        }

        public async Task<bool> DeleteAsync(Guid entityId)
        {
            await propertyRepository.DeleteAsync(entityId);
            await propertyRepository.saveAllChangesAsync();

            return true;
        }
    }
}
