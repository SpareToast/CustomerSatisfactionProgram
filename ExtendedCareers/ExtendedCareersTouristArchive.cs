using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExtendedCareers
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    class ExtendedCareersTouristArchive : MonoBehaviour
    {
		// Static singleton instance
		private static ExtendedCareersTouristArchive instance;

        private List<ProtoCrewMember> ProtoTouristArchive;
        private List<string> TouristArchive;
        private List<string> ReservedTouristArchive;
		private int odds = 1;
		
		private System.Random random = new System.Random();
		
		private ExtendedCareersTouristArchive()
		{
			
		}
		
		// Static singleton property
		public static ExtendedCareersTouristArchive Instance
		{
			get
			{
				if (instance==null)
				{
					instance = new ExtendedCareersTouristArchive();
				}
				return instance;
			}
		}

        void Awake()
        {
            Debug.Log("ExtendedCareers TouristArchive - Awake");
            try
            {
                Debug.Log("ExtendedCareers TouristArchive - Awaked");
                GameEvents.onKerbalAdded.Add(OnKerbalAdded);
                GameEvents.onKerbalRemoved.Add(OnKerbalRemoved);
                if (ProtoTouristArchive == null)
                    ProtoTouristArchive = new List<ProtoCrewMember>();
                if (TouristArchive == null)
                    TouristArchive = new List<string>();
                if (ReservedTouristArchive == null)
                    ReservedTouristArchive = new List<string>();
            }
            catch (Exception ex)
            {
                Debug.Log("ExtendedCareers TouristArchive - Awake error: " + ex.Message + ex.StackTrace);
            }
            
        }

		void OnKerbalAdded(ProtoCrewMember pcm)
		{
            Debug.Log("Added!" + pcm.name.ToString() + " " + pcm.type.ToString());
            if (pcm.type == ProtoCrewMember.KerbalType.Applicant)
            {
                Debug.Log("*****************************Valid Applicant!************************************************");
			    int count = TouristArchive.Count();
			    int countRandom = random.Next(0, count*odds);

                if (countRandom < count)
		    	{
                    Debug.Log("*****************************Let's Do This!************************************************");
                    string spaceJunkieName = TouristArchive[countRandom];
                    ProtoCrewMember spaceJunkie = null;
                    foreach (ProtoCrewMember k in HighLogic.CurrentGame.CrewRoster.Unowned)
                    {
                        if (k.name == spaceJunkieName)
                        {
                            spaceJunkie = k;
                        }
                    }
                    if (spaceJunkie != null)
                    {
                        ReserveTourist(countRandom, "Applicant");
                        Debug.Log("*****************************" + pcm.name + "************************************************");
                        pcm = spaceJunkie;
                        Debug.Log("*****************************" + pcm.name + "************************************************");
                    }
                }
    	    }
		}
		
		public void OnKerbalRemoved(ProtoCrewMember pcm)
		{
            Debug.Log("Removing " + pcm.name);
            string name = pcm.name;
			if ((pcm.type == ProtoCrewMember.KerbalType.Tourist)&&(!TouristArchive.Contains(pcm.name))) {
                Debug.Log("Archiving - OKR " + name);
  				ArchiveKerbal(pcm);
                Debug.Log("Archived - OKR " + name);
                ListKerbal(pcm);
	    	}   else if (ReservedTouristArchive.Count>0){
                foreach (string k in ReservedTouristArchive)
			    {
                   	if (k == pcm.name)
				   	{
                   		ReservedTouristArchive.Remove(k);
                        ArchiveKerbal(pcm);
				   	}
				}
			}
            Debug.Log("End of OnKerbalRemoved " + name);
		}

        void ArchiveKerbal(ProtoCrewMember pcm)
        {
            Debug.Log("Archiving - AK " + pcm.name);
            //spawn a crewmember to crib mystery stats off
            ProtoCrewMember amnesiac = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
            Debug.Log("Cloned " + amnesiac.name);
            //teach Kerbal how to be themselves again
            amnesiac.name = pcm.name;
			amnesiac.gender = pcm.gender;
			amnesiac.courage = pcm.courage;
            amnesiac.stupidity = pcm.stupidity;
            amnesiac.isBadass = pcm.isBadass;
            amnesiac.flightLog = pcm.flightLog;
            Debug.Log("Re-Educated " + amnesiac.name);
            //set them to be hidden nobodies
            amnesiac.type = ProtoCrewMember.KerbalType.Unowned;
            amnesiac.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
            Debug.Log("Hid " + amnesiac.name);
            //add them to the list
            TouristArchive.Add(amnesiac.name);
            Debug.Log("Listed " + amnesiac.name);
        }

        public void ListKerbal(ProtoCrewMember pcm)
        {
            Debug.Log(pcm.name + " " + pcm.gender + " " + pcm.courage + " " + pcm.stupidity + " " + pcm.isBadass + " " + pcm.hasToured + " " + pcm.type + " " + pcm.rosterStatus);
            Debug.Log(pcm.flightLog);
        }

		public string ReserveTourist(int location, string purpose)
		{
            if ((location >= 0) && (location < TouristArchive.Count()))
			{
				string careerKerbal = TouristArchive[location];
				ReservedTouristArchive.Add(careerKerbal);
				TouristArchive.Remove(careerKerbal);
				return careerKerbal;
			}
			else return null;
		}
	}
}