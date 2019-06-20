using System.Collections.Concurrent;

namespace ESDemo.ESClient
{
    /// <summary>
    /// 客户端Provider管理类
    /// </summary>
    public class EsClientProviderManager : IEsClientProviderManager
    {
        /// <summary>
        /// 客户端Provider池
        /// </summary>
        private readonly ConcurrentDictionary<string, EsClientProvider> esClientProviderPool;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EsClientProviderManager()
        {
            this.esClientProviderPool = new ConcurrentDictionary<string, EsClientProvider>();
        }

        /// <summary>
        ///  获取es客户端索引器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NetEsClient this[string name]
        {
            get { return esClientProviderPool[name].GetNetEsClient(); }
        }

        /// <summary>
        /// 添加客户端Provider
        /// </summary>
        /// <param name="name">Provider名称</param>
        /// <param name="esUrl">es服务地址</param>
        public void AddEsClientProvider(string name, string esUrl)
        {
            esClientProviderPool.TryAdd(name, new EsClientProvider(esUrl));
        }
    }
}
