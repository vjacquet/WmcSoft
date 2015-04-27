# WmcSoft
This project contains classes and functions I found useful.

For now, all this code is a **work in progress**. Comments and questions are welcome.

## WmcSoft.Core
_This library regroup extensions and helpers._

## WmcSoft.Mathematics
_Library of mathematical objects, such as Rationals, Vectors and Matrices._

The main goal is to have 
them easy to use. They are not aimed to be the fastest but I hope they will be fast enough.

## WmcSoft.Units
_Library of units and quantities._

## WmcSoft.Jobs
_Library to dispatch jobs. It predates the Task in .Net._

## WmcSoft.Components & WmcSoft.Windows.Forms
_Libraries with component and windows forms controls._

These are pretty much useless now, as the component model and the windows form are kind of deprecated.

I liked the whole component/designer concept, the idea that we can have different view of the code
and how it helped building other component. 

Yet, the main drawback I see is that it requires to shape the code in a specific way, 
especially mutating the component through properties. It made the component sometimes harder 
to implement.
