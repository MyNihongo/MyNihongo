﻿global using MyNihongo.Tests.Integration;
global using MyNihongo.WebApi.Infrastructure.Kanji;
global using Xunit;
global using VerifyXunit;

[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]