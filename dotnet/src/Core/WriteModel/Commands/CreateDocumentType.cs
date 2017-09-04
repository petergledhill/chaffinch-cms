using System;
using System.Collections.Generic;
using System.Text;
using CQRSlite.Commands;

namespace Chaffinch.Core.WriteModel.Commands
{
    public class CreateDocumentType : ICommand
    {
        public readonly string Name;

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }

        public CreateDocumentType(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
