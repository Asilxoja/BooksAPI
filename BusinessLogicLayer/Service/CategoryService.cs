using AutoMapper;
using BooksAPI.DTOs.CategoryDtos;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer.Service;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork,
                           IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddCategoryAsync(AddCategoryDto newCategory)
    {
        var list = await _unitOfWork.CategoryInterface.GetAllAsync();
        var category = _mapper.Map<Category>(newCategory);
        if (newCategory is null)
        {
            throw new CategoryException("Category was null!");
        }

        if (string.IsNullOrEmpty(newCategory.Name))
        {
            throw new CategoryException("Category name is required!");
        }

        if (list.Any(c => c.Name == newCategory.Name))
        {
            throw new CategoryException($"{newCategory.Name} name is already exist!");
        }

        //if (!category.IsValid())
        //{
        //    throw new CategoryException("Category name is required!");
        //}   
        //if (!category.IsUnique(list))
        //{
        //    throw new CategoryException($"{category.Name} name is already exist!");
        //}

        await _unitOfWork.CategoryInterface.AddAsync(category);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        _unitOfWork.CategoryInterface.Delete(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var list = await _unitOfWork.CategoryInterface.GetAllAsync();
        return list.Select(c => _mapper.Map<CategoryDto>(c)).ToList();
    }

    public async Task<List<CategoryDto>> GetCategoriesWithBooksAsync()
    {
        var list = await _unitOfWork.CategoryInterface.GetAllCategoriesWithBooksAsync();
        return list.Select(c => _mapper.Map<CategoryDto>(c)).ToList();
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.CategoryInterface.GetByIdAsync(id);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<PagedList<CategoryDto>> GetPagedCategories(int pageSize, 
                                                                 int pageNumber)
    {
        var list = await _unitOfWork.CategoryInterface.GetAllAsync();
        var dtos = list.Select(c => _mapper.Map<CategoryDto>(c))
                       .ToList();

        PagedList<CategoryDto> pagedList = new(dtos, 
                                               dtos.Count(), 
                                               pageNumber, 
                                               pageSize);
        return pagedList.ToPagedList(dtos, pageSize, pageNumber);
    }

    public async Task UpdateCategoryAsync(CategoryDto categoryDto)
    {
        if (categoryDto is null)
        {
            throw new CategoryException("Category was null!");
        }

        if (string.IsNullOrEmpty(categoryDto.Name))
        {
            throw new CategoryException("Category name is required!");
        }
        var category = _mapper.Map<Category>(categoryDto);
        //var result = _validator.Validate(category);
        //if (!result.IsValid)
        //{
        //    throw new CategoryException(string.Join("\n", result.Errors));
        //}

        var list = await _unitOfWork.CategoryInterface.GetAllAsync();
        if (list.Any(c => c.Name == categoryDto.Name && c.Id != categoryDto.Id))
        {
            throw new CategoryException($"{categoryDto.Name} name is already exist!");
        }

        _unitOfWork.CategoryInterface.Update(category);
        await _unitOfWork.SaveAsync();
    }
}