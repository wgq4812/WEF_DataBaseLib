﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WEF.ModelGenerator.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WEF.ModelGenerator.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap _0616152d52f214c689f01819eefb836d {
            get {
                object obj = ResourceManager.GetObject("0616152d52f214c689f01819eefb836d", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap WEF {
            get {
                object obj = ResourceManager.GetObject("WEF", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找类似 oracle 分页
        ///
        ///SELECT *
        ///
        ///  FROM (SELECT a.*, ROWNUM rn
        ///
        ///          FROM (SELECT *
        ///
        ///                  FROM table_name) a
        ///
        ///         WHERE ROWNUM &lt;= 40)
        ///
        /// WHERE rn &gt;= 21
        ///
        ///oracle取前面n条
        ///
        ///select * FROM
        /// TB_AUTHKEY where ROWNUM&lt;20
        ///
        ///sqlserver分页
        ///
        ///select * from 
        ///(select *, ROW_NUMBER() OVER(Order by ArtistId ) AS RowId from ArtistModels) as b  
        ///where RowId between 10 and 20
        ///
        ///select * from ArtistModels  order by ArtistId offset 4 rows fetch next 5 rows only
        ///
        ///sqlserver 取前面n条
        ///
        ///select top 20 * from [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string 分页 {
            get {
                return ResourceManager.GetString("分页", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 --创建名为 GetStuCou_DS 的有输入参数、输出参数和结果集的存储过程
        ///create procedure GetStuCou_DS
        ///@StuNo    nvarchar(64),
        ///@Height nvarchar(32) output
        ///as
        ///begin
        ///    if(@StuNo is not null and @StuNo &lt;&gt; &apos;&apos;)
        ///    begin
        ///        select @Height=S_Height 
        ///        from Student 
        ///        where S_StuNo=@StuNo
        ///    end
        ///    else
        ///    begin
        ///        set @Height=&apos;185&apos;
        ///    end
        ///
        ///    select s.S_Id,s.S_StuNo,s.S_Name,s.S_Sex,s.S_Height,s.S_BirthDate,c.C_Id,c.C_Name
        ///    from Student s
        ///    left join Course c on s.C_S_Id=c.C_Id
        ///    where S_S [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string 创建存储过程 {
            get {
                return ResourceManager.GetString("创建存储过程", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 CREATE DATABASE database-name 的本地化字符串。
        /// </summary>
        internal static string 创建数据库 {
            get {
                return ResourceManager.GetString("创建数据库", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 create table tabname(col1 type1 [not null] [primary key],col2 type2 [not null],..) 的本地化字符串。
        /// </summary>
        internal static string 创建新表 {
            get {
                return ResourceManager.GetString("创建新表", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 create [unique] index idxname on tabname(col….) 的本地化字符串。
        /// </summary>
        internal static string 创建索引 {
            get {
                return ResourceManager.GetString("创建索引", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 create view viewname as select statement 的本地化字符串。
        /// </summary>
        internal static string 创建视图 {
            get {
                return ResourceManager.GetString("创建视图", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Alter table tabname drop primary key(col) 的本地化字符串。
        /// </summary>
        internal static string 删除主键 {
            get {
                return ResourceManager.GetString("删除主键", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 DROP DATEBASE database-name 的本地化字符串。
        /// </summary>
        internal static string 删除数据库 {
            get {
                return ResourceManager.GetString("删除数据库", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 drop index idxname 的本地化字符串。
        /// </summary>
        internal static string 删除索引 {
            get {
                return ResourceManager.GetString("删除索引", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 drop table tabname 的本地化字符串。
        /// </summary>
        internal static string 删除表 {
            get {
                return ResourceManager.GetString("删除表", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 drop view viewname 的本地化字符串。
        /// </summary>
        internal static string 删除视图 {
            get {
                return ResourceManager.GetString("删除视图", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 dbcc shrinkdatabase(dbname) 的本地化字符串。
        /// </summary>
        internal static string 压缩数据库 {
            get {
                return ResourceManager.GetString("压缩数据库", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 选择：select * from table1 where 范围
        ///插入：insert into table1(field1,field2) values(value1,value2)
        ///删除：delete from table1 where 范围
        ///更新：update table1 set field1=value1 where 范围
        ///查找：select * from table1 where field1 like ’%value1%’ ---like的语法很精妙，查资料!
        ///排序：select * from table1 order by field1,field2 [desc]
        ///总数：select count as totalcount from table1
        ///求和：select sum(field1) as sumvalue from table1
        ///平均：select avg(field1) as avgvalue from table1
        ///最大：select max(field1) as maxvalue from table1
        ///最小：select min(field1) as minv [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string 基本的sql语句 {
            get {
                return ResourceManager.GetString("基本的sql语句", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Alter table tabname add column col type 的本地化字符串。
        /// </summary>
        internal static string 增加列 {
            get {
                return ResourceManager.GetString("增加列", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 --- 创建 备份数据的 device
        ///USE master
        ///EXEC sp_addumpdevice &apos;disk&apos;, &apos;testBack&apos;, &apos;c:\mssql7backup\MyNwind_1.dat&apos;
        ///--- 开始 备份
        ///BACKUP DATABASE pubs TO testBack 的本地化字符串。
        /// </summary>
        internal static string 备份SqlServer {
            get {
                return ResourceManager.GetString("备份SqlServer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Select * From TableName Order By CustomerName Collate Chinese_PRC_Stroke_ci_as //从少到多 的本地化字符串。
        /// </summary>
        internal static string 按姓氏笔画排序 {
            get {
                return ResourceManager.GetString("按姓氏笔画排序", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 DBCC SHRINKDB
        ///DBCC SHRINKFILE 的本地化字符串。
        /// </summary>
        internal static string 收缩数据和日志 {
            get {
                return ResourceManager.GetString("收缩数据和日志", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 select encrypt(&apos;原始密码&apos;)
        ///select pwdencrypt(&apos;原始密码&apos;)
        ///select pwdcompare(&apos;原始密码&apos;,&apos;加密后密码&apos;) = 1--相同；否则不相同 encrypt(&apos;原始密码&apos;) 的本地化字符串。
        /// </summary>
        internal static string 数据库加密 {
            get {
                return ResourceManager.GetString("数据库加密", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 select * from sys.objects where type=&apos;P&apos;
        ///
        ///select * from sys.procedures
        ///
        ///select * from sysobjects where xtype=&apos;P&apos;
        ///
        /////查看存储过程定义sql
        ///select text from syscomments where id=object_id(&apos;AddUser2&apos;) 的本地化字符串。
        /// </summary>
        internal static string 查看存储过程 {
            get {
                return ResourceManager.GetString("查看存储过程", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 A：create table tab_new like tab_old (使用旧表创建新表)
        ///B：create table tab_new as select col1,col2… from tab_old definition only 的本地化字符串。
        /// </summary>
        internal static string 根据已有的表创建新表 {
            get {
                return ResourceManager.GetString("根据已有的表创建新表", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 sqlserver
        ///
        ///Alter table tabname add primary key(col)
        ///
        ///
        ///oracle
        ///
        ///alter table T_Grade 
        ///add constraint pk_gradeId primary key (gradeId); 的本地化字符串。
        /// </summary>
        internal static string 添加主键 {
            get {
                return ResourceManager.GetString("添加主键", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 declare @NodeId int
        ///  declare @NID int
        ///  set @NodeId=1        -- 表示选择节点的ID
        ///
        ///  declare @temp_value table
        ///  (
        ///      ID int identity(1,1),
        ///      value int
        ///  )
        ///
        ///  insert into @temp_value 
        ///  select D_ID from Department where D_ID=@NodeId
        ///  
        ///  declare one_curr cursor local scroll dynamic        --定义一个局部的动态游标
        ///  for select value from @temp_value
        ///
        ///  open one_curr
        ///  fetch next from one_curr into @NID
        ///  while(@@FETCH_STATUS=0)
        ///  begin
        ///        if exists(select D_ID from Department where D_ParentID=@ [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string 游标 {
            get {
                return ResourceManager.GetString("游标", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 select Name from sysobjects where xtype=&apos;u&apos; and status&gt;=0 的本地化字符串。
        /// </summary>
        internal static string 用户表 {
            get {
                return ResourceManager.GetString("用户表", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 DBCC REINDEX
        ///DBCC INDEXDEFRAG 的本地化字符串。
        /// </summary>
        internal static string 重建索引 {
            get {
                return ResourceManager.GetString("重建索引", resourceCulture);
            }
        }
    }
}
