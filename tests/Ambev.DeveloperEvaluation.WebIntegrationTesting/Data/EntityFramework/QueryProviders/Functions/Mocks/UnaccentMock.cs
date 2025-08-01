using Bogus.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.QueryProviders.Functions.Mocks
{
  internal class UnaccentMock
  {
    public static readonly MethodInfo Original = typeof(NpgsqlFullTextSearchDbFunctionsExtensions)
        .GetMethod(nameof(NpgsqlFullTextSearchDbFunctionsExtensions.Unaccent), new[] { typeof(DbFunctions), typeof(string) })
            ?? throw new MissingMemberException($"Cannot get method {nameof(NpgsqlDbFunctionsExtensions.ILike)} from {nameof(NpgsqlDbFunctionsExtensions)}.");

    public static readonly MethodInfo Replacement = typeof(UnaccentMock)
        .GetMethod(nameof(UnaccentMock.Unaccent))
            ?? throw new MissingMemberException($"Cannot get method {nameof(UnaccentMock.Unaccent)} from {nameof(UnaccentMock)}.");

        public static string Unaccent(DbFunctions functions, string text)
      => text.RemoveDiacritics();
  }
}
