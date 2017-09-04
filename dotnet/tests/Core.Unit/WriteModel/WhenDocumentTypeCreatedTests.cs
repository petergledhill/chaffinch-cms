using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Chaffinch.Core.WriteModel.Commands;
using CQRSlite.Commands;
using CQRSlite.Domain;
using System.Threading.Tasks;
using Chaffinch.Core.Events;
using Chaffinch.Core.WriteModel.Domain;
using Chaffinch.Core.WriteModel.Handlers;
using CQRSlite.Bus;
using CQRSlite.Events;
using Xunit;

namespace Chaffinch.Core.Unit.WriteModel
{
    public class WhenDocumentTypeCreatedTests : Specification<DocumentType, DocumentTypeCommandHandlers, CreateDocumentType>
    {
        private Guid _guid;
        protected override DocumentTypeCommandHandlers BuildHandler()
        {
            return new DocumentTypeCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _guid = Guid.NewGuid();
            return new List<IEvent> { };
        }

        protected override CreateDocumentType When()
        {
            return new CreateDocumentType(_guid, "a new document type");
        }

        [Fact]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Fact]
        public void Should_create_correct_event()
        {
            Assert.IsType<DocumentTypeCreated>(PublishedEvents.First());
        }
       
        [Fact]
        public void Should_save_have_correct_name()
        {
            Assert.Equal("a new document type", ((DocumentTypeCreated)PublishedEvents.First()).Name);
        }
    }
}
