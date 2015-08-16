using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//TODO: Too many static variables

namespace CustomerSatisfactionProgram
{
    public class CustomerSave : ScenarioModule
    {

        public ConfigNode ModNode { get; private set; }

        private static Dictionary<String, CustomerRecord> _archivedCustomers;
        private static Dictionary<String, CustomerRecord> _reservedCustomers;

        public static Dictionary<String, CustomerRecord> ArchivedCustomers() {
            if (_archivedCustomers == null) _archivedCustomers = new Dictionary<string, CustomerRecord>();
            return _archivedCustomers;
        }

        public static Dictionary<String, CustomerRecord> ReservedCustomers() {
            if (_reservedCustomers == null) _reservedCustomers = new Dictionary<string, CustomerRecord>();
            return _reservedCustomers;
        }

        public static CustomerSave Instance { get; private set; }

        public CustomerSave() {
            Instance = this;
        }

        public override void OnLoad(ConfigNode gameNode) {
            if (gameNode.HasNode("CUSTOMER_SATISFACTION_PROGRAM")) {
                Debug.Log("CSP: SAVE FOUND");
                ModNode = gameNode.GetNode("CUSTOMER_SATISFACTION_PROGRAM");

                // restore archived customers
                _archivedCustomers = new Dictionary<String, CustomerRecord>();
                ConfigNode[] archivedNodes = ModNode.GetNodes("ARCHIVED_CUSTOMER");
                foreach (ConfigNode customerNode in archivedNodes) {
                    CustomerRecord customerRecord = ResourceUtilities.LoadNodeProperties<CustomerRecord>(customerNode);

                    ConfigNode kerbalNode = customerNode.GetNode("KERBAL");
                    customerRecord.kerbal  = new ProtoCrewMember(Game.Modes.CAREER, kerbalNode);

                    CustomerSave.ArchivedCustomers()[customerRecord.kerbal.name] = customerRecord;
                }

                // restore reserved customers
                _reservedCustomers = new Dictionary<String, CustomerRecord>();
                ConfigNode[] reservedNodes = ModNode.GetNodes("RESERVED_CUSTOMER");
                foreach (ConfigNode customerNode in reservedNodes) {
                    CustomerRecord customerRecord = ResourceUtilities.LoadNodeProperties<CustomerRecord>(customerNode);

                    ConfigNode kerbalNode = customerNode.GetNode("KERBAL");
                    customerRecord.kerbal = new ProtoCrewMember(Game.Modes.CAREER, kerbalNode);

                    CustomerSave.ReservedCustomers()[customerRecord.kerbal.name] = customerRecord;
                }
            }

            else {
                Debug.Log("CSP: SAVE NOT FOUND");
                _archivedCustomers = new Dictionary<string, CustomerRecord>();
                _reservedCustomers = new Dictionary<string, CustomerRecord>();
            }
        }

        public override void OnSave(ConfigNode gameNode) {
            Debug.Log("Saving??? " + CustomerSave.ArchivedCustomers());
            base.OnSave(gameNode);
            Debug.Log("Saving??? " + CustomerSave.ArchivedCustomers());
            if (gameNode.HasNode("CUSTOMER_SATISFACTION_PROGRAM")) {
                ModNode = gameNode.GetNode("CUSTOMER_SATISFACTION_PROGRAM");
            }
            else {
                Debug.Log("CUSTOMER STORE NOT FOUND (SAVE)");
                ModNode = gameNode.AddNode("CUSTOMER_SATISFACTION_PROGRAM");
            }

            // save archived customers
            foreach (KeyValuePair<string, CustomerRecord> p in ArchivedCustomers()) {

                ConfigNode customerNode = new ConfigNode("ARCHIVED_CUSTOMER");

                ConfigNode kerbalNode = new ConfigNode("KERBAL");
                p.Value.kerbal.Save(kerbalNode);
                customerNode.AddNode(kerbalNode);

                customerNode.AddValue("origin", p.Value.origin);
                customerNode.AddValue("status", p.Value.status);

                ModNode.AddNode(customerNode);
            }

            // save reserved customers
            foreach (KeyValuePair<string, CustomerRecord> p in ReservedCustomers())
            {

                ConfigNode customerNode = new ConfigNode("RESERVED_CUSTOMER");

                ConfigNode kerbalNode = new ConfigNode("KERBAL");
                p.Value.kerbal.Save(kerbalNode);
                customerNode.AddNode(kerbalNode);

                customerNode.AddValue("origin", p.Value.origin);
                customerNode.AddValue("status", p.Value.status);

                ModNode.AddNode(customerNode);
            }
        }
    }
}