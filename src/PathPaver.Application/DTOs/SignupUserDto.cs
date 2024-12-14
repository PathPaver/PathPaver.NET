namespace PathPaver.Application.DTOs;

/**
 * This DTO is used for user signing up a new user
 *
 * Used in route : api/v1/auth/signup
 */
public record SignupUserDto(string Username, string Email, string Password);