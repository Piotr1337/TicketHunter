using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Concrete;

namespace TicketHunter.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IEventRepository>().To<EFEventRepository>();
            kernel.Bind<ICategoryRepository>().To<EFCategoryRepository>();
            kernel.Bind<ITicketRepository>().To<EFTicketRepository>();
            kernel.Bind<IUserRepository>().To<EFUserRepository>();
            kernel.Bind<IArtistRepository>().To<EFArtistRepository>();
        }
    }
}