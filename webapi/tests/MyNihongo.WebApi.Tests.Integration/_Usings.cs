global using FluentAssertions;
global using Grpc.Core;
global using LinqToDB;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Logging.Debug;
global using MyNihongo.Tests.GrpcClient;
global using MyNihongo.Tests.Integration;
global using MyNihongo.WebApi.Infrastructure;
global using MyNihongo.WebApi.Resources.Const;
global using Xunit.Extensions.Ordering;

[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]
