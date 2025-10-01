    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MuniLK.Application.Generic.DTOs
{
    public class ClientConfigurationCreateDto
    {
        [Required]
        [MaxLength(256)]
        public string ConfigKey { get; set; }

        [Required]
        public JsonElement ConfigJson { get; set; }
    }

    public class ClientConfigurationUpdateDto : ClientConfigurationCreateDto
    {
        public Guid Id { get; set; }
    }

    public class ClientConfigurationDto : ClientConfigurationUpdateDto
    {
        public Guid? TenantId { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
    }
}

