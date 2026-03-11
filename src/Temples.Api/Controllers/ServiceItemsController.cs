using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Temples.Core.Constants;
using Temples.Core.DTOs;
using Temples.Core.DTOs.ServiceItems;
using Temples.Core.Interfaces;

namespace Temples.Api.Controllers;

[ApiController]
[Route("api/service-items")]
public class ServiceItemsController : ControllerBase
{
    private readonly IServiceItemService _service;

    public ServiceItemsController(IServiceItemService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.ServiceItemsView)]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        return Ok(ApiResponse<List<ServiceItemResponse>>.Ok(items));
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = Permissions.ServiceItemsView)]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound(ApiResponse.Fail("找不到該服務項目"));
        return Ok(ApiResponse<ServiceItemResponse>.Ok(item));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.ServiceItemsEdit)]
    public async Task<IActionResult> Create(
        [FromBody] CreateServiceItemRequest request,
        [FromServices] IValidator<CreateServiceItemRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        var item = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, ApiResponse<ServiceItemResponse>.Ok(item, "服務項目已建立"));
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = Permissions.ServiceItemsEdit)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateServiceItemRequest request,
        [FromServices] IValidator<UpdateServiceItemRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        var item = await _service.UpdateAsync(id, request);
        if (item == null) return NotFound(ApiResponse.Fail("找不到該服務項目"));
        return Ok(ApiResponse<ServiceItemResponse>.Ok(item, "服務項目已更新"));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = Permissions.ServiceItemsDelete)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse.Fail("找不到該服務項目"));
        return Ok(ApiResponse.Ok("服務項目已刪除"));
    }

    [HttpPut("sort")]
    [Authorize(Policy = Permissions.ServiceItemsEdit)]
    public async Task<IActionResult> UpdateSortOrder([FromBody] UpdateSortOrderRequest request)
    {
        await _service.UpdateSortOrderAsync(request);
        return Ok(ApiResponse.Ok("排序已更新"));
    }

    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicList()
    {
        var items = await _service.GetActiveListAsync();
        return Ok(ApiResponse<List<PublicServiceItemResponse>>.Ok(items));
    }

    [HttpGet("public/{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicDetail(int id)
    {
        var item = await _service.GetActiveByIdAsync(id);
        if (item == null) return NotFound(ApiResponse.Fail("找不到該服務項目"));
        return Ok(ApiResponse<PublicServiceItemDetailResponse>.Ok(item));
    }
}
