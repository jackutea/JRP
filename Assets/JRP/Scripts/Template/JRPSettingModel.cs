using System;

namespace JackRenderPipeline {

    [Serializable]
    public class JRPSettingModel {

        public JRPBatchingSettingModel batchingSetting;
        public JRPBufferSettingModel bufferSetting;
        public JRPShaderSettingModel shaderSetting;
        public JRPLightSettingModel lightSetting;
        public JRPMaterialSettingModel materialSetting;
        public JRPShadowSettingModel shadowSetting;

        public JRPSettingModel() {}

        public void Init() {
            lightSetting.Init();
        }

    }

}