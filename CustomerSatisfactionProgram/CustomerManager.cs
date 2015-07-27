using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomerLoyaltyProgram
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class CustomerManager : MonoBehaviour
    {
        private static System.Random random = new System.Random();
        private static int odds = 1;

        private static CustomerManager instance;

        private CustomerManager() { }
        
        public static CustomerManager Instance
        {
            get 
            {
                if (instance == null)
                {
                    Debug.Log("Customer Manager instance is being instantiated.");
                    instance = new CustomerManager();
                }
                return instance;
            }
        }

        public void Awake()
        {
            Debug.Log("Customer Manager is awake Events");
            GameEvents.onKerbalRemoved.Add(OnKerbalRemoved);
            GameEvents.onGameSceneLoadRequested.Add(UnloadEvents);
        }

        public void OnKerbalRemoved(ProtoCrewMember pcm)
        {
            Debug.Log("Removing " + pcm.name);
            if ((pcm.type == ProtoCrewMember.KerbalType.Tourist))
            {
                Debug.Log("Archiving - OKR " + pcm.name);
                ArchiveKerbal(pcm);
                Debug.Log("Archived - OKR " + pcm.name);
                ListKerbal(pcm);
            }

            Debug.Log("End of OnKerbalRemoved " + pcm.name);
        }

        void OnKerbalAdded(ProtoCrewMember pcm)
        {
/*            Debug.Log("Added!" + pcm.name.ToString() + " " + pcm.type.ToString());
            if (pcm.type == ProtoCrewMember.KerbalType.Applicant)
            {
                Debug.Log("*****************************Valid Applicant!************************************************");
                int count = CustomerSave.CustomerArchive().Count();
                int countRandom = random.Next(0, count * odds);

                if (countRandom < count)
                {
                    Debug.Log("*****************************Let's Do This!************************************************");
                    string spaceJunkieName = CustomerSave.CustomerArchive().Values.ElementAt(countRandom).name;
                    ProtoCrewMember spaceJunkie = null;
                    foreach (ProtoCrewMember k in HighLogic.CurrentGame.CrewRoster.Unowned)
                    {
                        if ((suffix + k.name) == spaceJunkieName)
                        {
                            spaceJunkie = k;
                        }
                    }

                    if (spaceJunkie != null)
                    {
                        Debug.Log("*****************************" + pcm.name + "************************************************");
                        spaceJunkie.name = spaceJunkie.name.Replace(suffix, "");
                        ReserveCustomer(new CustomerRecord(spaceJunkie), "Applicant");
                        pcm = spaceJunkie;
                        Debug.Log("*****************************" + pcm.name + "************************************************");
                    }
                }
            }
*/
        }

        void ArchiveKerbal(ProtoCrewMember pcm)
        {
            Debug.Log("Archiving - AK " + pcm.name);
            CustomerSave.CustomerArchive()[pcm.name] = new CustomerRecord(pcm);
        }

        public CustomerRecord ReserveCustomer(CustomerRecord customer, string purpose)
        {
            CustomerSave.ReservedCustomers()[customer.Name()] = customer;
            CustomerSave.CustomerArchive().Remove(customer.Name());
            return customer;
        }

        public void reeducateKerbal(ProtoCrewMember to, ProtoCrewMember from)
        {
            to.name = from.name;
            to.courage = from.courage;
            to.stupidity = from.stupidity;
            to.isBadass = from.isBadass;
            to.gender = from.gender;
            to.experienceTrait = from.experienceTrait;
            to.hasToured = from.hasToured;
        }

        public void transferCareerLog(ProtoCrewMember to, ProtoCrewMember from)
        {
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

        public void ListKerbal(ProtoCrewMember pcm)
        {
            Debug.Log(pcm.name + " " + pcm.gender + " " + pcm.courage + " " + pcm.stupidity + " " + pcm.isBadass + " " + pcm.hasToured + " " + pcm.type + " " + pcm.rosterStatus);
            Debug.Log(pcm.flightLog);
        }

        public void UnloadEvents(GameScenes scene)
        {
            GameEvents.onKerbalRemoved.Remove(OnKerbalRemoved);
            GameEvents.onKerbalRemoved.Remove(OnKerbalAdded);
            GameEvents.onGameSceneLoadRequested.Remove(UnloadEvents);
        }
        public string StringListString(List<string> list)
        {
            string newstring = "";
            foreach (string s in list)
            {
                newstring = newstring + s + " ";
            }
            return newstring;
        }

        void OnDestroy()
        {
            Debug.Log("Manager Destroyed!");
            GameEvents.onKerbalRemoved.Remove(OnKerbalRemoved);
            GameEvents.onKerbalRemoved.Remove(OnKerbalAdded);
            GameEvents.onGameSceneLoadRequested.Remove(UnloadEvents);
            Debug.Log("Manager Destroyed!!!");
        }
    }
}
