using BeetleX;
using BeetleX.Packets;
using ProtoBuf;

namespace SharedLibrary.BeetlexMessages;

[MessageType(1)]
[ProtoContract]
public class FileContentBlock : IMessageSubmitHandler
{
    [ProtoMember(1)]
    public string FileName { get; set; }
    [ProtoMember(2)]
    public bool Eof { get; set; }
    [ProtoMember(3)]
    public int Index { get; set; }
    [ProtoMember(4)]
    public byte[] Data { get; set; }
    [ProtoMember(5)]
    public Guid ProjectId { get; set; }
	[ProtoMember(6)]
	public Guid ReleaseId { get; internal set; }
	public Action<FileContentBlock> Completed { get; set; }
    public string SessionId { get; internal set; }

    public void Execute(object sender, object message)
    {
        Completed?.Invoke(this);
    }
}
