#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.UnityMCP.Common.Data
{
    public class DataPackage : IDataPackage
    {
        public ICommandData? Command { get; set; }
        public INotificationData? Notification { get; set; }

        public void Dispose()
        {
            Command?.Dispose();
            Notification?.Dispose();
        }
        ~DataPackage() => Dispose();
    }
}