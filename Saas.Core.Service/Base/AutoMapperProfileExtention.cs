
/*******************************************************************************
* Copyright (C) Oxygen
* 
* Author: Oxygen
* Create Date: 10/18/2019 11:54:03
* Description: AutoMapper映射关系配置文件(手动添加的对象映射配置，在此文件里面修改) 
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/

using Microsoft.Extensions.DependencyInjection;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;

namespace Saas.Core.Service.Base
{
    /// <summary>
    /// AutoMapp自定义对象映射关系配置文件
    /// </summary>
    public class AutoMapperProfileExtention : AutoMapperProfile
    {
        /// <summary>
        /// 自定义对象映射关系配置
        /// </summary>
        public AutoMapperProfileExtention() : base()
        {


            //CreateMap<MesFeedStandard, FeedStandardImportDto>()
            //    .ForMember(dest => dest.MaterielCode, opt => opt.MapFrom(src => src.Materiel.Code))
            //    .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.ProcessControl.Product.Code))
            //    .ForMember(dest => dest.ProcessCode, opt => opt.MapFrom(src => src.ProcessControl.Process.Code))
            //    .ForMember(dest => dest.ProcessName, opt => opt.MapFrom(src => src.ProcessControl.Process.Name))
            //    ;

            CreateMap<BusRemoteCommand, ClientHeartbeatOutput>()
                ;

            CreateMap<SysUser, CurrentUserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                ;

            CreateMap<SysClient, CurrentUserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LoginName, opt => opt.MapFrom(src => src.ClientId))
                ;

        }
    }

    /// <summary>
    /// AutoMapper配置扩展
    /// </summary>
    public static class OxygenAutoMapperExtention
    {
        /// <summary>
        /// 加载AutoMapper的映射配置关系
        /// </summary>
        /// <param name="services"></param>
        public static void UseAutoMapperExtention(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfile<AutoMapperProfileExtention>();

            }, typeof(AutoMapperProfileExtention).Assembly);
        }
    }
}

