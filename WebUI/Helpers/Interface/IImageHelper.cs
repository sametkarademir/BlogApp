using Shared.Entities.Abstract;
using Shared.Utilities.Result;
using WebUI.Models;

namespace WebUI.Helpers.Interface;

public interface IImageHelper
{
    Task<string> ShowReadContent(string folderPath);
    Task<OperationResult<ImageResultObject>> CreateContentFolder(string content, string folderName, string objectId, string objectName, ServiceInputDto serviceInputDto);
    Task<OperationResult<ImageResultObject>> UploadImgFolder(IFormFile imageFile, string folderName, string objectId, string objectName, ServiceInputDto serviceInputDto);
    Task<OperationResult<bool>> Delete(string folderUrl, ServiceInputDto serviceInputDto);
}