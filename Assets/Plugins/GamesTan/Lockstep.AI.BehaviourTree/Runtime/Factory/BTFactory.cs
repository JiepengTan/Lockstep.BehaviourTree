using Lockstep.AI;

namespace AIToolkitDemo {
    public class BTFactory {
        private static int _curIdx = 0;
        public static BTInfo CreateBtInfo(BTNode bt){
            var offsets = bt.GetTotalOffsets();
            var memSize = bt.GetTotalMemSize();
            return new BTInfo() {
                MemSize = memSize,
                Offsets = offsets,
                RootNode = bt,
            };
        }
        public static T CreateNode<T>() where T : BTNode, new(){
            return new T();
        }
    }
}