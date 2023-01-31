using Shared.Utilities.Extenstions.Interface;
using Shared.Utilities.Result;
using WebUI.Helpers.Interface;
using WebUI.Models;

namespace WebUI.Helpers.Concrete;

public class ImageHelper : IImageHelper
{
    private readonly IDateTimeExtensions _dateTimeExtensions;
    private readonly string _wwwroot;
    public static readonly string UploadImg = "Storage/Img";
    public static readonly string UploadContent = "Storage/Content";

    public ImageHelper(IWebHostEnvironment env, IDateTimeExtensions dateTimeExtensions)
    {
        _wwwroot = env.WebRootPath;
        _dateTimeExtensions = dateTimeExtensions;
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

    //public async Task<string> ShowReadContent(string folderPath)
    //{
    //    string content = "";
    //    await Task.Run(() =>
    //    {
    //        folderPath = $"{_wwwroot}/{folderPath}";
    //        FileStream fileStream = new FileStream(folderPath, FileMode.Open);
    //        using (StreamReader reader = new StreamReader(fileStream))
    //        {
    //            content = reader.ReadToEnd();
    //        }
    //    });
    //    return content;
    //}

    //public async Task<OperationResult<ImageResultObject>> CreateContentFolder(string content, string folderName, ServiceInputDto serviceInputDto)
    //{
    //    throw new NotImplementedException();
    //}

    public async Task<OperationResult<ImageResultObject>> UploadImgFolder(IFormFile imageFile, string folderName)
    {
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
        catch {

            return new OperationResult<ImageResultObject>
            {
                Data = new ImageResultObject
                {
                    FolderUrl = $"{UploadImg}/default.png", Size = "0", OldName = null
                },
                ExeptionStatus = ExeptionStatus.Error
            };
        }
    }
    
    public async Task<OperationResult<bool>> Delete(string folderUrl)
    {
        bool isSuccess = false;
        try
        {
            await Task.Run(() =>
            {
                var fileToDelete = Path.Combine($"{_wwwroot}/", folderUrl);
                if (File.Exists(fileToDelete))
                {
                    File.Delete(fileToDelete);
                }
            });
            return new OperationResult<bool> { Data = true, ExeptionStatus = ExeptionStatus.Success };
        }
        catch { return new OperationResult<bool> { Data = isSuccess, ExeptionStatus = ExeptionStatus.Error }; }
    }
}