namespace PathPaver.Application.DTOs;

/**
 * This DTO is used for user authentication
 *
 * Used in route : api/v1/auth/login
 */
public record UpdateUserDto(string Email, string Password, string NewEmail, string NewPassword);
