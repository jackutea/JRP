using System.Collections.Generic;

namespace JackRenderPipeline {

    public class AllRepo {

        CameraRepo cameraRepo;
        public CameraRepo CameraRepo => cameraRepo;

        LightRepo lightRepo;
        public LightRepo LightRepo => lightRepo;

        public AllRepo() {
            this.cameraRepo = new CameraRepo();
            this.lightRepo = new LightRepo();
        }

    }
}