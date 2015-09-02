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


## Providing storage
