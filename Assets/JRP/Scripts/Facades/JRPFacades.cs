using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class JRPFacades {

        JRPSettingModel settingModel;
        public JRPSettingModel SettingModel => settingModel;

        CommandBuffer cameraBuffer;
        public CommandBuffer CameraBuffer => cameraBuffer;

        CommandBuffer lightBuffer;
        public CommandBuffer LightBuffer => lightBuffer;

        CommandBuffer shadowBuffer;
        public CommandBuffer ShadowBuffer => shadowBuffer;

        public JRPFacades() { }

        public void Inject(JRPSettingModel settingModel, CommandBuffer cameraCB, CommandBuffer lightCB, CommandBuffer shadowCB) {

            this.settingModel = settingModel;

            this.cameraBuffer = cameraCB;
            this.lightBuffer = lightCB;
            this.shadowBuffer = shadowCB;

        }

    }

}