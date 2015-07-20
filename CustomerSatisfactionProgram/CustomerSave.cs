using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace CustomerLoyaltyProgram
{
    public class CustomerSave : ScenarioModule
    {
        public ConfigNode ModNode { get; private set; }

        private static Dictionary<String, CustomerRecord> _customerArchive;
        private static Dictionary<String, CustomerRecord> _reservedCustomerArchive;


        public static Dictionary<String, CustomerRecord> CustomerArchive()
        {
            if (_customerArchive == null) _customerArchive = new Dictionary<string, CustomerRecord>();
            return _customerArchive;
        }

        public static Dictionary<String, CustomerRecord> ReservedCustomerArchive()
        {
            if (_reservedCustomerArchive == null) _reservedCustomerArchive = new Dictionary<string, CustomerRecord>();
            return _reservedCustomerArchive;
        }

        public static CustomerSave Instance { get; private set; }

        public CustomerSave()
        {
            Instance = this;
        }

        public override void OnLoad(ConfigNode gameNode)
        {
            base.OnLoad(gameNode);
            if (gameNode.HasNode("CUSTOMER_LOYALTY_PROGRAM"))
            {
                ModNode = gameNode.GetNode("CUSTOMER_LOYALTY_PROGRAM");
                ConfigNode[] archivedNodes = ModNode.GetNodes("ARCHIVEDCUSTOMERS");
                foreach (ConfigNode customerNode in archivedNodes)
                {
                    CustomerRecord c = ResourceUtilities.LoadNodeProperties<CustomerRecord>(customerNode);
                    CustomerSave.CustomerArchive()[c.name] = c;
                }
                ConfigNode[] reservedNodes = ModNode.GetNodes("RESERVEDCUSTOMERS");
                foreach (ConfigNode customerNode in archivedNodes)
                {
                    CustomerRecord c = ResourceUtilities.LoadNodeProperties<CustomerRecord>(customerNode);
                    CustomerSave.ReservedCustomerArchive()[c.name] = c;
                }

            }
            else
            {
                        _customerArchive = new Dictionary<string, CustomerRecord>();
                        _reservedCustomerArchive = new Dictionary<String, CustomerRecord>();

            }
        }

        public override void OnSave(ConfigNode gameNode)
        {
            Debug.Log("Saving??? " + CustomerSave.CustomerArchive());
            base.OnSave(gameNode);
            Debug.Log("Saving??? " + CustomerSave.CustomerArchive());
            if (gameNode.HasNode("CUSTOMER_SATISFACTION_PROGRAM"))
            {
                Debug.Log("CUSTOMER STORE FOUND (SAVE)");
                ModNode = gameNode.GetNode("CUSTOMER_SATISFACTION_PROGRAM");
            }
            else
            {
                Debug.Log("CUSTOMER STORE NOT FOUND (SAVE)");
                ModNode = gameNode.AddNode("CUSTOMER_SATISFACTION_PROGRAM");
            }

            foreach (KeyValuePair<string, CustomerRecord> c in _customerArchive)
            {
                ConfigNode customerNode = new ConfigNode("ARCHIVEDCUSTOMER");
//                ProtoCrewMember amnesiac = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                customerNode.AddValue("name", c.Value.name);
                customerNode.AddValue("origin", c.Value.origin);

/*                customerNode.AddValue("gender", t.Value.gender);
                customerNode.AddValue("courage", t.Value.courage);
                customerNode.AddValue("stupidity", t.Value.stupidity);
                customerNode.AddValue("isBadass", t.Value.isBadass);
                customerNode.AddValue("type", t.Value.type);
                customerNode.AddValue("rosterStatus", t.Value.rosterStatus);

                ConfigNode careerNode = new ConfigNode("CAREER_LOG");
                t.Value.careerLog.Save(careerNode);
                customerNode.AddNode(careerNode);
                ConfigNode flightNode = new ConfigNode("FLIGHT_LOG");
                t.Value.flightLog.Save(flightNode);
                customerNode.AddNode(flightNode);
*/                ModNode.AddNode(customerNode);
            }
            foreach (KeyValuePair<string, CustomerRecord> c in _reservedCustomerArchive)
            {
                ConfigNode customerNode = new ConfigNode("RESERVEDCUSTOMER");
                customerNode.AddValue("name", c.Value.name);
                customerNode.AddValue("origin", c.Value.origin);
                ModNode.AddNode(customerNode);
            }
        }
    }
}