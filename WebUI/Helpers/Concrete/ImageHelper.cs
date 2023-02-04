using Business.Dtos;
using Business.Services.Interface;
using Entities.Concrete;
using Shared.Entities.Abstract;
using Shared.Utilities.Extenstions.Interface;
using Shared.Utilities.Result;
using System.IO;
using WebUI.Helpers.Interface;
using WebUI.Models;

namespace WebUI.Helpers.Concrete;

public class ImageHelper : IImageHelper
{
    private readonly IDateTimeExtensions _dateTimeExtensions;
    private readonly IFolderService _folderService;
    private readonly ISystemLogService _systemLogService;
    private readonly string _wwwroot;
    public static readonly string UploadImg = "Storage/Img";
    public static readonly string UploadContent = "Storage/Content";

    public ImageHelper(IWebHostEnvironment env, IDateTimeExtensions dateTimeExtensions, ISystemLogService systemLogService, IFolderService folderService)
    {
        _wwwroot = env.WebRootPath;
        _dateTimeExtensions = dateTimeExtensions;
        _systemLogService = systemLogService;
        _folderService = folderService;
    }

    public string StringReplace(string text)
    {
        text = text.Replace("İ", "I");
        text = text.Replace("ı", "i");
        text = text.Replace("Ğ", "G");
        text = text.Replace("ğ", "g");
        text = text.Replace("Ö", "O");
        text = text.Replace("ö", "o");
        text = text.Replace("Ü", "U");
        text = text.Replace("ü", "u");
        text = text.Replace("Ş", "S");
        text = text.Replace("ş", "s");
        text = text.Replace("Ç", "C");
        text = text.Replace("ç", "c");
        text = text.Replace(" ", "_");
        return text;
    }

    public async Task<string> ShowReadContent(string folderPath)
    {
        string content = "";
        if (!string.IsNullOrEmpty(folderPath))
        {
            await Task.Run(() =>
            {
                folderPath = $"{_wwwroot}/{folderPath}";
                FileStream fileStream = new FileStream(folderPath, FileMode.Open);
                using StreamReader reader = new StreamReader(fileStream);
                content = reader.ReadToEnd();
            });
        }
        return content;
    }

