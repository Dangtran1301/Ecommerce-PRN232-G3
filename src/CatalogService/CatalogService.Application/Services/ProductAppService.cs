using AutoMapper;
using CatalogService.API.Errors;
using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Services.Interfaces;
using CatalogService.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.Application.Services
{
    public class ProductAppService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductAppService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ProductAppService(
            IProductRepository repository,
            IMapper mapper,
            ILogger<ProductAppService> logger,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return Result.Failure<ProductDto>(ProductErrors.NotFound);

            return Result.Ok(_mapper.Map<ProductDto>(entity));
        }

        public async Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            return Result.Ok(_mapper.Map<IReadOnlyList<ProductDto>>(entities));
        }

        public async Task<Result<ProductDto>> CreateAsync(ProductDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Product>(dto);
            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok(_mapper.Map<ProductDto>(entity));
        }

        public async Task<Result<ProductDto>> UpdateAsync(ProductDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(dto.Id, cancellationToken);
            if (entity == null)
                return Result.Failure<ProductDto>(ProductErrors.NotFound);

            _mapper.Map(dto, entity);
            await _repository.Update(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok(_mapper.Map<ProductDto>(entity));
        }





        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return Result.Fail(ProductErrors.NotFound(id));


            await _repository.Remove(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }

        public async Task<PagedResult<ProductDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _repository.GetPagedAsync(request, cancellationToken);
            return new PagedResult<ProductDto>(
                _mapper.Map<IReadOnlyList<ProductDto>>(result.Items),
                result.TotalCount,
                result.PageNumber,
                result.PageSize);
        }

        public IQueryable<Product> GetQueryable() => _repository.GetQueryable();
    }
}
