using Lockstep.AI;

namespace AIToolkitDemo
{


    public partial class BTNodeFactoryInjector
    {
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoad]
        class AutoInject {
            static AutoInject() {
                BTNodeFactoryInjector.Inject();
                UnityEngine.Debug.Log("AutoRegister");
            }
        }
#endif
        
        private BTNodeFactoryInjector() { }
        public static void Inject()
        {
           new BTNodeFactoryInjector()._Inject();
        }

        partial void _Inject();
    }

}