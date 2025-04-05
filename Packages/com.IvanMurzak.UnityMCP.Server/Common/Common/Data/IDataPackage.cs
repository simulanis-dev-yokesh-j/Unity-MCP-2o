using System;

namespace com.IvanMurzak.UnityMCP.Common.Data
{
    public interface IDataPackage : IDisposable
    {
        ICommandData? Command { get; set; }
        INotificationData? Notification { get; set; }
    }
}