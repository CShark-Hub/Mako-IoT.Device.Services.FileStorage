using MakoIoT.Device.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace MakoIoT.Device.Services.FileStorage.Extensions
{
    public static class DeviceBuilderExtension
    {
        public static IDeviceBuilder AddFileStorage(this IDeviceBuilder builder)
        {
            builder.Services.AddSingleton(typeof(IStorageService), typeof(FileStorageService));
            return builder;
        }
    }
}
