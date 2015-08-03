using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//TODO: Too many static variables

namespace CustomerSatisfactionProgram
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class CustomerManager : MonoBehaviour
    {
        private static ConfigNode settings;
        private static System.Random random = new System.Random();
        private static int cap = 16;
        private static int cleanup = 0;
        private static string version = "0.9.0";

/*        //This doesn't do anything yet but it will		
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
*/
        public void Awake() {
            Debug.Log("********************************************************************" + HighLogic.LoadedScene + "*********************************************************************");
            settings = ConfigNode.Load("GameData/CustomerSatisfactionProgram/Config.cfg");

            if (settings.HasNode("SETTINGS")) {
                Debug.Log("Loading Settings");
                settings = settings.GetNode("SETTINGS");

                if (settings.HasValue("VERSION")) {
                    version = (settings.GetValue("VERSION"));
                }
                if (settings.HasValue("CAP")) {
                    cap = (int.Parse(settings.GetValue("CAP")));
                }
                if (settings.HasValue("CLEANUP")) {
                    cleanup = (int.Parse(settings.GetValue("CLEANUP")));
                    if (cleanup == 2) {
                        settings.AddValue("CLEANUP", 0);
                    }
                }
            }

            GameEvents.onKerbalRemoved.Add(OnKerbalRemoved);
			GameEvents.onKerbalAdded.Add(OnKerbalAdded);
            GameEvents.onKerbalTypeChange.Add(OnKerbalTypeChange);
            GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
        }
		
		public void Start() {
            //			_archivedCustomers = CustomerSave.ArchivedCustomers();
            //			_reservedCustomers = CustomerSave.ReservedCustomers();

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
			bool located = false;
			if (CustomerSave.ReservedCustomers().ContainsKey(pcm.name)) {
                Debug.Log(pcm.name + " has gone home");
		        CustomerSave.ReservedCustomers().Remove(pcm.name);
			    if (pcm.rosterStatus != ProtoCrewMember.RosterStatus.Dead)
                    ArchiveKerbal(pcm);
	    		located = true;
            }
            if ((!located) && (pcm.type == ProtoCrewMember.KerbalType.Tourist)) {
                ArchiveKerbal(pcm);
                ListKerbal(pcm);
				located = true;
            }
        }

//      when a new kerbal is added, potentially replaces with an archived customer
        public void OnKerbalAdded(ProtoCrewMember pcm) {
			bool located = false;
            if (CustomerSave.ArchivedCustomers().ContainsKey(pcm.name)) {
                    Debug.Log(pcm.name + " has returned on their own!");
					ReplaceKerbal(pcm, CustomerSave.ArchivedCustomers()[pcm.name].kerbal);
					located = true;
			}

            if ((!located) && ((pcm.type == ProtoCrewMember.KerbalType.Unowned) || (pcm.type == ProtoCrewMember.KerbalType.Applicant))) {
				int countRandom = random.Next(0, cap);
                int count = CustomerSave.ArchivedCustomers().Count();
                string spaceJunkie = null;
                Debug.Log("Is " + count + " > " + countRandom + "?");

                foreach (KeyValuePair<string, CustomerRecord> c in CustomerSave.ArchivedCustomers()) {
                    if (countRandom == 0) {
                        spaceJunkie = c.Key;
                        located = true;
                    }
                    countRandom = countRandom - 1;
                }
                if (spaceJunkie != null) {
                    if ((pcm.type == ProtoCrewMember.KerbalType.Unowned) && (pcm.experienceTrait.TypeName != "Tourist"))
                        CustomerSave.ArchivedCustomers()[spaceJunkie].status = "CASTAWAY";
                    if ((pcm.type == ProtoCrewMember.KerbalType.Unowned) && (pcm.experienceTrait.TypeName == "Tourist"))
                        CustomerSave.ArchivedCustomers()[spaceJunkie].status = "TOURIST";
                    if (pcm.type == ProtoCrewMember.KerbalType.Applicant)
                        CustomerSave.ArchivedCustomers()[spaceJunkie].status = "APPLICANT";
                    ReplaceKerbal(pcm, CustomerSave.ArchivedCustomers()[spaceJunkie].kerbal);
                }
            }
        }

		public void OnKerbalTypeChange(ProtoCrewMember pcm, ProtoCrewMember.KerbalType oldType, ProtoCrewMember.KerbalType newType) {
			bool located = false;
            if ((oldType == ProtoCrewMember.KerbalType.Applicant) && (newType == ProtoCrewMember.KerbalType.Crew)) {
                if (CustomerSave.ReservedCustomers().ContainsKey(pcm.name)) {
                        Debug.Log(pcm.name + " has been hired");
                        CustomerSave.ReservedCustomers().Remove(pcm.name);
                        located = true;
                }
            }

            if ((oldType == ProtoCrewMember.KerbalType.Unowned) && (newType == ProtoCrewMember.KerbalType.Tourist)) {
                if (CustomerSave.ReservedCustomers().ContainsKey(pcm.name))
                {
                    Debug.Log(pcm.name + " is going to space");
                    CustomerSave.ReservedCustomers().Remove(pcm.name);
                    located = true;
                }
            }

            if ((oldType == ProtoCrewMember.KerbalType.Unowned) && (newType == ProtoCrewMember.KerbalType.Crew)) {
                if (CustomerSave.ReservedCustomers().ContainsKey(pcm.name)) {
                        Debug.Log(pcm.name + " has been rescued");
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
        }
*/
        public void ArchiveKerbal(ProtoCrewMember pcm) {
            Debug.Log("Archiving " + pcm.name);
            pcm.type = ProtoCrewMember.KerbalType.Crew;
            KerbalRoster.SetExperienceTrait(pcm);
            CustomerRecord customer = new CustomerRecord(pcm);
            customer.status = "ARCHIVED";
            CustomerSave.ArchivedCustomers()[pcm.name] = customer;

        }

        public void ReserveCustomer(ProtoCrewMember pcm) {
            Debug.Log("Reserving " + pcm.name);
            CustomerSave.ReservedCustomers()[pcm.name] = CustomerSave.ArchivedCustomers()[pcm.name];
            CustomerSave.ArchivedCustomers().Remove(pcm.name);
        }
		
		public void ReplaceKerbal(ProtoCrewMember to, ProtoCrewMember from) {
            Debug.Log(from.name + " is bumping " + to.name);
            ReserveCustomer(from);
			UpdateTicket(to, from);
        }

        public void UpdateTicket(ProtoCrewMember to, ProtoCrewMember from) {
            Debug.Log(to.name + " is giving " + from.name + " their ticket");
            to.name = from.name;
            to.courage = from.courage;
            to.stupidity = from.stupidity;
            to.isBadass = from.isBadass;
            to.gender = from.gender;
            to.experienceTrait = from.experienceTrait;
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
            foreach (KeyValuePair<string, CustomerRecord> cr in CustomerSave.ReservedCustomers())
            {
                if (HighLogic.CurrentGame.CrewRoster.Exists(cr.Value.Name())) ;
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
