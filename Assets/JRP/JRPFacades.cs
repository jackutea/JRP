using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class JRPFacades {

        CommandBuffer cameraBuffer;
        public CommandBuffer CameraBuffer => cameraBuffer;

        public JRPFacades() {
            cameraBuffer = new CommandBuffer() {
                name = JRPConfig.BUFFER_CAMERA
            };
        }

    }

}