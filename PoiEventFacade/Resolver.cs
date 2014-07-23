using System;
using Microsoft.Practices.Unity;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Facade.Interface;

namespace PoiEventNetwork
{
    public static class Resolver
    {
        public static readonly IUnityContainer Container = new UnityContainer();

        static Resolver()
        {
            Container.RegisterType<IDataContext, DataContext>();
            Container.RegisterType<IPoiEventFacade, Facade.Facade>();
        }

        public static T Resolve<T>() where T : class
        {
            if (!typeof(T).IsInterface)
                throw new ArgumentException("The template argument is invalid. Only interfaces are valid.");

            return Container.Resolve<T>();
        }

        //public static T Resolve<T>(string name, object param) where T : class
        //{
        //    if (!typeof(T).IsInterface)
        //        throw new ArgumentException("The template argument is invalid. Only interfaces are valid.");

        //    return Container.Resolve<T>(new ParameterOverride(name, param));
        //}
    }
}
