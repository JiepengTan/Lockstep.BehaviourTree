using Lockstep.AI;

namespace AIToolkitDemo
{
    public partial class BTNodeFactoryInjector
    {
        private BTNodeFactoryInjector() { }

        public static void Inject()
        {
           new BTNodeFactoryInjector()._Inject();
        }

        partial void _Inject();
    }

}