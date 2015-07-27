using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomerLoyaltyProgram
{
    public class CustomerRecord
    {

        public ProtoCrewMember kerbal { get; set;}
        public string origin { get; set; }

        public CustomerRecord()
        {
        }

        public CustomerRecord(ProtoCrewMember pcm)
        {
            kerbal = pcm;
            origin = "TOURIST";
        }

        public string Name()
        {
            return kerbal.name;
        }
        
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