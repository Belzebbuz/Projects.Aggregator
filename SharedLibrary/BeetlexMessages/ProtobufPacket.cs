using BeetleX;
using BeetleX.Buffers;
using BeetleX.Clients;
using BeetleX.Packets;

namespace SharedLibrary.BeetlexMessages;

public class ProtobufPacket : FixedHeaderPacket
{
    static ProtobufPacket()
    {
        TypeHeader.Register(typeof(ProtobufPacket).Assembly);
    }

    public static CustomTypeHeader TypeHeader { get; private set; } = new CustomTypeHeader(MessageIDType.INT);

    public override IPacket Clone()
    {
        return new ProtobufPacket();
    }

    protected override object OnRead(ISession session, PipeStream stream)
    {
        Type type = TypeHeader.ReadType(stream);
        return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(type, stream, null, null, CurrentSize - 4);
    }
    protected override void OnWrite(ISession session, object data, PipeStream stream)
    {
        TypeHeader.WriteType(data, stream);
        ProtoBuf.Meta.RuntimeTypeModel.Default.Serialize(stream, data);
    }
}

public class ProtobufClientPacket : FixeHeaderClientPacket
{

    static ProtobufClientPacket()
    {
        TypeHeader.Register(typeof(ProtobufClientPacket).Assembly);
    }

    public static CustomTypeHeader TypeHeader { get; private set; } = new CustomTypeHeader(MessageIDType.INT);

    public override IClientPacket Clone()
    {
        return new ProtobufClientPacket();
    }

    protected override object OnRead(IClient client, PipeStream stream)
    {
        Type type = TypeHeader.ReadType(stream);
        return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(type, stream, null, null, CurrentSize - 4);
    }

    protected override void OnWrite(object data, IClient client, PipeStream stream)
    {
        TypeHeader.WriteType(data, stream);
        ProtoBuf.Meta.RuntimeTypeModel.Default.Serialize(stream, data);
    }
}

