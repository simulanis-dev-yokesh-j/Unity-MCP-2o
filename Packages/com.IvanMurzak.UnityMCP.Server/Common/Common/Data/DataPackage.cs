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