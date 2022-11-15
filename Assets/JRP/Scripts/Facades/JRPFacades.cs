using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class JRPFacades {

        CommandBuffer cameraBuffer;
        public CommandBuffer CameraBuffer => cameraBuffer;

        CommandBuffer lightBuffer;
        public CommandBuffer LightBuffer => lightBuffer;

        public JRPFacades() {
            
            cameraBuffer = new CommandBuffer();
            cameraBuffer.name = JRPConfig.BUFFER_CAMERA;

            lightBuffer = new CommandBuffer();
            lightBuffer.name = JRPConfig.BUFFER_LIGHT;

        }

    }

}