namespace MuniLK.Infrastructure.Services;
public sealed class EmailSettings
{
    public string SmtpHost { get; init; } = "smtp.gmail.com";
    public int    SmtpPort { get; init; } = 587;
    public string? SmtpUsername { get; init; } = "kavishkasubashana14@gmail.com";
    public string? SmtpPassword { get; init; } = "ghuu lwwi nuiq frim";
    public string FromEmail { get; init; } = "kavishkasubashana14@gmail.com";
    public string FromName  { get; init; } = "MuniLK";
}