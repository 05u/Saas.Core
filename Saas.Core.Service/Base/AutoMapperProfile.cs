
/*******************************************************************************
* Copyright (C) Oxygen
* 
* Author: Oxygen
* Create Date: 11/09/2020 10:57:23
* Description: AutoMapper????????????(??????T4??????????????) 
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/

using AutoMapper;
using Saas.Core.Data.Entities;
using Saas.Core.Service.Dtos;

namespace Saas.Core.Service.Base
{
    /// <summary>
    /// AutoMapperProfile
    /// </summary>
    public partial class AutoMapperProfile : Profile
    {
        /// <summary>
        /// AutoMapperProfile
        /// </summary>
        public AutoMapperProfile()
        {

            CreateMap<MdmXiaoaiSpeakerDto, MdmXiaoaiSpeaker>();
            CreateMap<MdmXiaoaiSpeaker, MdmXiaoaiSpeakerDto>();
            CreateMap<WorkTaskRecordDto, BusWorkTaskRecord>();
            CreateMap<BusWorkTaskRecord, WorkTaskRecordDto>();
            CreateMap<InterfaceMonitorDto, BusInterfaceMonitor>();
            CreateMap<BusInterfaceMonitor, InterfaceMonitorDto>();
            CreateMap<PregnantWomanEventRecordDto, BusPregnantWomanEventRecord>();
            CreateMap<BusPregnantWomanEventRecord, PregnantWomanEventRecordDto>();
            CreateMap<HomePersionDto, MdmHomePersion>();
            CreateMap<MdmHomePersion, HomePersionDto>();
            
        }
    }
}

