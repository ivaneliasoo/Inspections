using AutoMapper;
using Inspections.Core.Domain.Forms;

namespace Inspections.API.Features.Forms;

public class FormsMappingProfile : Profile
{
    public FormsMappingProfile()
    {
        CreateMap<FormDefinition, FormDefinitionResponse>();
    }
}
