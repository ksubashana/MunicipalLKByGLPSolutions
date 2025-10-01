using MediatR;
using MuniLK.Application.PropertiesLK.DTOs;

public record CreatePropertyCommand(CreatePropertyRequest Request) : IRequest<PropertyResponse>;
