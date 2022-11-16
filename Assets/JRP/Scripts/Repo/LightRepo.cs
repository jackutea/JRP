using System;
using System.Collections.Generic;

namespace JackRenderPipeline {

    public class LightRepo {

        List<LightRenderer> all;

        LightRenderer sunLight;
        public LightRenderer SunLight => sunLight;
        public void SetSunLight(LightRenderer value) => sunLight = value;

        public LightRepo() {
            this.all = new List<LightRenderer>();
        }

        public void Add(LightRenderer light) {
            all.Add(light);
        }

        public void ForEach(Action<LightRenderer> action) {
            all.ForEach(action);
        }

        public void Remove(LightRenderer light) {
            all.Remove(light);
        }

        public void Clear() {
            all.Clear();
        }

    }

}