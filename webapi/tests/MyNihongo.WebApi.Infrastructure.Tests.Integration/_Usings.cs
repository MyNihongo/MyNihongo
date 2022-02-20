global using FluentAssertions;
global using MyNihongo.Tests.Integration;
global using MyNihongo.WebApi.Infrastructure.Kanji;
global using Xunit;

[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]
[assembly: ApprovalTests.Reporters.UseReporter(typeof(ApprovalTests.Reporters.VisualStudioReporter))]
[assembly: ApprovalTests.Namers.UseApprovalSubdirectory("Approvals")] 