using Elasticsearch.Net;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using System;

namespace ESDemo.ESClient
{
    /// <summary>
    /// 
    /// </summary>
    public class EsClientProvider
    {
        /// <summary>
        /// esclient
        /// </summary>
        private readonly NetEsClient netEsClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="esUrl"></param>
        public EsClientProvider(string esUrl)
        {
            var connectionPool = new SingleNodeConnectionPool(new Uri(esUrl));
            var settings = new ConnectionSettings(connectionPool, (builtInSerializer, connectionSettings) =>
            new JsonNetSerializer(builtInSerializer, connectionSettings, () =>
              new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
            ));

            netEsClient = new NetEsClient(settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public NetEsClient GetNetEsClient()
        {
            return this.netEsClient;
        }
    }
}
