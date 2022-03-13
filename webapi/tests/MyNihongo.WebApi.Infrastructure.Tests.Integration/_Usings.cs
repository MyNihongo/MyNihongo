global using FluentAssertions;
global using LinqToDB;
global using Microsoft.Extensions.Configuration;
global using Moq;
global using MyNihongo.Tests.Integration;
global using MyNihongo.WebApi.Infrastructure.Kanji;
global using NodaTime;
global using Xunit.Extensions.Ordering;

[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]
