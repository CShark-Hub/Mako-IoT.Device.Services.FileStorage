using MakoIoT.Device.Services.DependencyInjection;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Device.Services.FileStorage.Extensions
{
    public static class DeviceBuilderExtension
    {
        public static IDeviceBuilder AddFileStorage(this IDeviceBuilder builder)
        {
            DI.RegisterSingleton(typeof(IStorageService), typeof(FileStorageService));
            return builder;
        }
    }
}
