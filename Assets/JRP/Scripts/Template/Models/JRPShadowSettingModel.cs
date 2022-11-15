using System;

namespace JackRenderPipeline {

    [Serializable]
    public class JRPShadowSettingModel {

        public int maxShadowCount;
        public float maxDistance;

        public JRPTextureSizeEnum atlasSize;

    }
}