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
        private static Dictionary<String, CustomerRecord> _reservedCustomers;

        public static Dictionary<String, CustomerRecord> CustomerArchive()
        {
            if (_customerArchive == null) _customerArchive = new Dictionary<string, CustomerRecord>();
            return _customerArchive;
        }

        public static Dictionary<String, CustomerRecord> ReservedCustomers()
        {
            if (_reservedCustomers == null) _reservedCustomers = new Dictionary<string, CustomerRecord>();
            return _reservedCustomers;
        }

        public static CustomerSave Instance { get; private set; }

        public CustomerSave()
        {
            Instance = this;
        }

        public override void OnLoad(ConfigNode gameNode)
        {
            if (gameNode.HasNode("CUSTOMER_SATISFACTION_PROGRAM"))
            {
                Debug.Log("CUSTOMER STORE FOUND (LOAD)");
                ModNode = gameNode.GetNode("CUSTOMER_SATISFACTION_PROGRAM");
                ConfigNode[] archivedNodes = ModNode.GetNodes("ARCHIVED_CUSTOMER");

                foreach (ConfigNode customerNode in archivedNodes)
                {
                    CustomerRecord customerRecord = ResourceUtilities.LoadNodeProperties<CustomerRecord>(customerNode);

                    ConfigNode kerbalNode = customerNode.GetNode("KERBAL");
                    customerRecord.kerbal  = new ProtoCrewMember(Game.Modes.CAREER, kerbalNode);

                    CustomerSave.CustomerArchive()[customerRecord.kerbal.name] = customerRecord;
                }
                /*
                                ConfigNode[] reservedNodes = ModNode.GetNodes("RESERVED_CUSTOMERS");
                                foreach (ConfigNode reservedNode in archivedNodes)
                                {
                                    string c = ResourceUtilities.LoadNodeProperties<string>(reservedNode);
                                    CustomerSave.ReservedCustomers()[c.name] = c;
                                }

                                ConfigNode[] archivedNodes = ModNode.GetNodes("ARCHIVED_CUSTOMER");
                                foreach (ConfigNode customerNode in archivedNodes)
                                {
                                    ProtoCrewMember p = new ProtoCrewMember(Game.Modes.CAREER, customerNode);
                                    CustomerSave.CustomerArchive()[p.name] = new CustomerRecord(p);
                                }
                */
            }
            else
            {
                Debug.Log("CUSTOMER STORE NOT FOUND (LOAD)");
                _customerArchive = new Dictionary<string, CustomerRecord>();
                _reservedCustomers = new Dictionary<String, CustomerRecord>();
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

            foreach (KeyValuePair<string, CustomerRecord> p in CustomerArchive())
            {

                ConfigNode customerNode = new ConfigNode("ARCHIVED_CUSTOMER");

                ConfigNode kerbalNode = new ConfigNode("KERBAL");
                p.Value.kerbal.Save(kerbalNode);
                customerNode.AddNode(kerbalNode);

                customerNode.AddValue("origin", p.Value.origin);

                ModNode.AddNode(customerNode);
            }

        }
    }
}