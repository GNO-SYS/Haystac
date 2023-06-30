namespace Haystac.Application.Common.Exceptions;

//< From: https://github.com/jasontaylordev/CleanArchitecture/blob/main/src/Application/Common/Exceptions/NotFoundException.cs
public class NotFoundException : Exception
{
    public NotFoundException()
        : base()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}
