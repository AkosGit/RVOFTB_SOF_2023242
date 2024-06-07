using M7CarManager.Models;
using SnipSmart.Models;

namespace SnipSmart.Services;

public interface IAuthService
{
    Task<(int, string)> Registeration(RegisterViewModel model, string role);
    Task<(int, string)> Login(LoginViewModel model);
}