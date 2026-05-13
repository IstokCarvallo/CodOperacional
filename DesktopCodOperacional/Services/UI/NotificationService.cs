using Notification.Wpf;

namespace DesktopCodOperacional.Services.UI
{
    public class NotificationService
    {
        private readonly NotificationManager _manager = new();

        public void Success(string message)
        {
            _manager.Show(new NotificationContent
                {
                    Title = "Éxito",
                    Message = message,
                    Type = NotificationType.Success
                });
        }

        public void Error(string message)
        {
            _manager.Show(new NotificationContent
                {
                    Title = "Error",
                    Message = message,
                    Type = NotificationType.Error
                });
        }

        public void Warning(string message)
        {
            _manager.Show(new NotificationContent
                {
                    Title = "Advertencia",
                    Message = message,
                    Type = NotificationType.Warning
                });
        }

        public void Info(string message)
        {
            _manager.Show(new NotificationContent
                {
                    Title = "Información",
                    Message = message,
                    Type = NotificationType.Information
                });
        }
    }
}
