using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;

namespace ESDemo.Models
{
    public class QAEntity_ES
    {
        /// <summary>
        /// 
        /// </summary>     
        [Keyword]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary        
        [Text(Analyzer ="ik_max_word",SearchAnalyzer = "ik_max_word")]
        public string Question { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Text(Analyzer = "ik_max_word", SearchAnalyzer = "ik_max_word")]
        public string Answer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Date]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Date]
        public DateTime UpdateDate { get; set; }
    }
}
