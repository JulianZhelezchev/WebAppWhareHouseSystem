using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebAppWhareHouseSystem.Models;

namespace WebAppWhareHouseSystem.Services
{
    public interface IProductServices
    {
        Task<List<ProductViewModel>> GetAllAsync();
        Task CreateAsync(ProductViewModel product);
        Task DeleteAsync(int id);
        Task<bool> ExistAsync(int id);
        Task<ProductViewModel> GetByIdAsync(int id);
        Task UpdateAsync(int id, ProductViewModel product);
        Task<FileModel> UploadFileToFileSystem(IFormFile formfile);


    }

}
