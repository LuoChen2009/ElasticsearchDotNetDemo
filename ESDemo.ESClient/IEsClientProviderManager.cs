namespace ESDemo.ESClient
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEsClientProviderManager
    {
        /// <summary>
        /// 获取客户端索引器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        NetEsClient this[string name] { get; }

        /// <summary>
        /// 添加provider
        /// </summary>
        /// <param name="name"></param>
        /// <param name="esUrl"></param>
        void AddEsClientProvider(string name, string esUrl);
    }
}
