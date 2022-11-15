using System;
using UnityEngine;

namespace JackRenderPipeline {

    [Serializable]
    public class JRPLightSettingModel {

        [Header("Shader PropertyID")]
        [SerializeField] string propLightColorName;
        public int PropLightColorID { get; private set; }

        [SerializeField] string propLightDirectionName;
        public int PropLightDirectionID { get; private set; }

        [SerializeField] string propLightIntensityName;
        public int PropLightIntensityID { get; private set; }

        public void Init() {
            this.PropLightColorID = Shader.PropertyToID(propLightColorName);
            this.PropLightDirectionID = Shader.PropertyToID(propLightDirectionName);
            this.PropLightIntensityID = Shader.PropertyToID(propLightIntensityName);
        }

    }
}