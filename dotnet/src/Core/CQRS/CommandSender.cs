using CQRSlite.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chaffinch.Core.CQRS
{
    /// <summary>
    /// A wrapper for CQRSLites CommandSender, so as not require CQRSLite in other libraries
    /// </summary>
    public class CommandSender : ICommandSender
    {
        private readonly CQRSlite.Commands.ICommandSender _cqrsLiteCommandSender;

        public CommandSender(CQRSlite.Commands.ICommandSender cqrsLiteCommandSender)
        {
            _cqrsLiteCommandSender = cqrsLiteCommandSender;
        }

        /// <summary>
        /// Trigger the command so it can be processed by the relevent handler(s)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Send<T>(T command, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ICommand
        {
            return _cqrsLiteCommandSender.Send<T>(command, cancellationToken);
        }
    }
}
