using AuthService.API.DTOs;
using AuthService.API.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace AuthService.API.Services;

public class UserInternalClient(IHttpClientFactory httpClientFactory) : IUserInternalClient
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("UserServiceClient");

    public async Task<Result<UserProfileResponse?>> GetUserByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/internal/v1/users/{id}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Result.Fail<UserProfileResponse?>(Error.Validation("User not found"));

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserProfileResponse?>>(cancellationToken: cancellationToken);
        if (apiResponse is null)
            return Result.Fail<UserProfileResponse?>(Error.Failure("Invalid response format"));
        return !apiResponse.Success
            ? Result.Fail<UserProfileResponse?>(apiResponse.Error ?? Error.Failure("Unknown error"))
            : Result.Ok(apiResponse.Data);
    }

    public async Task<Result<UserProfileResponse?>> CreateUserProfileAsync(CreateUserProfileInternalRequest payload,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/internal/v1/users", payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Result.Fail<UserProfileResponse?>(Error.Failure($"UserService error: {response.StatusCode}"));

        var apiResp = await response.Content.ReadFromJsonAsync<ApiResponse<UserProfileResponse?>>(cancellationToken: cancellationToken);
        if (apiResp is null)
            return Result.Fail<UserProfileResponse?>(Error.Failure("Invalid response format"));

        return !apiResp.Success
            ? Result.Fail<UserProfileResponse?>(apiResp.Error ?? Error.Failure("Unknown error"))
            : Result.Ok(apiResp.Data);
    }

    public async Task<Result> DeleteUserProfileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"/api/internal/v1/users/{id}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Result.Fail(Error.Failure($"UserService error: {response.StatusCode}"));

        var apiResp = await response.Content.ReadFromJsonAsync<ApiResponse>(cancellationToken: cancellationToken);
        if (apiResp is null)
            return Result.Fail(Error.Failure("Invalid response format"));

        return !apiResp.Success
            ? Result.Fail(apiResp.Error ?? Error.Failure("Unknown error"))
            : Result.Ok();
    }
}