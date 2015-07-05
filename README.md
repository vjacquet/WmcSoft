# WmcSoft
This project contains classes and functions I found useful.

I try to respect the following laws, from "From Mathematics to Generic Programming", by Alexander A. STEPANOV and Daniel E. ROSE.

- The Law of Useful Return: _A procedure should return all the potentially useful information it compute._

- The Law of Separating Types: _Do not assume that two types are the same when they may be different._

- The Law of Completeness: _When designing an interface, consider providing all the related procedures._

- The Law of Interface Refinement: _Designing interfaces, like designing programs, is a multi-pass activity._

For now, all this code is a **work in progress**. Comments and questions are welcome.

## WmcSoft.Core
_Library of extensions and helpers._

## WmcSoft.Mathematics
_Library of mathematical objects, such as Rationals, Vectors and Matrices._

The main goal is to have 
them easy to use. They are not aimed to be the fastest but I hope they will be fast enough.

## WmcSoft.Units
_Library of units and quantities._

## WmcSoft.Business
_Library of components to build business applications._

## WmcSoft.Jobs
_Library to dispatch jobs. It predates the Task in .Net._

## WmcSoft.Components & WmcSoft.Windows.Forms
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

## WmcSoft.Interop
_Library of classes or functions encapsulating native functions._

## WmcSoft.VisualStudio
_Library to build custom tools._

This library also provide a declarative, policy based, code generator. 
The classes are defined in an XML file and then generated in CSharp.

## WmcSoft.AI
_Library of artificial intelligence algorithms._

This library is so far a collection of algorithm, initially converted from Practical Neural Network Recipes in C++, 
by Timothy Masters.
