# Canvas

.Net core do not have classes to handle graphics (look for system.drawing at https://blogs.msdn.microsoft.com/dotnet/2016/07/15/net-core-roadmap/).

Moving to .Net core is already a burden, as classes are spreaded in multiple assemblies. And yet they still have their legacy setbacks, such as `IList<T>` that still does not specialize `IReadOnlyList<T>`. Another missed opportunity.
Existing code have to be compiled and tested again. Some parts have to be rewritten.
So, what choice do we have? 

## Why W3C canvas ?

`System.Drawing.Graphics` is tied to GDI+. Making it abstract is somewhat possible but it is still a lot of work to implement as there is a lot of methods.

## Design notes

- Handling matrices with `System.Numerics.Matrix4x4`, as it is standard.