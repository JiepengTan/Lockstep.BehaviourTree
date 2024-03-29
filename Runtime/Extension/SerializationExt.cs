﻿using Lockstep.Serialization;

namespace Lockstep.AI
{
    public unsafe partial class BTNode : Lockstep.Serialization.ISerializable
    {
        public virtual void Serialize(Serializer writer)
        {
            writer.Write(_indexInTree);
        }

        public virtual void Deserialize(Deserializer reader)
        {
            _indexInTree = reader.ReadUInt16();
        }
    }
    
    public unsafe partial class BTActionRoot
    {
        public override ushort TypeId=>(int)EBuiltinBTNodeType.BTActionRoot;
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
            writer.Write(IsPriority);
        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
            IsPriority = reader.ReadBoolean();
        }
    }
    public unsafe partial class BTActionLoop
    {
        public override ushort TypeId=>(int)EBuiltinBTNodeType.BTActionLoop;
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
            writer.Write(_loopCount);
        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
            _loopCount = reader.ReadInt32();
        }
    }
    public unsafe partial class BTActionParallel
    {
        public override ushort TypeId=>(int)EBuiltinBTNodeType.BTActionParallel;
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
            writer.Write((int)_evaluationRelationship);
            writer.Write((int)_runningStatusRelationship);
        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
            _evaluationRelationship = (ECHILDREN_RELATIONSHIP)reader.ReadInt32();
            _runningStatusRelationship = (ECHILDREN_RELATIONSHIP)reader.ReadInt32();
        }
    }

    public unsafe partial class BTActionSelector
    {
        public override ushort TypeId=>(int)EBuiltinBTNodeType.BTActionSelector;
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
            writer.Write(IsPriority);
        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
            IsPriority = reader.ReadBoolean();
        }
    }
    public unsafe partial class BTActionSequence
    {
        public override ushort TypeId=>(int)EBuiltinBTNodeType.BTActionSequence;
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
            writer.Write(_continueIfErrorOccors);
        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
            _continueIfErrorOccors = reader.ReadBoolean();
        }
    }

    public unsafe partial class BTCondition
    {
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
            writer.Write(IsInvert);
        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
            IsInvert = reader.ReadBoolean();
        }
    }

    public partial class BTConditionComposite 
    {
        public override ushort TypeId=>(int)EBuiltinBTNodeType.BTConditionComposite;
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
            writer.Write(IsAllTrue);
        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
            IsAllTrue = reader.ReadBoolean();
        }
    }
}