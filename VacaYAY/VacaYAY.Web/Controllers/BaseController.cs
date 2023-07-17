using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Web.Controllers;

public class BaseController : Controller
{
    private readonly INotyfService _toaster;

    public BaseController(INotyfService toaster)
    {
        _toaster = toaster;
    }

    protected void HandleModelErrors(List<CustomValidationResult> errors)
    {
        foreach (var error in errors)
        {
            ModelState.AddModelError(error.Property, error.Text);
        }
    }

    protected void HandleErrors(List<CustomValidationResult> errors)
    {
        foreach (var error in errors)
        {
            Notification(error.Text, NotificationType.Error);
        }
    }

    protected void Notification(string message, NotificationType type = NotificationType.Success)
    {
        switch (type)
        {
            case NotificationType.Info:
                _toaster.Information(message);
                break;
            case NotificationType.Warning:
                _toaster.Warning(message);
                break;
            case NotificationType.Success:
                _toaster.Success(message);
                break;
            case NotificationType.Error:
                _toaster.Error(message);
                break;
        };
    }
}
