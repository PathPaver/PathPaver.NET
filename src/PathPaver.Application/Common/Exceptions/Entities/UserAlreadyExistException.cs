namespace PathPaver.Application.Common.Exceptions.Entities;

public class UserAlreadyExistException() 
    : Exception("This email is already associated to an account");
