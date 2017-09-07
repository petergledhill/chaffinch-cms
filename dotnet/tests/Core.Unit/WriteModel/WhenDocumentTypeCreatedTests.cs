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
    public class WhenDocumentTypeCreatedTests 
    {
        private readonly Guid _guid;
        private readonly EventSourceHelper _helper;

        public WhenDocumentTypeCreatedTests()
        {
            _guid = Guid.NewGuid();

            _helper = new EventSourceHelper(new List<IEvent>());

            var command = new CreateDocumentType(_guid, "a new document type");

            var handlers = new DocumentTypeCommandHandlers(_helper.Session);
            handlers.Handle(command);
        }

     
        [Fact]
        public void DocumentType_can_be_got_from_session()
        {

            _helper.Session.Get<DocumentType>(Guid.Empty);
        }

        [Fact]
        public void Should_create_one_event()
        {
            Assert.Equal(1, _helper.PublishedEvents.Count);
        }

        [Fact]
        public void Should_create_correct_event()
        {
            Assert.IsType<DocumentTypeCreated>(_helper.PublishedEvents.First());
        }
       
        [Fact]
        public void Should_save_have_correct_name()
        {
            Assert.Equal("a new document type", ((DocumentTypeCreated)_helper.PublishedEvents.First()).Name);
        }

        [Fact]
        public void Should_save_the_correct_id()
        {
            Assert.Equal(_guid, ((DocumentTypeCreated)_helper.PublishedEvents.First()).Id);
        }
    }
}
