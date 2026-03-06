using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Records
{
    public record ProductFilters(
     string? SearchTerm = null,
     DateTime? StartDate = null,
     DateTime? EndDate = null,
     bool ShowInactive = false,
     int Page = 1,
     int PageSize = 10,
     string Sort = "name",
     string Order = "asc"
 );
}
