/*******************************************************************************
* Copyright (C) Oxygen
* 
* Author: Oxygen
* Create Date: 2019/12/3 15:37:27
* Description: <Description>
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/

namespace Saas.Core.Infrastructure.Extentions
{
    /// <summary>
    /// 文件夹目录扩展
    /// </summary>
    public static class DirectoryExtention
    {
        /// <summary>
        /// 如果文件夹不存在，则创建
        /// </summary>
        /// <param name="direcotry">文件夹本地路径</param>
        public static void CreateDirectoryIfNotExists(this string direcotry)
        {
            if (direcotry.IsBlank())
            {
                return;
            }
            if (!Directory.Exists(direcotry))
            {
                Directory.CreateDirectory(direcotry);
            }
        }
    }
}
