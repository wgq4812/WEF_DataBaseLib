﻿/****************************************************************************
*项目名称：WEF.NoSql.Core
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.NoSql.Core
*类 名 称：OnErrorHandler
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2019/12/6 15:54:23
*描述：
*=====================================================================
*修改时间：2019/12/6 15:54:23
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEF.NoSql.Core
{
    public delegate void OnErrorHandler(string settingInfo, Exception ex);
}
