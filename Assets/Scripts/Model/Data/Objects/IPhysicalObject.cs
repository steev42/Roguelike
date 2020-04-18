
public interface IPhysicalObject : IObject
{
    bool isLockedTo(IPhysicalObject o);
    float GetAttribute(string s);
    int GetAttributeInteger(string s);
}
