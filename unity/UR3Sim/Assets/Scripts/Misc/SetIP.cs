using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class SetIP : MonoBehaviour
    {
        private void Awake() {
            RosConnector rosConnector = GetComponent<RosConnector>();
            string ip = PlayerPrefs.GetString("ROSIP", "192.168.2.2");
            string fullIP = "ws://" + ip + ":9090";
            rosConnector.RosBridgeServerUrl = fullIP;
        }
    }
}