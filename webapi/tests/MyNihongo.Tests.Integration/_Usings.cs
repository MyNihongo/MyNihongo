global using LinqToDB;
global using LinqToDB.Data;
global using LinqToDB.Mapping;
global using Microsoft.Data.SqlClient;
global using Microsoft.Extensions.Configuration;
global using Moq;
global using Moq.Microsoft.Configuration;
global using MyNihongo.Database.Abstractions;
global using MyNihongo.Migrations;
global using MyNihongo.WebApi.Infrastructure;
global using MyNihongo.WebApi.Infrastructure.Kanji;
global using static MyNihongo.Migrations.Columns.Auth;
global using static MyNihongo.Migrations.Columns.Core;
global using static MyNihongo.Migrations.Columns.Kanji;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MyNihongo.Tests.GrpcClient")] 