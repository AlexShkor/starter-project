namespace DQF.Platform.Domain.Interfaces
{
    /// <summary>
    /// Domain Command interface
    /// </summary>
    public interface ICommand
    {
        string Id { get; set; }
        ICommandMetadata Metadata { get; set; }
    }
}