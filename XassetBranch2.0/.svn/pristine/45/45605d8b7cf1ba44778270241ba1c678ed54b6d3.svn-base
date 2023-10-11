using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
    public class TargetsHolder
    {
        private HashSet<KeyValuePair<RenderTargetIdentifier, int>> targetsInUse = new HashSet<KeyValuePair<RenderTargetIdentifier, int>>();
        
        public RenderTargetIdentifier GetTarget(int id, OutlineParameters parameters)
        {
            var identifier = new RenderTargetIdentifier(id, 0, CubemapFace.Unknown, -1);
            if (targetsInUse.Add(new KeyValuePair<RenderTargetIdentifier, int>(identifier, id)))
                RenderTargetUtility.GetTemporaryRT(parameters, id, parameters.Width, parameters.Height, 0);

            return identifier;
        }

        public void ReleaseAll(OutlineParameters parameters)
        {
            foreach (var rt in targetsInUse)
                parameters.Buffer.ReleaseTemporaryRT(rt.Value);

            targetsInUse.Clear();
        }
    }
}