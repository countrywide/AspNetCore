// <copyright file="MetricsAspNetWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.AspNetCore.Hosting;
using App.Metrics.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics AspNet Core services in an <see cref="IWebHostBuilder" />.
    /// </summary>
    public static class MetricsAspNetWebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureMetricsWithDefaults(
            this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, IMetricsBuilder> configureMetrics)
        {
            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    if (serviceProvider.GetService(typeof(MetricsMarkerService)) == null)
                    {
                        var metricsBuilder = AppMetrics.CreateDefaultBuilder();
                        configureMetrics(context, metricsBuilder);
                        metricsBuilder.Configuration.ReadFrom(context.Configuration);
                        services.AddMetrics(metricsBuilder);
                        services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();
                    }
                });

            return hostBuilder;
        }

        public static IWebHostBuilder ConfigureMetricsWithDefaults(this IWebHostBuilder hostBuilder, Action<IMetricsBuilder> configureMetrics)
        {
            hostBuilder.ConfigureMetricsWithDefaults(
                (context, builder) =>
                {
                    configureMetrics(builder);
                });

            return hostBuilder;
        }

        public static IWebHostBuilder ConfigureMetrics(this IWebHostBuilder hostBuilder, IMetricsRoot metrics)
        {
            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    if (serviceProvider.GetService(typeof(MetricsMarkerService)) == null)
                    {
                        services.AddMetrics(metrics);
                        services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();
                    }
                });
        }

        public static IWebHostBuilder ConfigureMetrics(
            this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, IMetricsBuilder> configureMetrics)
        {
            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    if (serviceProvider.GetService(typeof(MetricsMarkerService)) == null)
                    {
                        services.AddMetrics(
                            builder =>
                            {
                                configureMetrics(context, builder);
                                builder.Configuration.ReadFrom(context.Configuration);
                            });
                        services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();
                    }
                });
        }

        public static IWebHostBuilder ConfigureMetrics(this IWebHostBuilder hostBuilder, Action<IMetricsBuilder> configureMetrics)
        {
            hostBuilder.ConfigureMetrics(
                (context, builder) =>
                {
                    configureMetrics(builder);
                });

            return hostBuilder;
        }

        public static IWebHostBuilder ConfigureMetrics(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    if (serviceProvider.GetService(typeof(MetricsMarkerService)) == null)
                    {
                        var builder = AppMetrics.CreateDefaultBuilder().Configuration.ReadFrom(context.Configuration);
                        services.AddMetrics(builder);
                        services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();
                    }
                });
        }
    }
}