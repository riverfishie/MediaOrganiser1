using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MediaOrganiser1.Models
{
    public class FileClass
    {
        [Key]

        public int FileClassId { get; set; }

        public string Name { get; set; }

        public byte[] File { get; set; }

        public string ContentType { get; set; }

        public string Category { get; set; }

        public string Genre { get; set; }

        //public string FileName { get; set; }
    }
}