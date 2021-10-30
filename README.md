# BasketTest

## Project details

My BasketTest application is a Class library (BasketTestLib) written in C# .Net 5.

It has the abilty to add a range of products, and to apply vouchers to the basket total and calculate the resulting price. It can also handle a product being removed from the basket, and it then reviews any applied vouchers and if they are no longer valid feeds back to the client, and recalculates the total price (with updated discounts).

## Time spent

Including review of the problem, writing the class lib, writing the unit tests, testing the application and refactoring I have spent approx 5-6 hours on the assignment.

## Namespaces

The solution has the following namespaces:

- BasketTestLib.Services
- BasketTestLib.Models
- BasketTestLib.Interfaces
- BasketTestLib.Exceptions
- BasketTestLib.Extensions
- BasketTestLib.Tests

## Usage

Firstly create an instance of the BasketService class.

 BasketService uses constrcutor based dependency injection and therefore takes a parameter of type ICodeCheckService (ICodeCheckService is an interface which simulates calling off to an external service to check if a provided voucher code is valid. For the purposes of the unit tests, a stub is used to simulate this external service).

Now the client has a reference to an instance of Basket they can use the various methods on it such as "AddProduct", "ApplyVoucher", "GetBaskedFinalValue" etc.

Model classes can be instantiated to represent the various products and vouchers offered, and any behaviour defined.

## Architectural Considerations

- Product is the base abstract class that all Product concrete classes inherit from. 
- Products with sub categories such as HeadGear inherit from Product initially, and then the actual concrete implementation inherits from the HeadGear abstract class.
- Gift Vouchers inherit from product (because they are a product that can be bought) but also implement the Voucher interface so they have the functionality of vouchers available.

### SOLID principles

In the design of the BasketService component I have tried to keep to SOLID principles where applicable i.e: 

- Single responsibility (methods have been designed to only perform 1 task)
- Open/closed principle (BasketService designed in a way that a different implementation could be added based on the same interface that behaved different without modifying the original code)
- Interface segregation (all interfaces deal with only their relevant functionality), 
- Liskov substitution principle (no subclass overrides base class functionality so subclasses could be swapped for the base class) 
- Dependency Inversion (depending on abstractions i.e. interfaces, dependency injection). 

### Code complexity

I have used the Visual Studio Code metric tools to review the cyclometric complexity of the methods and reduced this where it got too high (>5 was the metric I used to trigger a refactor).

## Futher work to be considered

Things that I would consider adding should this demo be extended:

- Repository/unit of work pattern to interact with a datastore to represent a product stock database
- Add functionality to deal with the excess amounts from applied gift vouchers i.e. if a £50 voucher is used but max discountable is £25