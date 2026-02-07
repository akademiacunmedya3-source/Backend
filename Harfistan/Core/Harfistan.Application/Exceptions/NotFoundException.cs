namespace Harfistan.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message){}
    public NotFoundException(string name, object key) : base($"{name} with given key: {key} not found."){}
}
public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string message) : base(message){}
    public AlreadyExistsException(string name, object key) : base($"{name} with given key: {key}  already exists."){}
}