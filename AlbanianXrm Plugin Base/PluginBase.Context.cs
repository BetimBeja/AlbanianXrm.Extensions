﻿using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace AlbanianXrm
{
    public partial class PluginBase
    {
        private class Context : IContext
        {
            private readonly IServiceProvider serviceProvider;
            private ITracingService tracingService;
            private IPluginExecutionContext pluginExecutionContext;
            private IOrganizationServiceFactory organizationServiceFactory;
            private IOrganizationService organizationServiceUser;
            private IOrganizationService organizationServiceSystem;
            private Dictionary<Guid, IOrganizationService> organizationServices;

            public Context(IServiceProvider serviceProvider)
            {
                this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            }

            public ITracingService TracingService
            {
                get
                {
                    if (tracingService == null)
                    {
                        tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                    }
                    return tracingService;
                }
            }

            public IPluginExecutionContext PluginExecutionContext
            {
                get
                {
                    if (pluginExecutionContext == null)
                    {
                        pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                    }
                    return pluginExecutionContext;
                }
            }

            public IOrganizationService GetOrganizationService()
            {
                if (organizationServiceUser != null)
                {
                    return organizationServiceUser;
                }
                if (pluginExecutionContext == null)
                {
                    pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                }
                if (organizationServiceFactory == null)
                {
                    organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                }
                organizationServiceUser = organizationServiceFactory.CreateOrganizationService(pluginExecutionContext.UserId);
                return organizationServiceUser;
            }

            public IOrganizationService GetOrganizationService(Guid systemuserid)
            {
                IOrganizationService organizationService;
                if (organizationServices == null)
                {
                    organizationServices = new Dictionary<Guid, IOrganizationService>();
                }
                else if (organizationServices.TryGetValue(systemuserid, out organizationService))
                {
                    return organizationService;
                }
                if (pluginExecutionContext == null)
                {
                    pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                }
                if (organizationServiceFactory == null)
                {
                    organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                }
                if (systemuserid == pluginExecutionContext.UserId)
                {
                    if (organizationServiceUser == null)
                    {
                        organizationServiceUser = organizationServiceFactory.CreateOrganizationService(systemuserid);
                    }
                    organizationService = organizationServiceUser;
                }
                else
                {
                    organizationService = organizationServiceFactory.CreateOrganizationService(systemuserid);
                }

                organizationServices.Add(systemuserid, organizationService);
                return organizationService;
            }

            public IOrganizationService GetOrganizationServiceSystem()
            {
                if (organizationServiceSystem != null)
                {
                    return organizationServiceSystem;
                }
                if (organizationServiceFactory == null)
                {
                    organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                }
                organizationServiceSystem = organizationServiceFactory.CreateOrganizationService(null);
                return organizationServiceSystem;
            }
        }
    }
}
