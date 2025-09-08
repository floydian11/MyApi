using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.DTOs.Category;
using MyApi.Application.DTOs.Pagination;
using MyApi.Application.DTOs.Product;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Application.Results;
using MyApi.Application.Services.Abstract;
using MyApi.Application.Services.OuterServices.FileStorage;
using MyApi.Application.Utilities;
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

        public async Task<IDataResult<List<ProductListDto?>>> GetAllProductsAsync()
        {
            try
            {
                var dtos = await _repository.GetAllAsNoTracking() // IQueryable<Product>
                    .ProjectTo<ProductListDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new SuccessDataResult<List<ProductListDto?>>(
                            dtos.Cast<ProductListDto?>().ToList(), "Ürünler başarıyla listelendi.");

            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<ProductListDto?>>(null, "Ürünler listelenemedi: " + ex.Message);
            }
        }

        public Task<IDataResult<PagedResultDto<ProductListDto>>> GetPagedProductsAsync(PaginationRequestDto request)
        {
            return ServiceExecutor.ExecuteAsync(async () =>
            {
                var query = _repository.GetAll();

                var totalCount = await query.CountAsync();
                var items = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ProjectTo<ProductListDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new PagedResultDto<ProductListDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize
                };

            }, "Ürünler başarıyla listelendi", "Ürünler listelenemedi");
        }

        public async Task<IDataResult<PagedResultDto<ProductListDto>>> GetProductsFilteredAsync(ProductFilterDto filter)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // Sayfa ve sayfa boyutu güvenliği
                var page = filter.Page < 1 ? 1 : filter.Page;
                var pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;

                // Başlangıç query
                var query = _repository.GetWhere(p => true);

                query = query.Include(p => p.Categories)
                             .Include(p => p.ProductDocuments);

                // Dinamik filtreleme
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    query = query.Where(p => p.Name.Contains(filter.Name));

                if (filter.MinPrice.HasValue)
                    query = query.Where(p => p.Price >= filter.MinPrice.Value);

                if (filter.MaxPrice.HasValue)
                    query = query.Where(p => p.Price <= filter.MaxPrice.Value);

                if (filter.CategoryIds != null && filter.CategoryIds.Any())
                    query = query.Where(p => p.Categories.Any(c => filter.CategoryIds.Contains(c.Id)));

                if (filter.IsActive.HasValue)
                    query = query.Where(p => p.IsActive == filter.IsActive.Value);

                // Toplam kayıt sayısı
                var totalCount = await query.CountAsync();

                // Sayfalama
                var items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<ProductListDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                // Boş liste yerine her zaman liste dön
                items ??= new List<ProductListDto>();

                return new PagedResultDto<ProductListDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize
                };
            },
            successMessage: "Ürünler başarıyla listelendi.",
            errorMessage: "Ürünler getirilemedi.");
        }

        public async Task<IDataResult<ProductListDto?>> GetProductByIdAsync(Guid id)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                var entity = await _repository.GetWhere(p => p.Id == id)
                              .Include(p => p.Categories)
                              .Include(p => p.ProductDocuments)
                              .FirstOrDefaultAsync();

                if (entity == null)
                    return null; // ErrorDataResult içinden null dönecek

                return _mapper.Map<ProductListDto>(entity);
            },
            successMessage: "Ürün başarıyla getirildi.",
            errorMessage: "Ürün bulunamadı veya getirilemedi.");
        }

        public async Task<IDataResult<PagedResultDto<ProductListDto>>> SearchProductsAsync(ProductSearchDto filter)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                var query = _repository.GetWhere(p => true).AsQueryable();
                query = query.Include(p => p.Categories)
                             .Include(p => p.ProductDocuments);

                // Anahtar kelime ile arama
                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    query = query.Where(p => p.Name.Contains(filter.SearchTerm)
                                          || (p.Description != null && p.Description.Contains(filter.SearchTerm)));
                }

                // Toplam kayıt
                var totalCount = await query.CountAsync();

                // Sayfalama
                var items = await query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ProjectTo<ProductListDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new PagedResultDto<ProductListDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    Page = filter.Page,
                    PageSize = filter.PageSize
                };
            },
            successMessage: "Arama sonuçları başarıyla getirildi.",
            errorMessage: "Ürünler bulunamadı veya getirilemedi.");
        }

        public async Task<IDataResult<ProductResponseDto?>> AddProductAsync(ProductCreateDto dto)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // DTO → Entity
                var entity = _mapper.Map<Product>(dto);

                // ProductDetail
                entity.ProductDetail = new ProductDetail
                {
                    Manufacturer = dto.Manufacturer,
                    TechnicalSpecs = dto.TechnicalSpecs,
                    WarrantyPeriodInMonths = dto.WarrantyPeriodInMonths
                };

                // Kategoriler
                if (dto.CategoryIds.Any())
                {
                    var categories = await _categoryRepository
                        .GetWhere(c => dto.CategoryIds.Contains(c.Id))
                        .ToListAsync();

                    foreach (var category in categories)
                        entity.Categories.Add(category);
                }

                // Resim
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

                // Belgeler
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
                            FilePath = doc.FilePath,
                            FileName = doc.FileName
                        });
                    }
                }

                await _repository.AddAsync(entity);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<ProductResponseDto>(entity);
            },
            successMessage: "Ürün başarıyla eklendi.",
            errorMessage: "Ürün eklenemedi.");
        }

        public async Task<IDataResult<ProductResponseDto?>> UpdateProductAsync(ProductUpdateDto dto)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                var entity = await _repository.GetWhere(p => p.Id == dto.Id)
                                              .Include(p => p.ProductDetail)
                                              .Include(p => p.Categories)
                                              .Include(p => p.ProductDocuments)
                                              .FirstOrDefaultAsync();

                if (entity == null)
                    throw new Exception("Güncellenecek ürün bulunamadı.");

                // Primitive alanlar
                _mapper.Map(dto, entity);

                // ProductDetail manuel (varsa güncelle, yoksa oluştur)
                if (entity.ProductDetail != null)
                {
                    entity.ProductDetail.Manufacturer = dto.Manufacturer ?? entity.ProductDetail.Manufacturer;
                    entity.ProductDetail.TechnicalSpecs = dto.TechnicalSpecs ?? entity.ProductDetail.TechnicalSpecs;
                    entity.ProductDetail.WarrantyPeriodInMonths = dto.WarrantyPeriodInMonths ?? entity.ProductDetail.WarrantyPeriodInMonths;
                }
                else
                {
                    entity.ProductDetail = new ProductDetail
                    {
                        Manufacturer = dto.Manufacturer,
                        TechnicalSpecs = dto.TechnicalSpecs,
                        WarrantyPeriodInMonths = dto.WarrantyPeriodInMonths ?? 0
                    };
                }

                // Kategoriler manuel
                if (dto.CategoryIds.Any())
                {
                    var categories = await _categoryRepository
                        .GetWhere(c => dto.CategoryIds.Contains(c.Id))
                        .ToListAsync();
                    entity.Categories.Clear();
                    foreach (var category in categories)
                        entity.Categories.Add(category);
                }

                // Resim güncelleme
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

                // Belgeler manuel ekleme
                if (dto.Documents.Any())
                {
                    var uploadItems = dto.Documents.Select(d => new FileUploadItem
                    {
                        File = d,
                        FileName = d.FileName,
                        ContentType = d.ContentType
                    }).ToList();

                    var documentResults = await _fileStorage.SaveFilesAsync(uploadItems, "products/docs");

                    // Var olan belgeleri silme veya güncelleme
                    entity.ProductDocuments.Clear();
                    foreach (var doc in documentResults)
                    {
                        entity.ProductDocuments.Add(new ProductDocument
                        {
                            FileName = doc.FileName, // artık null olmayacak
                            FilePath = doc.FilePath
                        });
                    }
                }

                await _unitOfWork.CommitAsync();

                return _mapper.Map<ProductResponseDto>(entity);
            },
            successMessage: "Ürün başarıyla güncellendi.",
            errorMessage: "Ürün güncellenemedi.");
        }

        public async Task<IDataResult<bool>> DeleteProductAsync(Guid id)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                var entity = await _repository.GetWhere(p => p.Id == id)
                    .Include(p => p.ProductDetail)
                    .Include(p => p.ProductDocuments)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return false;

                // 1️⃣ ProductDocuments dosyalarını sil
                foreach (var doc in entity.ProductDocuments)
                {
                    await _fileStorage.DeleteFileAsync(doc.FilePath);
                }

                // 2️⃣ Ürün resmi varsa sil
                if (!string.IsNullOrEmpty(entity.ProductImagePath))
                {
                    await _fileStorage.DeleteFileAsync(entity.ProductImagePath);
                }

                // 3️⃣ Ürünü DB’den sil (ProductDetail ve ProductDocuments cascade ile silinir)
                await _repository.DeleteAsync(entity);
                await _unitOfWork.CommitAsync();

                return true;
            },
            successMessage: "Ürün başarıyla silindi.",
            errorMessage: "Ürün bulunamadı veya silinemedi.");
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
