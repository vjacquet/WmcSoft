# Cloning
Framework Design Guidelines advise against implementing `ICloneable` or even using it in public APIs mostly because this interface
does not specify whether the copy is deep or shallow. Instead, they suggest to define a `Clone` method on types that need a cloning 
mecanism and to ensure the documentation clearly states whether it is a deep or shallow copy.

As a **user**, I am only concerned by the fact that changing properties of copy are not reflected on the original, unless they are meant to.

For instance, let say we have a `Person` with `Name` and `Addresses`. Changing the name of its _clone_ should not change its name, yet the 
_clone_ can move to another address. On the other hand, if we fix a typo in the address, the typo should be fixed for the person and its _clone_.

As an **implementer**, it is my responsability to decide whether the copy is deep or shallow.

As an **inheritor**, I should be guided on what type of copy I should implement.


Not only the suggestion does not help with any of these but it prevent us to rely on the `ICloneable` contract for some generic operations. It does,
however, fix an issue they did not mention: `Clone` returns an object, so the return value should be cast before being used.

At first, I thought of defining a new interface
```csharp
public interface ICloneable<out T> : ICloneable
{
    new T Clone();
}
```

but it does not cope well with inheritance, as it can only return the type on which the interface is implement. Unlike C++, the return type of 
virtual function in C# is not covariant.


So, instead, I'd rather call an extension method to the rescue
```csharp
public static T Clone<T>(this T obj)
    where T : ICloneable
{
    return (T)obj.Clone();
}
```

This extension works best when `ICloneable` is explicitly implemented. If not, you have to specify `Clone<T>()` to disambiguate
the method call.

Not being able to make the method virtual is not really a problem as we can kill two birds with on stone by delegating the implementation
to a virtual method having an extra parameter that will be indicating to **inheritors** whether the copy should be deep or shallow.
```csharp
    protected virtual object Clone(Cloning.Deep strategy)
    {
        return MemberwiseClone();
    }

    object ICloneable.Clone()
    {
        return Clone(Cloning.SuggestDeep);
    }
```

The strategy can implemeted with type markers, so the intent is clearly state in the signature of the method.

```csharp
public static class Cloning
{
    #region Markers

    /// <summary>
    /// Marker to indicate the cloning should be deep.
    /// </summary>
    public struct Deep { }

    /// <summary>
    /// Suggest the cloning should be deep.
    /// </summary>
    public static readonly Deep SuggestDeep;

    /// <summary>
    /// Marker to indicate the cloning should be shallow.
    /// </summary>
    public struct Shallow { }

    /// <summary>
    /// Suggest the cloning should be shallow.
    /// </summary>
    public static readonly Shallow SuggestShallow;

    #endregion

    ...
}
```
