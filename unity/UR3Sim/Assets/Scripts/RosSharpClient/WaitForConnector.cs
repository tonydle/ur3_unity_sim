using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class WaitForConnector : MonoBehaviour
    {
        private RosConnector rosConnector;
        private MonoBehaviour[] comps;
        private bool topicsEnabled = false;
        void Start()
        {
            rosConnector = GetComponent<RosConnector>();
            comps = GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour c in comps)
            {
                c.enabled = false;
            }
            GetComponent<RosConnector>().enabled = true;
            GetComponent<WaitForConnector>().enabled = true;
        }

        private void FixedUpdate() {
            if (rosConnector == null) {
                rosConnector = GetComponent<RosConnector>();
            }
            else {
                if (rosConnector.IsConnected.WaitOne(0) && !topicsEnabled) {
                    foreach(MonoBehaviour c in comps)
                    {
                        c.enabled = true;
                    }
                    topicsEnabled = true;
                }
            }
        }
    }
}