    public async Task<OperationResult<ImageResultObject>> CreateContentFolder(string content, string folderName, string objectId, string objectName, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ImageResultObject> { Data = new ImageResultObject(), ExeptionStatus = ExeptionStatus.Error };
        try
        {
            #region FolderOperation
            if (!Directory.Exists($"{_wwwroot}/{UploadContent}")) { Directory.CreateDirectory($"{_wwwroot}/{UploadContent}"); }
            string fileExtension = ".txt";
            string file = $"{folderName}_{_dateTimeExtensions.ToUnixTime(DateTime.UtcNow)}{fileExtension}";
            string fileName = StringReplace(file);
            var path = Path.Combine($"{_wwwroot}/{UploadContent}", fileName);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(content);
            }
            #endregion
            #region CreateFolder
            var folderResult = await _folderService.CreateAsync(new FolderAddDto
            {
                ObjectName = objectName,
                ObjectId = objectId,
                Extension = "txt",
                Name = folderName,
                OldName = "",
                Size = "",
                Url = $"{UploadContent}/{fileName}"
            }, serviceInputDto);
            if (folderResult.ExeptionStatus != ExeptionStatus.Success)
            {
                await Delete($"{UploadContent}/{fileName}", serviceInputDto); throw new ArgumentException($"{folderResult.Exception?.Message} {folderResult.Exception?.StackTrace}");
            }
            #endregion
            return new OperationResult<ImageResultObject>
            {
                Data = new ImageResultObject
                {
                    FolderUrl = $"{UploadContent}/{fileName}",
                    Size = "",
                    OldName = ""
                },
                ExeptionStatus = ExeptionStatus.Success
            };
        }
        catch (Exception e)
        {
            await _systemLogService.CreateAsync(new SystemLogAddDto
            {

                Date = DateTime.UtcNow,
                LogStatus = LogStatus.Error,
                Message = $"Message : {e.Message} - Detail : {e.StackTrace}",
                Method = serviceInputDto.RemoteAction,
                Action = serviceInputDto.RemoteController,
                RemoteAddress = serviceInputDto.RemoteAddress,
                RemotePort = serviceInputDto.RemotePort,
                Username = serviceInputDto.Username
            });
        }
        return res;
    }

    public async Task<OperationResult<ImageResultObject>> UploadImgFolder(IFormFile imageFile, string folderName, string objectId, string objectName, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ImageResultObject> { Data = new ImageResultObject(), ExeptionStatus = ExeptionStatus.Error };
        try
        {
            #region FolderOperation
            if (!Directory.Exists($"{_wwwroot}/{UploadImg}")) { Directory.CreateDirectory($"{_wwwroot}/{UploadImg}"); }
            string fileExtension = Path.GetExtension(imageFile.FileName);
            string file = $"{folderName}_{_dateTimeExtensions.ToUnixTime(DateTime.UtcNow)}{fileExtension}";
            string fileName = StringReplace(file);
            var path = Path.Combine($"{_wwwroot}/{UploadImg}", fileName);
            await using (var stream = new FileStream(path, FileMode.Create)) { await imageFile.CopyToAsync(stream); }
            #endregion
            #region CreateFolder
            var folderResult = await _folderService.CreateAsync(new FolderAddDto
            {
                ObjectName = objectName,
                ObjectId = objectId,
                Extension = Path.GetExtension(imageFile.FileName),
                Name = folderName,
                OldName = imageFile.FileName,
                Size = imageFile.Length.ToString(),
                Url = $"{UploadImg}/{fileName}"
            }, serviceInputDto);
            if (folderResult.ExeptionStatus != ExeptionStatus.Success) { await Delete($"{UploadImg}/{fileName}", serviceInputDto); throw new ArgumentException($"{folderResult.Exception?.Message} {folderResult.Exception?.StackTrace}"); }
            #endregion
            return new OperationResult<ImageResultObject>
            {
                Data = new ImageResultObject
                {
                    FolderUrl = $"{UploadImg}/{fileName}",
                    Size = imageFile.Length.ToString(),
                    OldName = imageFile.FileName
                },
                ExeptionStatus = ExeptionStatus.Success
            };
        }
        catch (Exception e)
        {
            await _systemLogService.CreateAsync(new SystemLogAddDto
            {

                Date = DateTime.UtcNow,
                LogStatus = LogStatus.Error,
                Message = $"Message : {e.Message} - Detail : {e.StackTrace}",
                Method = serviceInputDto.RemoteAction,
                Action = serviceInputDto.RemoteController,
                RemoteAddress = serviceInputDto.RemoteAddress,
                RemotePort = serviceInputDto.RemotePort,
                Username = serviceInputDto.Username
            });
        }
        return res;
    }

    public async Task<OperationResult<bool>> Delete(string folderUrl, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<bool> { Data = false, ExeptionStatus = ExeptionStatus.Error };
        bool isSuccess = false;
        try
        {
            await Task.Run(() =>
            {
                var fileToDelete = Path.Combine($"{_wwwroot}/", folderUrl);
                if (File.Exists(fileToDelete)) { File.Delete(fileToDelete); }
            });
            res.Data = true;
            res.ExeptionStatus = ExeptionStatus.Success;
            await _folderService.DeleteByUrlAsync(folderUrl, serviceInputDto);
        }
        catch (Exception e)
        {
            await _systemLogService.CreateAsync(new SystemLogAddDto
            {
                Date = DateTime.UtcNow,
                LogStatus = LogStatus.Error,
                Message = $"Message : {e.Message} - Detail : {e.StackTrace}",
                Method = serviceInputDto.RemoteAction,
                Action = serviceInputDto.RemoteController,
                RemoteAddress = serviceInputDto.RemoteAddress,
                RemotePort = serviceInputDto.RemotePort,
                Username = serviceInputDto.Username
            });
        }
        return res;
    }
}