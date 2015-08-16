using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomerSatisfactionProgram
{
    public class CustomerRecord
    {
        public ProtoCrewMember kerbal { get; set;}
        public string origin { get; set; }
        public string status { get; set; }

        public CustomerRecord()
        {
        }

        public CustomerRecord(ProtoCrewMember pcm)
        {
            kerbal = pcm;
            origin = "TOURIST";
            status = "ARCHIVED";
        }

        public string Name()
        {
            return kerbal.name;
        }        
    } 
}