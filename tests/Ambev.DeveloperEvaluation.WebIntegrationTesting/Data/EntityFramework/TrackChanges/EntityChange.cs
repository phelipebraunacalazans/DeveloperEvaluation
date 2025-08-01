using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.TrackChanges
{
  public class EntityChange
  {
    public required EntityState State { get; init; }

    public required List<PropertyChange> Properties { get; init; }
  }
}
