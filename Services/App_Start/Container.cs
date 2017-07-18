using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Integration.SignalR;
using Domain.Datas;
using Domain.Models;
using Domain.Repositories;
using Infrastructure;
using Infrastructure.Cache;
using Infrastructure.Log;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Application.TestModule;
namespace Services.App_Start
{
    public static class Container
    {
        private static IContainer _currentContainer;
        private static ContainerBuilder _builder;
        public static IContainer Current
        {
            get
            {
                return _currentContainer;
            }
        }
        /// <summary>
        /// Ioc容器的注册配置
        /// 要实例化的接口在此配置
        /// 依赖注入
        /// </summary>
        public static void Register(HttpConfiguration config)
        {
            _builder = new ContainerBuilder();
            //signalR Autofac
            _builder.RegisterApiControllers(Assembly.GetExecutingAssembly());//注册api容器的实现
            _builder.RegisterHubs(Assembly.GetExecutingAssembly());//注册signalR容器的实现
            System.Data.Entity.Database.SetInitializer<MainUnitOfWork>(null);//关闭EF和模型匹配检查
            _builder.RegisterType<MainUnitOfWork>().As<IQueryableUnitOfWork>().InstancePerLifetimeScope();
            _builder.RegisterType<MCache>().As<ICache>();
            _builder.RegisterType<TestObjRepository>().As<ITestObjRepository>();
            _builder.RegisterType<Repository<TestObjs>>().As<IRepository<TestObjs>>();
            _builder.RegisterType<TestApp>().As<ITestApp>();
            _currentContainer = _builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(_currentContainer);//注册API容器
            GlobalHost.DependencyResolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(_currentContainer);//注册SIGNALR
            InitFactories();//初始化工厂相关
        }
        private static void InitFactories()
        {
            LoggerFactory.SetCurrent(new TraceSourceLogFactory());
            CacheFactory.SetCurrent(new MCacheFactory());
        }
    }
}