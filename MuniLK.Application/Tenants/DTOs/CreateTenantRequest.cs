using System.ComponentModel.DataAnnotations;

public class CreateTenantRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Subdomain must be lowercase alphanumeric or hyphens.")]
    public string Subdomain { get; set; }

    [Required]
    [EmailAddress]
    public string ContactEmail { get; set; }
}
