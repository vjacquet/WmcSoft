# WmcSoft
This project contains classes and functions I found useful.

I try to respect the following laws, from "From Mathematics to Generic Programming", by Alexander A. STEPANOV and Daniel E. ROSE.

- The Law of Useful Return: _A procedure should return all the potentially useful information it compute._

- The Law of Separating Types: _Do not assume that two types are the same when they may be different._

- The Law of Completeness: _When designing an interface, consider providing all the related procedures._

- The Law of Interface Refinement: _Designing interfaces, like designing programs, is a multi-pass activity._

For now, all this code is a **work in progress**. Comments and questions are welcome.

## Design notes

To improve readability:

- Extensions methods calling extensions methods in the same class should be called as function.

        public static IEnumerable<string> Tokenize(this string self, char separator) {
            return Tokenize(self, new CharTokenizer(separator));
        }

- Extensions methods yielding `IEnumerable<T>` should check the arguments and then return the result from an unguarded version. 
  The unguarded version might be called separately from other places and the verification of the arguments will not be defered 
  until the start of the enumeration. It serves two purpose: not only the unguarded can be called from other algorithms but also the 
  validation of the arguments will be done right away and not when the enumeration starts.

        static IEnumerable<T> UnguardedBackwards<T>(IReadOnlyList<T> source) {
            for (int i = source.Count - 1; i >= 0; i--) {
                yield return source[i];
            }
        }

        public static IReadOnlyCollection<T> Backwards<T>(this IReadOnlyList<T> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new ReadOnlyCollectionAdapter<T>(source.Count, UnguardedBackwards(source));
        }

  On a side note, it is unfortunate that the convention for a null `this` argument of the extensions should throw `ArgumentNullException`
  instead of `NullReferenceException`. Not only the latter is what calling a method on a null reference would throw but also we would not
  have to always guard for null. Anyway, we will follow this convention the Linq context. 
  In all other cases, we prefer to throw `NullReferenceException`.

- For readability, extensions methods should not accept a null `this`. If the method is defined when the first parameter is null, then the method should not be an extension method. `using static`.

- Linq-like extensions methods should not mutate the enumerable, therefore `ForEach` on an enumerable is not recommended, as stated by Eric Lippert in [“foreach” vs “ForEach”](https://blogs.msdn.microsoft.com/ericlippert/2009/05/18/foreach-vs-foreach/).

- Operator overloads should provide the equivalent with a named function. Apparently, the framework does not have 
a clear policy on whether the function should be static or not. It is static for `Complex` but not for `TimeSpan`.

        public static Rational operator -(Rational x) {
            return new Rational(-x._numerator, x._denominator);
        }
        public static Rational Negate(Rational x) {
            return -x;
        }

## Libraries

### WmcSoft.Core
_Library of extensions and helpers._

### WmcSoft.Mathematics
_Library of mathematical objects, such as Rationals, Vectors and Matrices._

The main goal is to have 
them easy to use. They are not aimed to be the fastest but I hope they will be fast enough.

### WmcSoft.Units
_Library of units and quantities._

### WmcSoft.Business.Primitives
_Library of primitives types to build business applications._

- Time, adapted from <http://domainlanguage.com>'s time & money:

  - `TimePoint` is equivalent to DateTime in GMT yet it is more abstract as it does not have accessors to its part.
  - `DateTime`, with a `DateTimeKind.Unspecified`, replaces `CalendarMinute`
  - `TimeUnit` should derive from WmcSoft's units.
  - `newOfSameType()` factory methods are not required for value types.

QuantLib also defines some time related types. The current implementatio of `TimeUnit` is close to `Period`. The types might be merged.

Finally, it looks like a [`Date`](https://github.com/dotnet/corefxlab/blob/master/src/System.Time/System/Date.cs) and 
[`TimeOfDay`](https://github.com/dotnet/corefxlab/blob/master/src/System.Time/System/TimeOfDay.cs) types 
are being added to the framework therefore the current design should be as close as possible to those.

### WmcSoft.Business.Calendars
_Library of utilities to manage business calendar._


### WmcSoft.Business
_Library of components to build business applications._

- Security

- RuleModel

- Party, PartyRole & PartyRelationship

- ProductType, ProductInstance, Package

- Accounting

### WmcSoft.Jobs
_Library to dispatch jobs. It predates the Task in .Net._

### WmcSoft.Components & WmcSoft.Windows.Forms
_Libraries of component and windows forms controls._

These are pretty much useless now, as the component model and the windows form are kind of deprecated.

I liked the whole component/designer concept, the idea that we can have different view of the code
and how it helped building other component.

Yet, the first drawback I see is that it requires to shape the code in a specific way, 
especially mutating the component through properties. It made the component sometimes harder 
to implement, or lead to suboptimal implementations.  
The second drawback is related to forms, user controls and inheritance because there is a conflict 
between access control and visual representation. How do you hide in code what is visible in display? 
How can you see controls and yet be unable to move them inside the form? 

### WmcSoft.Interop
_Library of classes or functions encapsulating native functions._

### WmcSoft.VisualStudio
_Library to build custom tools._

This library also provide a declarative, policy based, code generator. 
The classes are defined in an XML file and then generated in CSharp.

### WmcSoft.AI
_Library of artificial intelligence algorithms._

This library is so far a collection of algorithm, initially converted from Practical Neural Network Recipes in C++, 
by Timothy Masters, and C++ Neural Networks & Fuzzy Logic by Rao & Rao.
