using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Domain.Entities
{
    // In MuniLK.Core or MuniLK.Domain
    public class Module  // Assuming you have a base entity
    {
        public Guid Id { get; set; } // Using Guid for primary key
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string IconCssClass { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsCoreModule { get; set; } // Modules that cannot be disabled (globally)

        // For hierarchical modules
        public Guid? ParentModuleId { get; set; }
        public Module ParentModule { get; set; }
        public ICollection<Module> ChildModules { get; set; } = new List<Module>();
    }
}
