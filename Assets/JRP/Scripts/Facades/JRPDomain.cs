namespace JackRenderPipeline {

    public class JRPDomain {

        CameraDomain cameraDomain;
        public CameraDomain CameraDomain => cameraDomain;

        LightDomain lightDomain;
        public LightDomain LightDomain => lightDomain;

        public JRPDomain() {
            cameraDomain = new CameraDomain();
            lightDomain = new LightDomain();
        }

    }

}