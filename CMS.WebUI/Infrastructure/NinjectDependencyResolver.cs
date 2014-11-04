using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System.Configuration;
using CMS.Domain.Models;
using CMS.Domain.Abstract;

namespace CMS.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
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
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IEventRepository>().To<EventRepository>();
            kernel.Bind<IFAQRepository>().To<FAQRepository>();
            kernel.Bind<IGalleryRepository>().To<GalleryRepository>();
            kernel.Bind<IImageRepository>().To<ImageRepository>();
            kernel.Bind<IFolderRepository>().To<FolderRepository>();
            kernel.Bind<IDocumentRepository>().To <DocumentRepository>();
            kernel.Bind<IPageRepository>().To<PageRepository>();
            kernel.Bind<ITrashRepository>().To<TrashRepository>();
            kernel.Bind<IMenuRepository>().To<MenuRepository>();
            kernel.Bind<IMenuItemRepository>().To<MenuItemRepository>();
            kernel.Bind<IAdminRepository>().To<AdminRepository>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<IBlogPostRepository>().To<BlogPostRepository>();
            kernel.Bind<IJSONRepository>().To<JSONRepository>();
            kernel.Bind<IFormFieldRepository>().To<FormFieldRepository>();
            kernel.Bind<IFormRepository>().To<FormRepository>();
            kernel.Bind<IHTMLWidgetRepository>().To<HTMLWidgetRepository>();
            kernel.Bind<IWidgetContainer>().To<WidgetContainerRepository>();
            kernel.Bind<IHomeRepository>().To<HomeRepository>();
            kernel.Bind<INewsRepository>().To<NewsRepository>();
            kernel.Bind<ISystemSettingsRepository>().To<SystemSettingsRepository>();
        }
    }
}