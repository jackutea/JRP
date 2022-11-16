using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class JRPFacades {

        JRPSettingModel settingModel;
        public JRPSettingModel SettingModel => settingModel;

        AllRepo repo;
        public AllRepo Repo => repo;

        CommandBuffer shadowBuffer;
        public CommandBuffer ShadowBuffer => shadowBuffer;

        public JRPFacades() { }

        public void Inject(JRPSettingModel settingModel) {

            this.settingModel = settingModel;

            this.repo = new AllRepo();

        }

    }

}