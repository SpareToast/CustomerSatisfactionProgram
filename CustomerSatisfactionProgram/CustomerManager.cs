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
        private static string suffix = "CSP Archive TOU ";
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
/*            if ((pcm.type == ProtoCrewMember.KerbalType.Tourist))
            {
                Debug.Log("Archiving - OKR " + pcm.name);
                ArchiveKerbal(pcm);
                Debug.Log("Archived - OKR " + pcm.name);
                ListKerbal(pcm);
            }
*/
            Debug.Log("End of OnKerbalRemoved " + pcm.name);
 //           Debug.Log("************** Customer Archive " + StringListString(CustomerArchive) + " " + CustomerArchive.Count());
        }

        void OnKerbalAdded(ProtoCrewMember pcm)
        {
            Debug.Log("Added!" + pcm.name.ToString() + " " + pcm.type.ToString());
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
        }

        void ArchiveKerbal(ProtoCrewMember pcm)
        {
            Debug.Log("Archiving - AK " + pcm.name);
            //spawn a crewmember to crib mystery stats off
            ProtoCrewMember amnesiac = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
            Debug.Log("Cloned " + amnesiac.name);

            //teach Kerbal how to be themselves again
            amnesiac.name = suffix + pcm.name;
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

            CustomerRecord customerRecord = new CustomerRecord(amnesiac);
            CustomerSave.CustomerArchive()[customerRecord.name] = customerRecord;
            Debug.Log("Listed " + amnesiac.name);
            
            //            Debug.Log("************** Customer Archive " + StringListString(CustomerArchive) + " " + CustomerArchive.Count());
        }

        public CustomerRecord ReserveCustomer(CustomerRecord customer, string purpose)
        {
//            if ((location >= 0) && (location < CustomerSave.CustomerArchive().Count()))
//            {
                CustomerSave.ReservedCustomerArchive()[customer.name] = customer;
                CustomerSave.CustomerArchive().Remove(customer.name);
                return customer;
//            }
//            else return null;
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
