using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
namespace XAsset
{
    public class AgentSelect : MonoBehaviour
    {
        public string agenttype;
        public Outlinable outlinable;
        public void OnMouseOver()
        {
            outlinable.enabled = true;
        }
        public void OnMouseExit()
        {
            //outlinable.enabled = false;
            outlinable.enabled = true;
            if (Global.AgentType != agenttype)
            {
                outlinable.enabled = false;
            }
        }
        public void OnMouseDown()
        {
            outlinable.enabled = !outlinable.enabled;
            Global.AgentType = agenttype;
        }
    }
}
