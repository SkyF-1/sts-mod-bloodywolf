namespace StsModBloodywolf.Loader;

internal sealed class ModLoadException : Exception
{
    public ModLoadException(string message) : base(message)
    {
    }

    public ModLoadException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
