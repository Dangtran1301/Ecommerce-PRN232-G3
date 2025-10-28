using AuthService.API.DTOs;
using AuthService.API.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace AuthService.API.Services;

public class UserInternalClient(IHttpClientFactory httpClientFactory) : IUserInternalClient
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("UserServiceClient");

    public async Task<Result<UserServiceUserDto?>> CreateUserProfileAsync(CreateUserProfileInternalRequest payload,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/internal/v1/users", payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Result.Fail<UserServiceUserDto?>(Error.Failure($"UserService error: {response.StatusCode}"));

        var apiResp = await response.Content.ReadFromJsonAsync<ApiResponse<UserServiceUserDto?>>(cancellationToken: cancellationToken);
        if (apiResp is null)
            return Result.Fail<UserServiceUserDto?>(Error.Failure("Invalid response format"));

        return !apiResp.Success
            ? Result.Fail<UserServiceUserDto?>(apiResp.Error ?? Error.Failure("Unknown error"))
            : Result.Ok(apiResp.Data);
    }

    public async Task<Result<UserServiceUserDto?>> GetUserByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/internal/v1/users/{id}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Result.Fail<UserServiceUserDto?>(Error.Validation("User not found"));

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserServiceUserDto?>>(cancellationToken: cancellationToken);
        if (apiResponse is null)
            return Result.Fail<UserServiceUserDto?>(Error.Failure("Invalid response format"));
        return !apiResponse.Success
            ? Result.Fail<UserServiceUserDto?>(apiResponse.Error ?? Error.Failure("Unknown error"))
            : Result.Ok(apiResponse.Data);
    }
}