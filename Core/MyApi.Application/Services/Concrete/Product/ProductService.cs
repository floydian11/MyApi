using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.DTOs.Category;
using MyApi.Application.DTOs.Product;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Application.Results;
using MyApi.Application.Services.Abstract;
using MyApi.Application.Services.OuterServices.FileStorage;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Concrete
{    
    public class ProductService : ServiceBase<Product>, IProductService
    {
        private readonly IFileStorage _fileStorage;
        private readonly IRepository<Category> _categoryRepository;
        public ProductService(IProductRepository repository, IUnitOfWork unitOfWork,IMapper mapper, IFileStorage fileStorage, IRepository<Category> categoryRepository)
            : base(repository, unitOfWork, mapper)
        {
            _fileStorage = fileStorage;
            _categoryRepository = categoryRepository;
        }

        public async Task<IDataResult<List<ProductListDto>>> GetAllProductsAsync()
        {
            return await GetAllAsync<ProductListDto>();
        }

        public async Task<IDataResult<ProductListDto?>> GetProductByIdAsync(Guid id)
        {
            var result = await GetByIdAsync<ProductListDto>(id);
            if (!result.Success)
                return new ErrorDataResult<ProductListDto>(default!, "Ürün bulunamadı."); //burada default! yerine null da kullanılabilir.

            return result;
        }

        public async Task<IDataResult<ProductResponseDto>> AddProductAsync(ProductCreateDto dto)
        {
            // 1️⃣ DTO → Entity
            var entity = _mapper.Map<Product>(dto);

            // 2️⃣ ProductDetail
            entity.ProductDetail = new ProductDetail
            {
                Manufacturer = dto.Manufacturer,
                TechnicalSpecs = dto.TechnicalSpecs,
                WarrantyPeriodInMonths = dto.WarrantyPeriodInMonths
            };

            // 3️⃣ Kategoriler
            if (dto.CategoryIds.Any())
            {
                var categories = await _categoryRepository
                    .GetWhere(c => dto.CategoryIds.Contains(c.Id))
                    .ToListAsync();

                foreach (var category in categories)
                    entity.Categories.Add(category);
            }

            // 4️⃣ Tekil resim (opsiyonel)
            if (dto.ProductImage is not null)
            {
                var uploadItem = new FileUploadItem
                {
                    File = dto.ProductImage,
                    FileName = dto.ImageFileName ?? dto.ProductImage.FileName,
                    ContentType = dto.ProductImage.ContentType
                };

                var uploadResult = await _fileStorage.SaveFileAsync(uploadItem, "products/images");

                entity.ProductImagePath = uploadResult.FilePath;
            }

            // 5️⃣ Çoklu belgeler (opsiyonel)
            if (dto.Documents.Any())
            {
                var uploadItems = dto.Documents.Select(d => new FileUploadItem
                {
                    File = d,
                    FileName = d.FileName,
                    ContentType = d.ContentType
                }).ToList();

                var documentResults = await _fileStorage.SaveFilesAsync(uploadItems, "products/docs");

                foreach (var doc in documentResults)
                {
                    entity.ProductDocuments.Add(new ProductDocument
                    {
                        FilePath = doc.FilePath
                    });
                }
            }

            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            var response = _mapper.Map<ProductResponseDto>(entity);
            return new SuccessDataResult<ProductResponseDto>(response, "Ürün başarıyla eklendi.");
        }

        public async Task<IDataResult<ProductResponseDto?>> UpdateProductAsync(Guid id, ProductUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return new ErrorDataResult<ProductResponseDto?>(null, "Ürün bulunamadı.");

            // Nullable-safe update
            if (!string.IsNullOrEmpty(dto.Name))
                entity.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Description))
                entity.Description = dto.Description;

            if (dto.Price.HasValue)
                entity.Price = dto.Price.Value;

            if (dto.ReleaseDate.HasValue)
                entity.ReleaseDate = dto.ReleaseDate.Value;

            if (dto.IsActive.HasValue)
                entity.IsActive = dto.IsActive.Value;

            // ProductDetail update
            if (entity.ProductDetail == null)
                entity.ProductDetail = new ProductDetail();

            if (!string.IsNullOrEmpty(dto.Manufacturer))
                entity.ProductDetail.Manufacturer = dto.Manufacturer;

            if (!string.IsNullOrEmpty(dto.TechnicalSpecs))
                entity.ProductDetail.TechnicalSpecs = dto.TechnicalSpecs;

            if (dto.WarrantyPeriodInMonths.HasValue)
                entity.ProductDetail.WarrantyPeriodInMonths = dto.WarrantyPeriodInMonths.Value;

            // Kategoriler (varsa güncelle)
            if (dto.CategoryIds.Any())
            {
                var categories = await _categoryRepository
                    .GetWhere(c => dto.CategoryIds.Contains(c.Id))
                    .ToListAsync();
                entity.Categories.Clear();
                foreach (var category in categories)
                    entity.Categories.Add(category);
            }

            // ProductImage update
            if (dto.ProductImage != null)
            {
                if (!string.IsNullOrEmpty(entity.ProductImagePath))
                    await _fileStorage.DeleteFileAsync(entity.ProductImagePath);

                var uploadItem = new FileUploadItem
                {
                    File = dto.ProductImage,
                    FileName = dto.ImageFileName ?? dto.ProductImage.FileName,
                    ContentType = dto.ProductImage.ContentType
                };

                var result = await _fileStorage.SaveFileAsync(uploadItem, "products/images");
                entity.ProductImagePath = result.FilePath;
            }

            // Çoklu belgeler ekle (eski belgeler silinmez)
            if (dto.Documents != null && dto.Documents.Any())
            {
                var uploadItems = dto.Documents.Select(d => new FileUploadItem
                {
                    File = d,
                    FileName = d.FileName,
                    ContentType = d.ContentType
                }).ToList();

                var results = await _fileStorage.SaveFilesAsync(uploadItems, "products/docs");
                foreach (var doc in results)
                    entity.ProductDocuments.Add(new ProductDocument { FilePath = doc.FilePath });
            }

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();

            var response = _mapper.Map<ProductResponseDto>(entity);
            return new SuccessDataResult<ProductResponseDto?>(response, "Ürün güncellendi.");
        }



        public async Task<IDataResult<List<ProductListDto>>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            // IRepository.GetWhere kullanımı
              var list = await _repository
             .GetWhere(p => p.Categories.Any(c => c.Id == categoryId), tracking: false)
             .ProjectTo<ProductListDto>(_mapper.ConfigurationProvider)
             .ToListAsync();

            if (!list.Any())
                return new ErrorDataResult<List<ProductListDto>>(list, "Bu kategoriye ait ürün bulunamadı.");

            return new SuccessDataResult<List<ProductListDto>>(list, "Kategoriye göre ürünler listelendi.");
        }

        public async Task<IDataResult<List<ProductListDto>>> GetExpensiveProductsAsync(decimal minPrice)
        {
            // IRepository.GetWhere + ordering
            var list = await _repository
            .GetWhere(p => p.Price >= minPrice, tracking: false)
            .OrderByDescending(p => p.Price)
            .ProjectTo<ProductListDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

            if (!list.Any())
                return new ErrorDataResult<List<ProductListDto>>(list, "Belirtilen fiyatın üzerinde ürün bulunamadı.");

            return new SuccessDataResult<List<ProductListDto>>(list, "Pahalı ürünler listelendi.");
        }
    }
}
