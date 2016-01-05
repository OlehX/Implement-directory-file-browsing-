using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApiPagingAngularClient.Models
{
    public class DriveModels:Dyrectory { 
       
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

    }
}