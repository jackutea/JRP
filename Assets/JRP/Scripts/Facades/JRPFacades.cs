using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class JRPFacades {

        ScriptableRenderContext renderCtx;
        public ScriptableRenderContext RenderContext => renderCtx;
        public void SetRenderContext(ScriptableRenderContext value) => renderCtx = value;

        JRPSettingModel settingModel;
        public JRPSettingModel SettingModel => settingModel;

        AllRepo repo;
        public AllRepo Repo => repo;

        CommandBuffer cameraBuffer;
        public CommandBuffer CameraBuffer => cameraBuffer;

        CommandBuffer lightBuffer;
        public CommandBuffer LightBuffer => lightBuffer;

        CommandBuffer shadowBuffer;
        public CommandBuffer ShadowBuffer => shadowBuffer;

        public JRPFacades() { }

        public void Inject(JRPSettingModel settingModel, CommandBuffer cameraCB, CommandBuffer lightCB, CommandBuffer shadowCB) {

            this.settingModel = settingModel;

            this.repo = new AllRepo();

            this.cameraBuffer = cameraCB;
            this.lightBuffer = lightCB;
            this.shadowBuffer = shadowCB;

        }

    }

}