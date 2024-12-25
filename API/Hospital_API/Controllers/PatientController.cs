using Hospital_API.Constants;
using Hospital_API.CQRS.Commands;
using Hospital_API.CQRS.Queries;
using Hospital_API.Extensions;
using Hospital_API.Requests;
using Hospital_API.Responses;
using Hospital_API.ValueTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Hospital_API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/patient")]
public class PatientController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientController(IMediator mediator)
    {
        _mediator = mediator;
    }


    /// <summary>
    /// Gets the list of patients
    /// </summary>
    /// <remarks>
    /// Sample request: GET /get-all
    /// </remarks>
    /// <param name="token">cancellation token</param>
    /// <returns>Return PatientResponse collection</returns>
    /// <response code="200">If the patient list was successfully found</response>
    /// <response code="404">If patient list not found</response>
    [HttpGet]
    [Route("get-all")]
    [ProducesResponseType(typeof(IEnumerable<PatientResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> GetAllPatients(CancellationToken token)
    {
        var result = await _mediator.Send(new GetAllPatientsQuery(), token);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }


    /// <summary>
    /// Gets a patient by their name id
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// GET /get/b7351fb5-881a-4bec-12c2-08dd211d7e10
    /// </remarks>
    /// <param name="id">Id (guid)</param>
    /// <param name="token">cancellation token</param>
    /// <returns>Return PatientResponse</returns>
    /// <response code="200" type="Error">If the patient was successfully found</response>
    /// <response code="404" type="Error">If patient not found</response>
    /// <response code="400" type="Error">If the request was entered incorrectly</response>
    [HttpGet]
    [Route("get/{id}")]
    [ProducesResponseType(typeof(PatientResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<PatientResponse>> GetPatient(Guid id, CancellationToken token)
    {
        if(!ModelState.IsValid) 
            return BadRequest(new Error($"Incorrect request: {ModelState.Values}"));

        var result = await _mediator.Send(new GetPatientByIdQuery(id), token);

        return result.IsSuccess
             ? Ok(result.Value)
             : NotFound(result.Error);
    }


    /// <summary>
    /// Creates the patient
    /// </summary>
    /// <remarks>
    /// Sample request: 
    /// POST /create
    /// '{
    ///    "name": {
    ///      "use": "string",
    ///      "family": "string",
    ///      "given": [
    ///        "string"
    ///      ]
    ///    },
    ///    "gender": "unknown",
    ///    "birthDate": "2024-12-21T21:01:00.305Z",
    ///    "active": true
    /// }'
    /// </remarks>
    /// <param name="request">PatienCreateRequest object</param>
    /// <param name="token">cancellation token</param>
    /// <returns>Return PatientResponse</returns>
    /// <response code="200" type="PatientResponse">If the patient was successfully created</response>
    /// <response code="400" type="Error">If the request was entered incorrectly</response>
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(PatientResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PatientResponse>> CreatePatient([FromBody]PatientCreateRequest request, CancellationToken token)
    {
        if (!ModelState.IsValid)
            return BadRequest(new Error($"Incorrect request: {ModelState.Values}"));

        var result = await _mediator.Send(new CreatePatientCommand(request), token);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }


    /// <summary>
    /// Creates the patient collection
    /// </summary>
    /// <remarks>
    /// Sample request: 
    /// POST /batch
    /// ['{
    ///    "name": {
    ///      "use": "string",
    ///      "family": "string",
    ///      "given": [
    ///        "string"
    ///      ]
    ///    },
    ///    "gender": "unknown",
    ///    "birthDate": "2024-12-21T21:01:00.305Z",
    ///    "active": true
    /// }']
    /// </remarks>
    /// <param name="request">PatienCreateRequest colletion</param>
    /// <param name="token">cancellation token</param>
    /// <returns>Return Array PatientResponse</returns>
    /// <response code="200" type="PatientResponse List">If the patient was successfully created</response>
    /// <response code="400" type="Error">If the request was entered incorrectly</response>
    [HttpPost]
    [Route("batch")]
    [ProducesResponseType(typeof(IEnumerable<PatientResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> BatchPatient([FromBody] IEnumerable<PatientCreateRequest> request, CancellationToken token)
    {
        if (!ModelState.IsValid)
            return BadRequest(new Error($"Incorrect request: {ModelState.Values}"));
       
        var result = await _mediator.Send(new BatchPatientCommand(request), token);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }


    /// <summary>
    /// Updates the patient
    /// </summary>
    /// <remarks>
    /// Sample request: 
    /// PUT /update
    /// '{
    ///    "name": {
    ///      "id": "b7351fb5-881a-4bec-12c2-08dd211d7e10"
    ///      "use": "string",
    ///      "family": "string",
    ///      "given": [
    ///        "string"
    ///      ]
    ///    },
    ///    "gender": "unknown",
    ///    "birthDate": "2024-12-21T21:01:00.305Z",
    ///    "active": true
    /// }'
    /// </remarks>
    /// <param name="request">PatientUpdateRequest object</param>
    /// <param name="token"></param>
    /// <returns>Return NoContent</returns>
    /// <response code="204">If the patient information is successfully updated</response>
    /// <response code="404" type="Error">If the patient was not found</response>
    /// <response code="400" type="Error">If the request was entered incorrectly</response>
    [HttpPut]
    [Route("update")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdatePatient([FromBody]PatientUpdateRequest request, CancellationToken token)
    {
        if (!ModelState.IsValid)
            return BadRequest(new Error($"Incorrect request: {ModelState.Values}"));

        var result = await _mediator.Send(new UpdatePatientCommand(request), token);

        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.Error);
    }


    /// <summary>
    /// Deleted the patient
    /// </summary>
    /// <remarks>
    /// Sample request: 
    /// DELETE /delete/b7351fb5-881a-4bec-12c2-08dd211d7e10
    /// </remarks>
    /// <param name="id">Id (guid)</param>
    /// <param name="token"></param>
    /// <returns>Return NoContent</returns>
    /// <response code="204">If the patient was successfully removed</response>
    /// <response code="404" type="Error">If the patient was not found</response>
    /// <response code="400" type="Error">If the request was entered incorrectly</response>
    [HttpDelete]
    [Route("delete/{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeletePatient(Guid id, CancellationToken token)
    {
        if (!ModelState.IsValid)
            return BadRequest(new Error($"Incorrect request: {ModelState.Values}"));

        var result = await _mediator.Send(new DeletePatientCommand(id), token);

        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.Error);
    }


    /// <summary>
    /// Search for a patient by date using a prefix code (prefix is ​​not required "equal by default")
    /// </summary>
    /// <remarks>
    /// Sample request: 
    /// GET /search-by-birthday/eq2024-12-22
    /// Used prefix codes: eq, ne, gt, lt, ge, le, sa, eb, ap
    /// Details: https://www.hl7.org/fhir/search.html#date
    /// </remarks>
    /// <param name="date">date with prefix (string)</param>
    /// <param name="token"></param>
    /// <returns>Return NoContent</returns>
    /// <response code="200">If the patient was found</response>
    /// <response code="404" type="Error">If the patient was not found</response>
    /// <response code="400" type="Error">If the request was entered incorrectly</response>
    [HttpGet]
    [Route("search-by-birthday/{date}")]
    [ProducesResponseType(typeof(IEnumerable<PatientResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> SearchByBirthDate(string date, CancellationToken token)
    {
        if (!date.TryParseSearchingData(out SearchingPrefix prefix, out DateTime startDate, out DateTime endDate))
            return BadRequest(new Error($"Incorrect request"));

        var result = await _mediator.Send(new GetAllPatientsByBirthDateQuery(prefix, startDate, endDate), token);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }
}
