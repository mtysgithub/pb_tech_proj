//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: protodef/file.proto
namespace dataconfig
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PbVector3")]
  public partial class PbVector3 : global::ProtoBuf.IExtensible
  {
    public PbVector3() {}
    
    private float _x;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"x", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float x
    {
      get { return _x; }
      set { _x = value; }
    }
    private float _y;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"y", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float y
    {
      get { return _y; }
      set { _y = value; }
    }
    private float _z;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"z", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float z
    {
      get { return _z; }
      set { _z = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PbFood")]
  public partial class PbFood : global::ProtoBuf.IExtensible
  {
    public PbFood() {}
    
    private int _score;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"score", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int score
    {
      get { return _score; }
      set { _score = value; }
    }
    private PbVector3 _position;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"position", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public PbVector3 position
    {
      get { return _position; }
      set { _position = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PbPlayer")]
  public partial class PbPlayer : global::ProtoBuf.IExtensible
  {
    public PbPlayer() {}
    
    private int _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private readonly global::System.Collections.Generic.List<PbVector3> _bodys = new global::System.Collections.Generic.List<PbVector3>();
    [global::ProtoBuf.ProtoMember(2, Name=@"bodys", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<PbVector3> bodys
    {
      get { return _bodys; }
    }
  
    private int _score;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"score", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int score
    {
      get { return _score; }
      set { _score = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PbGameFile")]
  public partial class PbGameFile : global::ProtoBuf.IExtensible
  {
    public PbGameFile() {}
    
    private PbPlayer _player;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"player", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public PbPlayer player
    {
      get { return _player; }
      set { _player = value; }
    }
    private readonly global::System.Collections.Generic.List<PbFood> _foods = new global::System.Collections.Generic.List<PbFood>();
    [global::ProtoBuf.ProtoMember(2, Name=@"foods", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<PbFood> foods
    {
      get { return _foods; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}