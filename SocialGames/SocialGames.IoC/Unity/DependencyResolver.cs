﻿using System.Data.Entity;
using SocialGames.Domain.Interfaces.Repositories;
using SocialGames.Domain.Interfaces.Repositories.Base;
using SocialGames.Domain.Interfaces.Services;
using SocialGames.Domain.Services;
using SocialGames.Infra.Persistence;
using SocialGames.Infra.Persistence.Repositories;
using SocialGames.Infra.Persistence.Repositories.Base;
using SocialGames.Infra.Transactions;
using Unity;
using Unity.Lifetime;

namespace SocialGames.IoC.Unity
{
    public class DependencyResolver
    {
        public static void Resolve(UnityContainer container)
        {

            container.RegisterType<DbContext, SocialGamesContext>(new HierarchicalLifetimeManager());
            //UnitOfWork
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());

            //Serviço de Domain
            //container.RegisterType(typeof(IServiceBase<,>), typeof(ServiceBase<,>));

            container.RegisterType<IServicePlayer, ServicePlayer>(new HierarchicalLifetimeManager());



            //Repository
            container.RegisterType(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));

            container.RegisterType<IRepositoryPlayer, RepositoryPlayer>(new HierarchicalLifetimeManager());



        }
    }
}
