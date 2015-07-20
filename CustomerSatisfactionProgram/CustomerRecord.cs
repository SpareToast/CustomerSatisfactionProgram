using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomerLoyaltyProgram
{
    public class CustomerRecord
    {
        
        public string name { get; set;}
        public string origin { get; set; }
/*      currently piggybacking on the kerbal save function

        public string gender { get; set; }
        public float courage { get; set; }
        public float stupidity { get; set; }
        public bool isBadass { get; set; }
        public FlightLog flightLog { get; set; }
        public FlightLog careerLog { get; set; }
        public string type { get; set; }
        public string rosterStatus { get; set; }
*/
        public CustomerRecord()
        {
        }

        public CustomerRecord(ProtoCrewMember pcm)
        {
            name = pcm.name;
            origin = "TOU";

/*      currently piggybacking on the kerbal save function

            if (pcm.gender == ProtoCrewMember.Gender.Male)
                gender = "Male";
            else gender = "Female";
            courage = pcm.courage;
            stupidity = pcm.stupidity;
            isBadass = pcm.isBadass;
            flightLog = pcm.flightLog;
            careerLog = pcm.careerLog;
            switch (pcm.type)
            {
                case ProtoCrewMember.KerbalType.Applicant:
                    type = "Applicant";
                    break;
                case ProtoCrewMember.KerbalType.Crew:
                    type = "Crew";
                    break;
                case ProtoCrewMember.KerbalType.Tourist:
                    type = "Tourist";
                    break;
                default:
                    type = "Unowned";
                    break;
            }

            switch (pcm.rosterStatus)
            {
                case ProtoCrewMember.RosterStatus.Assigned:
                    rosterStatus = "Assigned";
                    break;
                case ProtoCrewMember.RosterStatus.Available:
                    rosterStatus = "Available";
                    break;
                case ProtoCrewMember.RosterStatus.Dead:
                    rosterStatus = "Dead";
                    break;
                default:
                    rosterStatus = "Missing";
                    break;
            }
*/  }

/*        public ProtoCrewMember ToProtoCrewMember()
        {
            ProtoCrewMember amnesiac = CrewGenerator.RandomCrewMemberPrototype();
            amnesiac.name = name;
            if (gender == "Male")
                amnesiac.gender = ProtoCrewMember.Gender.Male;
            else amnesiac.gender = ProtoCrewMember.Gender.Female;
            amnesiac.courage = courage;
            amnesiac.stupidity = stupidity;
            amnesiac.isBadass = isBadass;
            amnesiac.flightLog = flightLog;
            amnesiac.careerLog = careerLog;
           
            switch (type)
            {
                case "Applicant":
                    amnesiac.type = ProtoCrewMember.KerbalType.Applicant;
                    break;
                case "Crew":
                    amnesiac.type = ProtoCrewMember.KerbalType.Crew;
                    break;
                case "Tourist":
                    amnesiac.type = ProtoCrewMember.KerbalType.Tourist;
                    break;
                default:
                    amnesiac.type = ProtoCrewMember.KerbalType.Unowned;
                    break;
            }

            switch (rosterStatus)
            {
                case "Assigned":
                    amnesiac.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
                    break;
                case "Available":
                    amnesiac.rosterStatus = ProtoCrewMember.RosterStatus.Available;
                    break;
                case "Dead":
                    amnesiac.rosterStatus = ProtoCrewMember.RosterStatus.Dead;
                    break;
                default:
                    amnesiac.rosterStatus = ProtoCrewMember.RosterStatus.Missing;
                    break;
            }

            return amnesiac;
        }


        public ProtoCrewMember OverwriteProtoCrewMember(ProtoCrewMember amnesiac)
        {
            amnesiac.name = name;
            if (gender == "Male")
                amnesiac.gender = ProtoCrewMember.Gender.Male;
            else amnesiac.gender = ProtoCrewMember.Gender.Female;
            amnesiac.courage = courage;
            amnesiac.stupidity = stupidity;
            amnesiac.isBadass = isBadass;
            amnesiac.flightLog = flightLog;
            amnesiac.careerLog = careerLog;

            switch (type)
            {
                case "Applicant":
                    amnesiac.type = ProtoCrewMember.KerbalType.Applicant;
                    break;
                case "Crew":
                    amnesiac.type = ProtoCrewMember.KerbalType.Crew;
                    break;
                case "Tourist":
                    amnesiac.type = ProtoCrewMember.KerbalType.Tourist;
                    break;
                default:
                    amnesiac.type = ProtoCrewMember.KerbalType.Unowned;
                    break;
            }

            switch (rosterStatus)
            {
                case "Assigned":
                    amnesiac.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
                    break;
                case "Available":
                    amnesiac.rosterStatus = ProtoCrewMember.RosterStatus.Available;
                    break;
                case "Dead":
                    amnesiac.rosterStatus = ProtoCrewMember.RosterStatus.Dead;
                    break;
                default:
                    amnesiac.rosterStatus = ProtoCrewMember.RosterStatus.Missing;
                    break;
            }
            return amnesiac;
        }
*/  } 
}