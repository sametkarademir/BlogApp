using System;
using System.Collections;

namespace Business.Services.Abstract;

public static class ResultMessages
{
    public static string GetError(string? objectId, string? collectionName, string? layerName, Exception? exception)
    {
        return $"[{objectId}] kodlu [{collectionName}] çağırma işleminde [{layerName}] katmanda hata oluştu. Hata: [{exception?.Message}] - Detay: [{exception?.StackTrace}]";
    }
    public static string GetSuccess(string? objectName, string? collectionName)
    {
        return $"[{objectName}] adlı [{collectionName}] görüntülendi.";
    }
    public static string GetAllError(string? collectionName, string? layerName, Exception? exception)
    {
        return $"[{collectionName}] listeleme işleminde [{layerName}] katmanda hata oluştu. Hata: [{exception?.Message}] - Detay: [{exception?.StackTrace}]";
    }
    public static string GetAllSuccess(string? collectionName)
    {
        return $"[{collectionName}] listesi görüntülendi.";
    }
    public static string AddError(string? objectName, string? collectionName, string? layerName, Exception? exception)
    {
        return $"[{objectName}] adlı [{collectionName}] ekleme işleminde [{layerName}] katmanda hata oluştu. Hata: [{exception?.Message}] - Detay: [{exception?.StackTrace}]";
    }
    public static string AddSuccess(string? objectName, string? collectionName)
    {
        return $"[{objectName}] adlı [{collectionName}] eklendi.";
    }
    public static string UpdateError(string? objectName, string? collectionName, string? layerName, Exception? exception)
    {
        return $"[{objectName}] adlı [{collectionName}] güncelleme işleminde [{layerName}] katmanda hata oluştu. Hata: [{exception?.Message}] - Detay: [{exception?.StackTrace}]";
    }
    public static string UpdateSuccess(string? objectName, string? collectionName)
    {
        return $"[{objectName}] adlı [{collectionName}] güncellendi.";
    }
    public static string DeleteError(string? objectId, string? collectionName, string? layerName, Exception? exception)
    {
        return $"[{objectId}] kodlu [{collectionName}] silme işleminde [{layerName}] katmanda hata oluştu. Hata: [{exception?.Message}] - Detay: [{exception?.StackTrace}]";
    }
    public static string DeleteSuccess(string? objectName, string? collectionName)
    {
        return $"[{objectName}] adlı [{collectionName}] silindi.";
    }
}

public static class ImageHelperMessages
{
    public static string UploadFolderSuccess(string? objectName, string? collectionName)
    {
        return $"[[{objectName}] adlı [{collectionName}] yüklendi.";
    }
    public static string UploadFolderError(string? objectName, string? collectionName, Exception? exception)
    {
        return $"[[{objectName}] adlı [{collectionName}] yükleme işleminde Helper katmanında hata oluştu. Hata: [{exception?.Message}] - Detay: [{exception?.StackTrace}]";
    }
    public static string UploadFolderNotFound(string? objectId, string? objectName, string? collectionName)
    {
        return $"[{objectId}] kodlu [{objectName}] adlı [{collectionName}] yükleme işleminde resim datası boş olduğundan yüklenemedi.";
    }
    public static string DeleteFolderSuccess(string? objectName, string? collectionName)
    {
        return $"[[{objectName}] adlı [{collectionName}] silindi.";
    }
    public static string DeleteFolderError(string? pictureName, string? collectionName, Exception? exception)
    {
        return $"[{pictureName}] url [{collectionName}] silme işleminde Helper katmanda hata oluştu. Hata: [{exception?.Message}] - Detay: [{exception?.StackTrace}]";
    }
}

public static class OperationMessages
{
    public static string Created() { return $"Kaydedildi.";}
    public static string ErrorMessage()
    {
        return "Beklenmedik bir hata oluştu.";
    }

    public static string ValidationMessage()
    {
        return "Girilen değerler eksik veya hatalı.";
    }
}