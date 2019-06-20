using ESDemo.ESClient;
using ESDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QAController : ControllerBase
    {
        /// <summary>
        /// es client
        /// </summary>
        private readonly NetEsClient netEsClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QAController(IEsClientProviderManager esClientProviderManager)
        {
            this.netEsClient = esClientProviderManager["esclient"];
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CreateIndex(string indexName)
        {
            return Ok(await CreateIndexAsync(indexName));
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DeleteIndex(string indexName)
        {
            return Ok(await DeleteIndexAsync(indexName));
        }

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> Add(AddQADto request)
        {
            var result = await AddAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> Update(UpdateQADto request)
        {
            return Ok(await UpdateAsync(request.Id, request.Answer));
        }

        /// <summary>
        /// 获取指定文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var result = await GetAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> Query(QueryQADto request)
        {
            var result = await QueryAsync(request.Keyword);
            return Ok(result);
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await DeleteAsync(id));
        }

        /// <summary>
        /// 更加id获取指定文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<QAEntity_ES> GetAsync(string id)
        {
            var result = await netEsClient.GetAsync<QAEntity_ES>(DocumentPath<QAEntity_ES>.Id(id), x => { return x.Index("qaindex"); });
            return result?.Source;
        }

        #region private method
        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="id"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        private async Task<bool> UpdateAsync(string id, string answer)
        {
            var entity_es = await GetAsync(id);
            if (entity_es != null)
            {
                entity_es.Answer = answer;
                entity_es.UpdateDate = DateTime.Now;
                //var result = await netEsClient.IndexAsync<QAEntity_ES>(entity_es, x => { return x.Index("qaindex"); });
                var result = await netEsClient.UpdateAsync(DocumentPath<QAEntity_ES>.Id(id), d => d.Index("qaindex").RetryOnConflict(3).Doc(entity_es));
                return result.IsValid;
            }

            return false;
        }

        /// <summary>
        /// 查询文档 
        /// </summary>
        /// <returns></returns>
        private async Task<List<QAEntity_ES>> QueryAsync(string keyword)
        {
            var result = await netEsClient.SearchAsync<QAEntity_ES>(x => x.Index("qaindex").From(0).Size(10).Query(q => q.Match(m => m.Field
                 (f => f.Question).Query(keyword))));
            if (result != null && result.Hits != null)
            {
                return result.Hits.Where(x => x.Score > 0.5).Select(x => x.Source).ToList();
            }

            return null;
        }

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<bool> AddAsync(AddQADto request)
        {
            QAEntity_ES entity = new QAEntity_ES
            {
                Id = Guid.NewGuid().ToString("N"),
                Question = request.Question,
                Answer = request.Answer,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
            var result = await netEsClient.IndexAsync<QAEntity_ES>(entity, x => { return x.Index("qaindex"); });
            if (result != null && result.IsValid)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<bool> DeleteAsync(string id)
        {
            var result = await netEsClient.DeleteAsync<QAEntity_ES>(DocumentPath<QAEntity_ES>.Id(id), d => d.Index("qaindex"));
            return result.IsValid;
        }
        #endregion

        #region index
        /// <summary>
        /// 创建索引 
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        private async Task<bool> CreateIndexAsync(string indexName)
        {
            if (!netEsClient.IndexExists(indexName).Exists)
            {
                IIndexState indexState = new IndexState
                {
                    Settings = new IndexSettings
                    {
                        NumberOfShards = 1,
                        NumberOfReplicas = 0
                    }
                };

                var result = await netEsClient.CreateIndexAsync(indexName,
                    c => c.InitializeUsing(indexState)
                    .Mappings(m => m.Map<QAEntity_ES>(mq => mq.AutoMap())));

                return result.IsValid;
            }

            return true;
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        private async Task<bool> DeleteIndexAsync(string indexName)
        {
            if (string.IsNullOrWhiteSpace(indexName))
                return false;
            var result = await netEsClient.DeleteIndexAsync(Indices.Index(indexName));

            return result.IsValid;
        }
        #endregion

    }
}