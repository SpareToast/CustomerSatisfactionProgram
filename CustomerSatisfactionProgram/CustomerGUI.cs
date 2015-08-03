using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomerSatisfactionProgram
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class CustomerGUI : MonoBehaviour
    {
        private Rect _windowPosition = new Rect();
        private GUIStyle _windowStyle, _labelStyle, _nameStyle, _scrollStyle, _buttonStyle;
        private Vector2 _scrollPosition;
        private ApplicationLauncherButton cspLaunchButton;
        private bool _hasInitStyles = false;
        string _castawayToggle;
        string _touristToggle;
        string _applicantToggle;
        string _archivedToggle;
        float _windowWidth = 360f;

        void Awake()
        {
            var texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes("GAMEDATA/CUSTOMERSATISFACTIONPROGRAM/CSP.png"));
            Debug.Log(texture.ToString());
            cspLaunchButton = ApplicationLauncher.Instance.AddModApplication(GuiOn, GuiOff, null, null, null, null,
                ApplicationLauncher.AppScenes.ALWAYS, texture);
        }
        
        public void Start()
        {
            if (!_hasInitStyles)
                InitStyles();
            Debug.Log("Gui? " + _labelStyle.fontSize);
        }
        public void GuiOn()
        {
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
        }

        public void GuiOff()
        {
            RenderingManager.RemoveFromPostDrawQueue(0, OnDraw);
        }

        public void OnDraw()
        {
            _windowPosition = GUILayout.Window(10, _windowPosition, OnWindow, "Customer Satisfaction Program", _windowStyle);
        }


        // this is so ugly
        public void OnWindow(int windowID)
        {
            List<CustomerRecord> castaways = new List<CustomerRecord>();
            List<CustomerRecord> tourists = new List<CustomerRecord>();
            List<CustomerRecord> applicants = new List<CustomerRecord>();
            
            foreach (KeyValuePair<string, CustomerRecord> cr in CustomerSave.ReservedCustomers()) {
                if (cr.Value.status == "CASTAWAY")
                    castaways.Add(cr.Value);
                if (cr.Value.status == "TOURIST")
                    tourists.Add(cr.Value);
                if (cr.Value.status == "APPLICANT")
                    applicants.Add(cr.Value);
            }

            _castawayToggle = DrawPane(castaways, " Rescue Contracts", _castawayToggle);
            _touristToggle = DrawPane(tourists, " Tourist Contracts", _touristToggle);
            _applicantToggle = DrawPane(applicants, " Available Applicants", _applicantToggle);
            _archivedToggle = DrawPane(CustomerSave.ArchivedCustomers().Values.ToList(), " Archived Customers", _archivedToggle);

            GUI.DragWindow();
        }

        // so very ugly
        string DrawPane(List<CustomerRecord> crl, string heading, string toggle)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(heading, _labelStyle, GUILayout.Width(_windowWidth - 80));
            GUILayout.Label(crl.Count().ToString(), _labelStyle, GUILayout.Width(40));
            if (GUILayout.Button(toggle, _buttonStyle))
            {
                _windowPosition.height = 0f;
                if (toggle == "-")
                    toggle = "+";
                else
                    toggle = "-";
            }
            GUILayout.EndHorizontal();

            if ((heading.Count() > 0) && (toggle == "-"))
            {
                foreach (CustomerRecord cr in crl)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", _labelStyle, GUILayout.Width(10));
                    GUILayout.Label(cr.Name() + ", Level " + cr.kerbal.experienceLevel + " " + cr.kerbal.experienceTrait.TypeName, _labelStyle, GUILayout.Width(240));
                    GUILayout.EndHorizontal();
                }
            }

            return toggle;
        }

        internal void OnDestroy()
        {
            if (cspLaunchButton == null)
                return;
            ApplicationLauncher.Instance.RemoveModApplication(cspLaunchButton);
            cspLaunchButton = null;
        }

        private void InitStyles()
        {
            _castawayToggle = "+";
            _touristToggle = "+";
            _applicantToggle = "+";
            _archivedToggle = "+";


            _windowStyle = new GUIStyle(HighLogic.Skin.window);
            _windowStyle.fixedWidth = _windowWidth;

            _labelStyle = new GUIStyle(HighLogic.Skin.label);

            _nameStyle = new GUIStyle(HighLogic.Skin.label);

            _scrollStyle = new GUIStyle(HighLogic.Skin.scrollView);
            _scrollStyle.stretchHeight = true;
            _scrollPosition = Vector2.zero;

            _buttonStyle = new GUIStyle(HighLogic.Skin.button);
            _buttonStyle.fixedWidth = 20;
            _buttonStyle.fixedHeight = 20;

            _hasInitStyles = true;
        }
    }
}