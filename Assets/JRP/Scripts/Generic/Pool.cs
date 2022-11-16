using System;
using System.Collections.Generic;

namespace JackRenderPipeline {

    public class Pool<T> {

        Stack<T> stack;

        Func<T> generateFunc;

        public Pool(Func<T> generateFunc, int baseCount = 4) {
            this.generateFunc = generateFunc;
            this.stack = new Stack<T>();
            for (int i = 0; i < baseCount; i += 1) {
                this.stack.Push(generateFunc());
            }
        }

        public T Take() {
            if (this.stack.Count == 0) {
                this.stack.Push(this.generateFunc());
            }
            return this.stack.Pop();
        }

        public void Release(T obj) {
            this.stack.Push(obj);
        }

    }

}