//===================================================================================
// Microsoft Developer and Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================

namespace Domain.Datas
{
    //“工具”->“库程序包管理器”->“程序包管理器控制台”update-database -ProjectName Infrastructure.Data.MainBoundedContext
    // Install-Package EntityFramework安装EF工具
    //在程序包管理器控制台中运行 Enable-Migrations -ProjectName {name} 命令
    //一个新的 Migrations 文件夹已添加至项目中，它包含两个文件：
    //Configuration.cs — 此文件包含“迁移”将用来迁移 SSODBEntities 的设置。在此处可以指定种子数据、为其他数据库注册提供程序、更改生成迁移的命名空间等。
    //<时间戳>_InitialCreate.cs — 这是第一个迁移，它表示已经应用于数据库的更改。
    //运行 Update-Database 命令，将新迁移应用于数据库.
    //如果提示丢失数据运行add-migration initial创建初始迁移再运行Update-Database此操作可能会丢失数据.
    //在调用 Update-Database 命令查看对数据库执行的 SQL 时，可以使用 –Verbose 开关。

    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System;
    using Domain.Models;
    using System.Data.Entity.Core.Mapping;
    using System.Data.Entity.Core.Metadata.Edm;
    public class MainUnitOfWork
        : DbContext, IQueryableUnitOfWork
    {
        public MainUnitOfWork()
            : base("name=DbConn")
        {
            //first Time warmUp
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
            mappingCollection.GenerateViews(new List<EdmSchemaError>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MainUnitOfWork>());//保护数据建议使用数据库迁移工具
            Database.CreateIfNotExists();
            //just for performance
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
        }
        #region IDbSet Members
        IDbSet<TestObjs> _testObj = null;
        public IDbSet<TestObjs> TBL_TEST
        {
            get { return _testObj ?? base.Set<TestObjs>(); }
        }
        IDbSet<TestForeign> _testForeignObj = null;
        public IDbSet<TestForeign> TBL_TESTFOREIGN
        {
            get { return _testForeignObj ?? base.Set<TestForeign>(); }
        }
        #endregion

        #region IQueryableUnitOfWork Members
      
        public DbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class
        {
            return base.Set<TEntity>();
        }
        //自对象加载到上下文中后，或自上次调用 System.Data.Objects.ObjectContext.SaveChanges() 方法后，此对象尚未经过修改；
        public void SetUnchanged<TEntity>(TEntity item)
            where TEntity : class
        {
            //attach and set as unchanged
            base.Entry<TEntity>(item).State = System.Data.Entity.EntityState.Unchanged;
        }
        public void SetDetached<TEntity>(TEntity item)
            where TEntity : class
        {
            base.Entry<TEntity>(item).State = System.Data.Entity.EntityState.Detached;
        }
        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            base.Set<TEntity>().Attach(item);
        }
        /// <summary>
        /// 对象已添加到对象上下文，但尚未调用 System.Data.Objects.ObjectContext.SaveChanges() 方法；
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        public void SetAdded<TEntity>(TEntity item) where TEntity : class
        {
            base.Entry<TEntity>(item).State = System.Data.Entity.EntityState.Added;
        }
        /// <summary>
        /// 使用 System.Data.Objects.ObjectContext.DeleteObject(System.Object) 方法从对象上下文中删除了对象；
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        public void SetDeleted<TEntity>(TEntity item) where TEntity : class
        {
            base.Entry<TEntity>(item).State = System.Data.Entity.EntityState.Deleted;
        }
        /// <summary>
        /// 对象已更改，但尚未调用 System.Data.Objects.ObjectContext.SaveChanges() 方法。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        public void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            //this operation also attach item in object state manager
            base.Entry<TEntity>(item).State = System.Data.Entity.EntityState.Modified;
        }
        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            //if it is not attached, attach original and set current values
            base.Entry<TEntity>(original).CurrentValues.SetValues(current);
        }

        public void Commit()
        {
            base.SaveChanges();
        }
        /// <summary>
        /// 提交数据更改并刷新
        /// </summary>
        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries.ToList()
                              .ForEach(entry =>
                               {
                                   entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                               });

                }
            } while (saveFailed);

        }
        /// <summary>
        /// 回滚数据库操作
        /// </summary>
        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = System.Data.Entity.EntityState.Unchanged);
        }
        /// <summary>
        /// 执行特殊SQL脚本查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }
        /// <summary>
        /// 执行特殊SQL脚本入口
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        #endregion

        #region DbContext Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
        #endregion


    }
}
