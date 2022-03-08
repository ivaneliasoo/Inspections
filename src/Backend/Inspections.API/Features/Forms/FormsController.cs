using Ardalis.GuardClauses;
using Inspections.API.Features.Forms.Create;
using Inspections.API.Features.Forms.Delete;
using Inspections.API.Features.Forms.Get;
using Inspections.API.Features.Forms.GetAll;
using Inspections.API.Features.Forms.GetAllByReportId;
using Inspections.Core.Domain.Forms;
using Inspections.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Forms;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FormsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly InspectionsContext _context;

    public FormsController(IMediator mediator, InspectionsContext context)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    [HttpGet(Name = nameof(GetFormsDefinitions))]
    public async Task<ActionResult<List<FormDefinitionResponse>>> GetFormsDefinitions([FromQuery]GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}", Name = nameof(GetFormDefinition))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetFormDefinition(int id, CancellationToken cancellationToken)
    {
        var formDef = await _mediator.Send(new GetFormByIdQuery(id), cancellationToken);
        return Ok(formDef);
    }

    // PUT: api/FormsDefinitions/5
    [HttpPut("{id}", Name = "UpdateFormDefinition")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> PutFormDefinition(int id, [FromBody] NewFormDefinitionCommand newFormDef)
    {
        Guard.Against.Null(newFormDef, nameof(newFormDef));

        if (id != newFormDef.Id)
        {
            return BadRequest();
        }

        var savedFormDefinition = await _context.Set<FormDefinition>().FindAsync(id).ConfigureAwait(false);
            
        if (savedFormDefinition == null)
        {
            return NotFound("formDef not found");
        }
            
        //TODO: Update
        savedFormDefinition.Enabled = newFormDef.Enabled;
        savedFormDefinition.SetFields(newFormDef.Fields);
        savedFormDefinition.Icon = newFormDef.Icon;
        savedFormDefinition.SetTitle(newFormDef.Title);
        _context.Entry(savedFormDefinition).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FormDefinitionExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpPost(Name = nameof(AddFormDefinition))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<FormDefinitionResponse>> AddFormDefinition([FromBody] NewFormDefinitionCommand newFormDef, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(newFormDef, cancellationToken);
        return CreatedAtAction("AddFormDefinition", new { id = newFormDef.Id }, result);
    }

    [HttpDelete("{id}", Name = "DeleteFormDefinition")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteFormDefinition(int id)
    {
        var result = await _mediator.Send(new DeleteFormDefinitionCommand(id));
        if (!result)
            return BadRequest();

        return NoContent();
    }

    private bool FormDefinitionExists(int id)
    {
        return _context.FormsDefinitions.Any(e => e.Id == id);
    }
}
