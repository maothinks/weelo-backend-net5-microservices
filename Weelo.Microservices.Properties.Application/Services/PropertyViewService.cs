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
    /// <summary>
    /// Service to process data with user cases and persist it in a repository
    /// </summary>
    public class PropertyViewService : IBaseService<ParamsDTO, PaginationMetadataDTO, PropertyView, Guid>
    {
        private readonly IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyView, Guid> propertyViewsRepository;

        public PropertyViewService(IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyView, Guid> _propertyViewsRepository)
        {
            propertyViewsRepository = _propertyViewsRepository;
        }
        public async Task<PropertyView> AddAsync(PropertyView entity)
        {
            if (entity == null)
                throw new ArgumentNullException("The 'Property Image' is required");

            var propertyResult = await propertyViewsRepository.AddAsync(entity);
            await propertyViewsRepository.saveAllChangesAsync();
            return propertyResult;
        }

        public async Task<bool> EditAsync(PropertyView entity)
        {
            if (entity == null)
                throw new ArgumentNullException("The 'Property' is required to edit");

            await propertyViewsRepository.EditAsync(entity);
            await propertyViewsRepository.saveAllChangesAsync();

            return true;
        }

        public async Task<IList<PropertyView>> GetAllAsync(ParamsDTO paramsDto)
        {
            return await propertyViewsRepository.GetAllAsync(paramsDto);
        }
        public async Task<IList<PropertyView>> GetAllByParentIdAsync(Guid propertyId)
        {
            return await propertyViewsRepository.GetAllByParentIdAsync(propertyId);
        }

        public async Task<PaginationMetadataDTO> GetAllMetadataAsync(ParamsDTO paramsDto)
        {
            return await propertyViewsRepository.GetAllMetadataAsync(paramsDto);
        }

        public async Task<PropertyView> GetByIdAsync(Guid entityId)
        {
            return await propertyViewsRepository.GetByIdAsync(entityId);
        }

        public async Task<bool> DeleteAsync(Guid entityId)
        {
            await propertyViewsRepository.DeleteAsync(entityId);
            await propertyViewsRepository.saveAllChangesAsync();

            return true;
        }
    }
}
