using Ardalis.GuardClauses;
using Inspections.API.Features.Forms.Create;
using Inspections.Core.Domain;
using Inspections.Core.Domain.Forms;

namespace Inspections.API.Features.Forms;

internal static class FormsExtensions
{
    internal static FormDefinition ToEntity(this NewFormDefinitionCommand newForm)
    {
        Guard.Against.Null(newForm, nameof(newForm));

        var result = new FormDefinition(newForm.Name, newForm.Title, newForm.Icon, newForm.Fields);
        return result;
    }
}
