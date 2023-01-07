
// auto generate by tools, DO NOT Modify it!!!
using Lockstep.AI;
using Lockstep.Serialization;

namespace AIToolkitDemo
{
    public partial class CON_HasReachedTarget
    {
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
			writer.Write(Distance);

        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
			Distance = reader.ReadSingle();

        }
    }
}


namespace AIToolkitDemo
{
    public partial class NOD_MoveTo
    {
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
			writer.Write(MoveSpeed);
			writer.Write(StopDistance);

        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
			MoveSpeed = reader.ReadSingle();
			StopDistance = reader.ReadSingle();

        }
    }
}


namespace AIToolkitDemo
{
    public partial class NOD_TurnTo
    {
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
			writer.Write(TurnSpeed);

        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
			TurnSpeed = reader.ReadSingle();

        }
    }
}

