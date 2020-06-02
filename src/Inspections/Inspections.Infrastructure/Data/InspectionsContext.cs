using Inspections.Core.Domain.InspectionsAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Infrastructure.Data
{
    public class InspectionsContext : DbContext
    {
        public InspectionsContext(DbContextOptions options)
            :base(options)
        {

        }
        public ICollection<Inspection> Inspections { get; set; }
    }
}
