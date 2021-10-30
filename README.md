# BasketTest

## Project details

My BasketTest application is a Class library (BasketTestLib) written in C# .Net 5.

It has the abilty to add a range of products, and to apply vouchers to the basket total and calculate the resulting price. It can also handle a product being removed from the basket, and it then reviews any applied vouchers and if they are no longer valid feeds back to the client, and recalculates the total price (with updated discounts).

## Time spent

Including review of the problem, writing the class lib, writing the unit tests and refactoring I have spent approx 6 hours on the application.

## Namespaces

The solution has the following namespaces:

- BasketTestLib.Services
- BasketTestLib.Models
- BasketTestLib.Interfaces
- BasketTestLib.Exceptions
- BasketTestLib.Extensions
- BasketTestLib.Tests

## Usage

The client should invoke the "GetInstance" method of the BasketService, which is a singleton class. GetInstance returns the existing singleton or creates it. 

 BasketService uses constrcutor based dependency injection and therefore takes a parameter of type ICodeCheckService (ICodeCheckService is an interface which simulates calling off to an external service to check if a provided voucher code is valid. For the purposes of the unit tests, a stub is used to simulate this external service).

Once the client has a reference to the BasketService, they can call the "GetBasket" method which allocates a basket. The "BasketGuid" property of the returned basket can be used to retrieve the existing basket from the BasketService later if required.

Now the client has a reference to an instance of Basket they can use the various methods on it such as "AddProduct", "ApplyVoucher", "GetBaskedFinalValue" etc.

## Architectural Considerations

### Design patterns

- The BasketService is a Singleton for centralised control and management of baskets, including retrieving a basket by GUID
- The Basket is effectively a facade for the more complex logic such as ApplyVoucher - the client does not need to worry about the calculations going on under the hood
- The Strategy pattern is used to make the varying logic for vouchers cleaner, and this could also easily be extended if more voucher types were added later

### SOLID principles

In the design of the BasketService component I have tried to keep to SOLID principles where applicable i.e: 

- Single responsibility (methods have been designed to only perform 1 task)
- Interface segregation (all interfaces deal with only their relevant functionality), 
- Liskov substitution principle (no subclass overrides base class functionality) 
- Dependency Inversion (depending on abstractions i.e. interfaces, dependency injection). 

### Code complexity

I have used the Visual Studio Code metric tools to review the cyclometric complexity of the methods and reduced this where it got too high (>5 was the metric I used to trigger a refactor).

## Futher work to be considered

Things that I would consider adding should this demo be extended:

- Repository/unit of work pattern to interact with a datastore to represent a product stock database
- Add functionality to deal with the excess amounts from applied gift vouchers