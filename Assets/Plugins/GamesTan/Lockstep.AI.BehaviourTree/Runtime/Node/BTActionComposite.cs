namespace Lockstep.AI
{
    public abstract unsafe partial class BTActionComposite : BTAction
    {       
        public BTActionComposite(int maxChildCount)
            : base(maxChildCount){
        }
    }
}