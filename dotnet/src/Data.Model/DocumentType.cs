using System;
using System.Collections.Generic;

namespace Data.Model
{
    /*
    public class DocumentTypeDoc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public IEnumerable<>
    }

    public class DocumentTypePropertyDoc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ValueType { get; set; }
    }*/

    public class Document
    {
        public int Id { get; set; }
        public DocumentType DocumentType { get; set; }
    }

    public class DocumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DocumentTypeProperty> Properties { get; set; }
    }

    public class DocumentValue
    {
        public int Id { get; set; }
        public Language Language { get; set; } //nullable?
        public DocumentTypeProperty Property { get; set; }
        public int ValueInt { get; set; }
        public string ValueString { get; set; }
    }

    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DocumentTypeProperty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ValueType ValueType { get; set; }
    }

    public enum ValueType
    {
        Int,
        String        
    }
    
}
