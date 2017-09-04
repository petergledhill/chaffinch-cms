using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chaffinch.Core.WriteModel.Commands;
using Chaffinch.Core.WriteModel.Domain;
using CQRSlite.Commands;
using CQRSlite.Domain;

namespace Chaffinch.Core.WriteModel.Handlers
{
    public class DocumentTypeCommandHandlers : ICommandHandler<CreateDocumentType>
    {
        private readonly ISession _session;

        public DocumentTypeCommandHandlers(ISession session)
        {
            _session = session;
        }

        public async Task Handle(CreateDocumentType message)
        {
              var item = new DocumentType(message.Id, message.Name);
              await _session.Add(item);
              await _session.Commit();
        }
    }
}
