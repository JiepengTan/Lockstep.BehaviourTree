
// auto generate by tools, DO NOT Modify it!!!
using Lockstep.AI;
namespace AIToolkitDemo
{
    public enum EGameBTNodeType
    {
		CON_HasReachedTarget = 6,
		NOD_Attack = 7,
		NOD_MoveTo = 8,
		NOD_TurnTo = 9,

    }
    
    public partial class BTNodeFactoryInjector
    {
        partial void _Inject()
        {
			BTNodeFactory.Register((int)(EGameBTNodeType.CON_HasReachedTarget),()=>new AIToolkitDemo.CON_HasReachedTarget());
			BTNodeFactory.Register((int)(EGameBTNodeType.NOD_Attack),()=>new AIToolkitDemo.NOD_Attack());
			BTNodeFactory.Register((int)(EGameBTNodeType.NOD_MoveTo),()=>new AIToolkitDemo.NOD_MoveTo());
			BTNodeFactory.Register((int)(EGameBTNodeType.NOD_TurnTo),()=>new AIToolkitDemo.NOD_TurnTo());

        }
    }

partial class CON_HasReachedTarget {public override int TypeId=>(int)EGameBTNodeType.CON_HasReachedTarget;}
partial class NOD_Attack {public override int TypeId=>(int)EGameBTNodeType.NOD_Attack;}
partial class NOD_MoveTo {public override int TypeId=>(int)EGameBTNodeType.NOD_MoveTo;}
partial class NOD_TurnTo {public override int TypeId=>(int)EGameBTNodeType.NOD_TurnTo;}

}