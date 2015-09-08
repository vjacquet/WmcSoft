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
for theses results.

## Providing storage
