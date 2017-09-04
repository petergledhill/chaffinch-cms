using System;
using System.Collections.Generic;
using System.Text;
using Chaffinch.Core.Events;
using CQRSlite.Domain;

namespace Chaffinch.Core.WriteModel.Domain
{
    public class DocumentType : AggregateRoot
    {
        private DocumentType() { }
        public DocumentType(Guid id, string name)
        {
            Id = id;
            ApplyChange(new DocumentTypeCreated(id, name));
        }

    }
}
