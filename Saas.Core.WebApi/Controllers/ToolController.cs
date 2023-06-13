using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Attributes;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 工具
    /// </summary>
    public class ToolController : BaseApiController
    {
        private readonly ToolService _toolService;
        private readonly ICapPublisher _capPublisher;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly BusNoticeMessageService _noticeMessageService;
        private readonly BlockchainService _blockchainService;

        /// <summary>
        /// ctor
        /// </summary>
        public ToolController(ToolService toolService, ICapPublisher capPublisher, EmailService emailService, IConfiguration configuration, BusNoticeMessageService noticeMessageService, BlockchainService blockchainService)
        {
            _toolService = toolService;
            _capPublisher = capPublisher;
            _emailService = emailService;
            _configuration = configuration;
            _noticeMessageService = noticeMessageService;
            _blockchainService = blockchainService;
        }

        /// <summary>
        /// 获取所有的枚举列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public List<SelectItem> AllSelectListTypes()
        {

            return (from type in typeof(ToolTipsAttribute).Assembly.GetTypes()
                    where type.Namespace != null && (type.Namespace.IsNotBlank() &&
                                                   type.Namespace.Contains("Saas.Core.Infrastructure.Enums") &&
                                                   type.IsPublic && type.IsEnum)
                    select new SelectItem { Code = type.Name, Id = type.Name, Text = type.FullName }).ToList();//&& type.Name.ToLower() == className.ToLower()
        }


        /// <summary>
        /// 获取枚举值列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public List<SelectEnumItem> SelectListTypes(string typeName)
        {

            var curType = (from type in typeof(ToolTipsAttribute).Assembly.GetTypes()
                           where type.Namespace != null && (type.Namespace.IsNotBlank() &&
                                                          type.Namespace.Contains("Saas.Core.Infrastructure.Enums") &&
                                                          type.IsPublic && type.IsEnum && type.Name.ToLower() == typeName.ToLower())
                           select type).FirstOrDefault();//&& type.Name.ToLower() == className.ToLower()
            var filterLst = curType.SelectList();
            return filterLst;

        }

        /// <summary>
        /// 测试使用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<object> OnlyTest(string slug)
        {
            
            return await _blockchainService.GetPrice(slug);

        }



        /// <summary>
        /// ChatGPT语言模型接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ChatGPTOutput> ChatGPT([FromBody] ChatGPTInput input)
        {
            return await _toolService.ChatGPT(input);
        }
    }
}
