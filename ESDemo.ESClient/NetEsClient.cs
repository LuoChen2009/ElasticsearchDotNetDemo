using Nest;

namespace ESDemo.ESClient
{
    /// <summary>
    /// net版本ElasticClient
    /// </summary>
    public class NetEsClient : ElasticClient
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionSettings"></param>
        public NetEsClient(IConnectionSettingsValues connectionSettings) : base(connectionSettings)
        { }
    }
}
