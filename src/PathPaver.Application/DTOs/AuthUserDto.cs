namespace PathPaver.Application.DTOs;

/**
 * This DTO is used for user authentication
 *
 * Used in route : api/v1/auth/login
 */
public record AuthUserDto(string Email, string Password);
