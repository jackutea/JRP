using System;
using UnityEngine;

namespace JackRenderPipeline {

    [Serializable]
    public class JRPShadowSettingModel {

        [SerializeField] string propDirectShadowAtlasName;
        public int PropDirectShadowAtlasID { get; private set; }

        public int maxShadowLightCount;
        public float maxDistance;

        public JRPTextureSizeEnum atlasSize;

        public void Init() {
            this.PropDirectShadowAtlasID = Shader.PropertyToID(propDirectShadowAtlasName);
        }

    }
}