namespace PathPaver.Application.Common.Exceptions.Entities;

public class UserNotFoundException(string email) 
    : Exception($"User associated to the email {email} can't be found");