using EMQTest.Common.Messaging.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMQTest.Messages.Commands
{
    public class HelloCommand : ICommand
    {
        public Guid CommandId { get; set; }
    }
}
