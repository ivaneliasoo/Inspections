using Inspections.API.Features.ReportsConfiguration.Commands;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.ReportsConfiguration.Handlers;

public class UpdateAdditionalFieldsHandler : IRequestHandler<UpdateAdditionalFields, bool>
{
    private readonly IReportConfigurationsRepository _repository;

    public UpdateAdditionalFieldsHandler(IReportConfigurationsRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<bool> Handle(UpdateAdditionalFields request, CancellationToken cancellationToken)
    {
        try
        {
            var config = await _repository.GetByIdAsync(request.Id);
            //config.AdditionalFields = request.FieldsDefinitions;
            await _repository.UpdateAsync(config);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
