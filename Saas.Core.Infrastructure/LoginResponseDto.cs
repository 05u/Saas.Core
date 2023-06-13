/*******************************************************************************
* Copyright (C) Oxygen
* 
* Author: Oxygen
* Create Date: 2019/11/25 10:46:02
* Description: <Description>
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/

namespace Saas.Core.Infrastructure.Dtos
{
    /// <summary>
    /// 登陆返回数据模型
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间(秒)
        /// </summary>
        public int Expire { get; set; }
    }
}
