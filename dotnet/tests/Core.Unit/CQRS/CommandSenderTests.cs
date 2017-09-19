using Chaffinch.Core.CQRS;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Chaffinch.Core.Unit.CQRS
{
    public class CommandSenderTests
    {
        [Fact]
        public async Task test_send_command_wraps_cqrs_lite()
        {          
            var cqrsLiteCommandSender = new Mock<CQRSlite.Commands.ICommandSender>();
            var commandSender = new CommandSender(cqrsLiteCommandSender.Object);
            
            var mockCommand = new Mock<CQRSlite.Commands.ICommand>();
            var cancelationToken = default(CancellationToken);

            await commandSender.Send(mockCommand.Object, cancelationToken);

            cqrsLiteCommandSender.Verify(m => m.Send(mockCommand.Object, cancelationToken));
        }
    }
}
