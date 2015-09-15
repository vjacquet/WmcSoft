# WmcSoft.Business

This assembly regroups classes managing Party, PartyRoles & PartyRelationships, Products & Inventory, 
Customer Relationship and Order.

Theses models should be configurable and extensible, leading to some design decisions that will be recorded and explained here.

## Do not constraint the code

Some features, such as XML serialization, induce constrains on the way to write code. You should have an empty constructors 
and properties cannot be readonly. Worse, implementing IDictionary on RuleContext proved to be bothersome. 
Versioning can also become a difficult task.

## Deciding what is extensible, and what is not



## Creating services

Along with asp.net MVC and Entity Framework, MicroSoft provided a new UserManager class, generic on the user.
All methods return whether a Task on a piece of data or a Task on an IdentityResult. Returning tasks allows to continue 
with other tasks but, returning an IdentityResult is somehow bothersome.
This class contains a bool value indicating whether the call succeeded or not, and a list of IdentityErrors. 
You can fail and yet return no error. 

In .Net 4.6, you can create Task from an exception. Therefore, there is no need 
for theses "results" class.

## Providing storage

Most features can be enable only when extra data are stored. Therefore, the UserManager's Store can implement several interfaces and the UserManager have check methods
to tell the developer which are available. This design decision has two drawbacks:

0. the check is done at runtime;

0. the developer has more work because he/she have to be prepared for the fact that a feature is unavailable.

This can be solved by putting the Store as a Generic parameter and using constraints
on extensions methods, so the check can then be done at compile time.

