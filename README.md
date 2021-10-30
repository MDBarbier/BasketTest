# BasketTest

## Project details

The BasketTest application is a Class library (BasketTestLib) written in C# .Net 5 which simulates a shopping basket service.

It has the abilty to add a range of products, and to apply vouchers to the basket total and calculate the resulting price.

## Namespaces

The solution has the following namespaces:

- BasketTestLib.Services
- BasketTestLib.Models
- BasketTestLib.Interfaces
- BasketTestLib.Exceptions
- BasketTestLib.Tests

## Usage

The client should invoke the "GetInstance" method of the BasketService, which is a singleton class. GetInstance returns the existing singleton or creates it. 

 BasketService uses constrcutor based dependency injection and therefore takes a parameter of type ICodeCheckService (ICodeCheckService is an interface which simulates calling off to an external service to check if a provided voucher code is valid. For the purposes of the unit tests, a stub is used to simulate this external service).

Once the client has a reference to the BasketService, they can call the "GetBasket" method which allocates a basket. The "BasketGuid" property of the returned basket can be used to retrieve the existing basket from the BasketService later if required.

Now the client has a reference to an instance of Basket they can use the various methods on it such as "AddProduct", "ApplyVoucher", "GetBaskedFinalValue" etc.

## Architectural Considerations

In the design of the BasketService component I have attempted to keep to SOLID principles where appropriate i.e: Single responsibility (methods have been designed to only perform 1 task), Interface segregation (all interfaces deal with only their relevant functionality), Liskov substitution (no subclass overrides base class functionality) and Dependency Inversion (dependency on external service is tied to an interfact so it can be stubbed using dependency injection). 

I have used the Visual Studio Code metric tools to review the cyclometric complexity of the methods and reduced this where it got too high (>5 was the metric I used).

## Futher work to be considered

Things that I would consider adding should this demo be extended:

- Repository/unit of work pattern to interact with a datastore to represent a product stock database
