using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//TODO: Too many static variables

namespace CustomerLoyaltyProgram
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class CustomerManager : MonoBehaviour
    {
        private static System.Random random = new System.Random();
        private static int cap = 16;

        //This doesn't do anything yet but it will		
        private CustomerManager() { }
        private static Dictionary<string, CustomerRecord> _archivedCustomers;
        private static Dictionary<string, CustomerRecord> _reservedCustomers;
        private static CustomerManager instance;
        public static CustomerManager Instance {
            get {
                if (instance == null) {
                    Debug.Log("Customer Manager instance is being instantiated.");
                    instance = new CustomerManager();
                }
                return instance;
            }
        }

        public void Awake() {
            Debug.Log("Customer Manager is awake");
            GameEvents.onKerbalRemoved.Add(OnKerbalRemoved);
			GameEvents.onKerbalAdded.Add(OnKerbalAdded);
            GameEvents.onKerbalTypeChange.Add(OnKerbalTypeChange);
            GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
        }
		
		public void Start() {
			_archivedCustomers = CustomerSave.ArchivedCustomers();
			_reservedCustomers = CustomerSave.ReservedCustomers();
		}

        public void OnGameSceneLoadRequested(GameScenes scene) {
            UnloadEvents();
        }

		public void UnloadEvents() {
            GameEvents.onKerbalRemoved.Remove(OnKerbalRemoved);
            GameEvents.onKerbalAdded.Remove(OnKerbalAdded);
            GameEvents.onKerbalTypeChange.Remove(OnKerbalTypeChange);
            GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
        }

        public void OnKerbalRemoved(ProtoCrewMember pcm) {
//            Debug.Log("Removing " + pcm.name);
			bool located = false;
			if (CustomerSave.ReservedCustomers().ContainsKey(pcm.name)) {
		        CustomerSave.ReservedCustomers().Remove(pcm.name);
			    if (pcm.rosterStatus != ProtoCrewMember.RosterStatus.Dead)
                    ArchiveKerbal(pcm);
	    		located = true;
            }
            if ((!located) && (pcm.type == ProtoCrewMember.KerbalType.Tourist)) {
//                Debug.Log("Archiving - OKR " + pcm.name);
                ArchiveKerbal(pcm);
//                Debug.Log("Archived - OKR " + pcm.name);
                ListKerbal(pcm);
				located = true;
            }

//            Debug.Log("End of OnKerbalRemoved " + pcm.name);
        }

        public void OnKerbalAdded(ProtoCrewMember pcm) {
//            Debug.Log("Added!" + pcm.name.ToString() + " " + pcm.type.ToString() + " " + pcm.experienceTrait.ToString());
			bool located = false;
            if (CustomerSave.ArchivedCustomers().ContainsKey(pcm.name)) {
					ReplaceKerbal(pcm, CustomerSave.ArchivedCustomers()[pcm.name].kerbal);
					located = true;
			}
            if ((!located) && ((pcm.type == ProtoCrewMember.KerbalType.Unowned) || (pcm.type == ProtoCrewMember.KerbalType.Applicant))) {

//				Debug.Log("*****************************Valid Applicant!************************************************");

				int countRandom = random.Next(0, cap);
                int count = CustomerSave.ArchivedCustomers().Count();
                string spaceJunkie = null;
//               Debug.Log(count);
                foreach (KeyValuePair<string, CustomerRecord> c in CustomerSave.ArchivedCustomers()) {
                    Debug.Log(count);
                    if (countRandom == 0) {
//                        Debug.Log("*****************************Let's Do This!************************************************");
                        spaceJunkie = c.Key;
                        located = true;
                    }
                    countRandom = countRandom - 1;
                }
                if (spaceJunkie != null)
                    ReplaceKerbal(pcm, CustomerSave.ArchivedCustomers()[spaceJunkie].kerbal);
            }
        }

		public void OnKerbalTypeChange(ProtoCrewMember pcm, ProtoCrewMember.KerbalType oldType, ProtoCrewMember.KerbalType newType) {
			bool located = false;
            if ((oldType == ProtoCrewMember.KerbalType.Applicant) && (newType == ProtoCrewMember.KerbalType.Crew)) {
                if (CustomerSave.ReservedCustomers().ContainsKey(pcm.name)) {
                        CustomerSave.ReservedCustomers().Remove(pcm.name);
                        located = true;
                }
            }
         
            if ((oldType == ProtoCrewMember.KerbalType.Unowned) && (newType == ProtoCrewMember.KerbalType.Tourist)) {
                if (CustomerSave.ReservedCustomers().ContainsKey(pcm.name)) {
                        CustomerSave.ReservedCustomers().Remove(pcm.name);
                        located = true;
                }
            }
        }

        /*
        public void OnContractOffered(Contracts.Contract contract)
        {
            System.Type type = contract.GetType();
            if (contract is FinePrint.Contracts.TourismContract);
            {

            }
            foreach (var p in contract.AllParameters)
            {
                Debug.Log(p);
            }
            Debug.Log(contract.Keywords);
            Debug.Log(contract.AllParameters);
            Debug.Log(contract.GetType());
            Debug.Log(contract.Notes);
            Debug.Log(contract.Title);

        }
*/
        public void ArchiveKerbal(ProtoCrewMember pcm) {
            Debug.Log("Archiving " + pcm.name);
            CustomerSave.ArchivedCustomers()[pcm.name] = new CustomerRecord(pcm);
        }

        public void ReserveCustomer(ProtoCrewMember customer) {
            CustomerSave.ReservedCustomers()[customer.name] = CustomerSave.ArchivedCustomers()[customer.name];
            CustomerSave.ArchivedCustomers().Remove(customer.name);
        }
		
		public void ReplaceKerbal(ProtoCrewMember to, ProtoCrewMember from) {
            Debug.Log(from.name + " is bumping " + to.name);
            ReserveCustomer(from);
			ReeducateKerbal(to, from);
        }

        public void ReeducateKerbal(ProtoCrewMember to, ProtoCrewMember from) {
            to.name = from.name;
            to.courage = from.courage;
            to.stupidity = from.stupidity;
            to.isBadass = from.isBadass;
            to.gender = from.gender;
            to.hasToured = from.hasToured;
            TransferCareerLog(to, from);
        }

        public void TransferCareerLog(ProtoCrewMember to, ProtoCrewMember from) {
            // record current flightlog & reset, if applicable
            FlightLog current = null;
            if (to.flightLog.Entries.Count() > 0) {
                current = new FlightLog();
                foreach (FlightLog.Entry entry in to.flightLog.Entries)
                    current.Entries.Add(entry);
                to.flightLog = new FlightLog();
            }
            
            //transfer careerLog
            foreach (FlightLog flight in from.careerLog.GetFlights()) {
                foreach (FlightLog.Entry entry in flight.Entries)
                    to.flightLog.Entries.Add(entry);
                to.ArchiveFlightLog();
            }

            //rewrite flightLog, if applicable
            if (current != null)
                foreach (FlightLog.Entry entry in current.Entries)
                    to.flightLog.Entries.Add(entry);
        }

        // this is O(MN) DWI
        // also doesn't do  anything yet
        public void KerbalCleanup()
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, CustomerRecord> rc in CustomerSave.ReservedCustomers())
            {
                bool found = false;
                foreach (ProtoCrewMember pcm in HighLogic.CurrentGame.CrewRoster.Unowned)
                {
                    if (pcm.name == rc.Key)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    list.Add(rc.Key);
            }
            foreach (string s in list)
            {

            }
        }

        public void ListKerbal(ProtoCrewMember pcm) {
            Debug.Log(pcm.name + " " + pcm.gender + " " + pcm.courage + " " + pcm.stupidity + " " + pcm.isBadass + " " + pcm.hasToured + " " + pcm.type + " " + pcm.rosterStatus);
            Debug.Log(pcm.flightLog);
        }
		
        public string StringListString(List<string> list) {
            string newstring = "";
            foreach (string s in list) {
                newstring = newstring + s + " ";
            }
            return newstring;
        }

        void OnDestroy() {
            Debug.Log("Manager Destroying!");
            UnloadEvents();
            Debug.Log("Manager Destroyed!!!");
        }
    }
}
