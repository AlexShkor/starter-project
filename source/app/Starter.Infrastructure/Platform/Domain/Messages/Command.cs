using System;
using DQF.Platform.Domain.Interfaces;

namespace DQF.Platform.Domain.Messages
{
    /// <summary>
    /// Domain Command
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <summary>
        /// Command metadata
        /// </summary>
        private ICommandMetadata _metadata = new CommandMetadata();

        /// <summary>
        /// Command metadata
        /// </summary>
        public ICommandMetadata Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }
    }

    public class CommandMetadata : ICommandMetadata
    {
        /// <summary>
        /// Unique Id of Command
        /// </summary>
        public string CommandId { get; set; }

        /// <summary>
        /// User Id of user who initiate this command
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Assembly qualified CLR Type name of Command Type
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Time when command was created
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
