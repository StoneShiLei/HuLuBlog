using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Com.Stone.HuLuBlog.Repositories
{
    public static class SqlSugarExtensions
    {
        /// <summary>
        /// 获取数据库处理对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sqlSugarClient"></param>
        /// <returns></returns>
        public static SimpleClient<TEntity> GetSimpleClient<TEntity>(this SqlSugarClient sqlSugarClient) where TEntity : class, new()
        {
            return new SimpleClient<TEntity>(sqlSugarClient);
        }

        #region 根据数据库产生实体类

        /// <summary>
        /// 根据数据库表产生实体类
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        public static void CreateClassFileByDBTalbe(this SqlSugarClient _db, string strPath, string strNameSpace)
        {
            CreateClassFileByDBTalbe(_db, strPath, strNameSpace, null);
        }

        /// <summary>
        /// 根据数据库表产生实体类
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        public static void CreateClassFileByDBTalbe(
            this SqlSugarClient _db,
                string strPath,
                string strNameSpace,
                string[] lstTableNames)
        {
            CreateClassFileByDBTalbe(_db, strPath, strNameSpace, lstTableNames, string.Empty);
        }

        /// <summary>
        /// 根据数据库表产生实体类
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        public static void CreateClassFileByDBTalbe(
            this SqlSugarClient _db,
              string strPath,
              string strNameSpace,
              string[] lstTableNames,
              string strInterface,
              bool blnSerializable = false)
        {
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                _db.DbFirst.Where(lstTableNames).IsCreateDefaultValue().IsCreateAttribute()
                    .SettingClassTemplate(p => p = @"
{using}

namespace {Namespace}
{
    {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
    public partial class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
    {
        public {ClassName}()
        {
{Constructor}
        }
{PropertyName}
    }
}
")
                    .SettingPropertyTemplate(p => p = @"
            {SugarColumn}
            public {PropertyType} {PropertyName}
            {
                get
                {
                    return _{PropertyName};
                }
                set
                {
                    if(_{PropertyName}!=value)
                    {
                        base.SetValueCall(" + "\"{PropertyName}\",_{PropertyName}" + @");
                    }
                    _{PropertyName}=value;
                }
            }")
                    .SettingPropertyDescriptionTemplate(p => p = "          private {PropertyType} _{PropertyName};\r\n" + p)
                    .SettingConstructorTemplate(p => p = "              this._{PropertyName} ={DefaultValue};")
                    .CreateClassFile(strPath, strNameSpace);
            }
            else
            {
                _db.DbFirst.IsCreateAttribute().IsCreateDefaultValue()
                    .SettingClassTemplate(p => p = @"
{using}

namespace {Namespace}
{
    {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
    public partial class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
    {
        public {ClassName}()
        {
{Constructor}
        }
{PropertyName}
    }
}
")
                    .SettingPropertyTemplate(p => p = @"
            {SugarColumn}
            public {PropertyType} {PropertyName}
            {
                get
                {
                    return _{PropertyName};
                }
                set
                {
                    if(_{PropertyName}!=value)
                    {
                        base.SetValueCall(" + "\"{PropertyName}\",_{PropertyName}" + @");
                    }
                    _{PropertyName}=value;
                }
            }")
                    .SettingPropertyDescriptionTemplate(p => p = "          private {PropertyType} _{PropertyName};\r\n" + p)
                    .SettingConstructorTemplate(p => p = "              this._{PropertyName} ={DefaultValue};")
                    .CreateClassFile(strPath, strNameSpace);
            }
        }

        #endregion

        #region 根据实体类生成数据库表

        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public static void CreateTableByEntity<T>(this SqlSugarClient _db, bool blnBackupTable, params T[] lstEntitys) where T : class, new()
        {
            Type[] lstTypes = null;
            if (lstEntitys != null)
            {
                lstTypes = new Type[lstEntitys.Length];
                for (int i = 0; i < lstEntitys.Length; i++)
                {
                    T t = lstEntitys[i];
                    lstTypes[i] = typeof(T);
                }
            }
            CreateTableByEntity(_db, blnBackupTable, lstTypes);
        }

        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public static void CreateTableByEntity(this SqlSugarClient _db, bool blnBackupTable, params Type[] lstEntitys)
        {
            if (blnBackupTable)
            {
                _db.CodeFirst.BackupTable().InitTables(lstEntitys); //change entity backupTable            
            }
            else
            {
                _db.CodeFirst.InitTables(lstEntitys);
            }
        }
        #endregion
    }
}
