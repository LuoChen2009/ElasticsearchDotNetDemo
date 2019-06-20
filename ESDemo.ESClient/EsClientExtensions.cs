using Microsoft.Extensions.DependencyInjection;
using System;

namespace ESDemo.ESClient
{
    /// <summary>
    /// ES客户端扩展类
    /// </summary>
    public static class EsClientExtensions
    {
        /// <summary>
        /// 注入服务扩展方法
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="name">不能clientprovider名称</param>
        /// <param name="esUrl">es服务地址</param>
        public static void AddEsClientProvider(this IServiceProvider provider, string name, string esUrl)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(esUrl))
                throw new Exception($"{nameof(name)}或{nameof(esUrl)}不能为空");

            var esProvider = provider.GetService<IEsClientProviderManager>();
            esProvider.AddEsClientProvider(name, esUrl);
        }
    }
}
