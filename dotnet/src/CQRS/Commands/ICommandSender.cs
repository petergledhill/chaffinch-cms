using CQRSlite.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chaffinch.CQRS.Commands
{
    public interface ICommandSender
    {
        Task Send<T>(T command, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ICommand;
    }
}
