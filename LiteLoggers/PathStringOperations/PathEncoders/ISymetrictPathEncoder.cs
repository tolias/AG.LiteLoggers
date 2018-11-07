namespace AG.PathStringOperations.PathEncoders
{
    public interface ISymetrictPathEncoder : IPathEncoder
    {
        string DecodeFileName(string encodedFileName);
    }
}
