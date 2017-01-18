# Canvas

Let's face it, moving existing code to .Net core can be a burden because it is only _almost_ .Net. 

The classes are spreaded in multiple assemblies.
 Some of them still have their legacy setbacks, such as `IList<T>` still not specializing `IReadOnlyList<T>`, which is another missed opportunity. 
And, finally, some feature you were relying on might have simply [disaperead](https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/).

For instance, .Net core do not have classes to handle graphics.
So, what choice do we have? 

`System.Drawing.Graphics` is tied to GDI+. Making it abstract is somewhat possible but it is still a lot of work to implement as there is a lot of methods.

The HTML 2dcontext canvas does not have that many methods, is [specified](https://html.spec.whatwg.org/multipage/scripting.html#2dcontext), [documented, with samples and interactive console](https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D).
Therefore, implementing test cases is pretty straighforward.

The abstractions will be tested against an implementation based on `System.Drawing.Graphics`. Once completed, I'll try an implementation based on [ImageSharp](https://github.com/JimBobSquarePants/ImageSharp).

## Design notes

- Canvas silently ignores invalid input. These abstractions should not.

- Should I define a Matrix class or use the standard `System.Numerics.Matrix4x4`? The first one cannot be generic as it is difficult, though not impossible, to efficently perform arithmetics. 

- Should I define a concrete Path2D class or an abstract class and rely on existing classes?