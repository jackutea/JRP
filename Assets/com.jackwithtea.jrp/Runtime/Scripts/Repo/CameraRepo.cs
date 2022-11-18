using System;
using System.Collections.Generic;

namespace JackRenderPipeline {

    public class CameraRepo {

        List<CameraRenderer> all;

        public CameraRepo() {
            this.all = new List<CameraRenderer>();
        }

        public void Add(CameraRenderer cam) {
            all.Add(cam);
        }

        public void Remove(CameraRenderer cam) {
            all.Remove(cam);
        }

        internal void ForEach(Action<CameraRenderer> value) {
            all.ForEach(value);
        }

        internal void Clear() {
            all.Clear();
        }
    }

}