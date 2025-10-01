using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Generic.DTOs
{
    public class ModuleCreateDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string IconCssClass { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsCoreModule { get; set; }
        public Guid? ParentModuleId { get; set; }
    }
    public class ModuleUpdateDto : ModuleCreateDto
    {
        public Guid Id { get; set; }
    }

    public class ModuleDto : ModuleUpdateDto
    {
        public string ParentModuleName { get; set; } // For displaying parent name in the DTO
    }
}
