using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomerSatisfactionProgram
{
    public class CustomerGUI : MonoBehaviour
    {
        private Rect _windowPosition = new Rect();
        public void OnDraw()
        {
            _windowPosition = GUILayout.Window(10, _windowPosition, OnWindow, "Customer Satisfaction Program");
        }

        public void OnWindow(int windowID)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(320f));

            GUILayout.Label("Available for Hire");
            GUILayout.EndHorizontal();

            foreach (KeyValuePair<string, CustomerRecord> rc in CustomerSave.ReservedCustomers())
            {

                if (rc.Value.status == "APPLICANT")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(rc.Value.Name());
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label("Repeat Customers");
            GUILayout.EndHorizontal();

            foreach (KeyValuePair<string, CustomerRecord> rc in CustomerSave.ReservedCustomers())
            {
                if (rc.Value.status == "CONTRACT")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(rc.Value.Name());
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Fans");
            GUILayout.EndHorizontal();

            foreach (KeyValuePair<string, CustomerRecord> rc in CustomerSave.ArchivedCustomers())
            {
                if (rc.Value.status == "ARCHIVED")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(rc.Value.Name());
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.Label(CustomerSave.ArchivedCustomers().Count() + " " + CustomerSave.ReservedCustomers().Count());

            GUI.DragWindow();
        }
    }
}