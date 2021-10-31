# BasketTest

## Project details

My BasketTest application is a Class library (BasketTestLib) written in C# .Net 5.

It has the abilty to add a range of products, and to apply vouchers to the basket total and calculate the resulting price. It can also handle a product being removed from the basket, and it then reviews any applied vouchers and if they are no longer valid feeds back to the client, and recalculates the total price (with updated discounts).

## Time spent

Including review of the problem, writing the class lib, writing the unit tests, testing the application and refactoring I have spent approx 6 hours on the assignment over the course of a weekend.

## Namespaces

The solution has the following namespaces:

- BasketTestLib.Services
- BasketTestLib.Models
- BasketTestLib.Interfaces
- BasketTestLib.Exceptions
- BasketTestLib.Extensions
- BasketTestLib.Tests

## Usage

Import the class lib into a project that supports .Net 5 and add a reference.

Then create an instance of the BasketService class (and add a reference to BasketTestLib.Services). BasketService uses constructor based dependency injection and therefore takes a parameter of type ICodeCheckService (requires a reference to BasketTestLib.Interfaces). 

ICodeCheckService is an interface which simulates calling off to an external service to check if a provided voucher code is valid as an example of how the class library would deal with dependencies via IoC/DI. For the purposes of the unit tests, either a stub can be used to simulate this external service, or it can be mocked (included unit test project shows both approaches).

Once the client has a reference to an instance of BasketService they can use the various methods on it such as "AddProduct", "GetBaskedFinalValue" etc. Model classes can be instantiated (with a reference to BasketTestLib.Models) to represent the various products and vouchers offered, and any behaviour defined such as "ApplyVoucher".

## Features of application

- Multiple products can be added to a basket
- Products can be removed from a basket
- Removing a product will trigger a review of the net basket total and any applied vouchers
- Only one OfferVoucher can be added to a basket
- OfferVouchers can be specifically valid for certain products (or families of products)
- Multiple GiftVouchers can be added to a basket
- Gift vouchers can only be applied to non-gift voucher balances and purchases
- Gift vouchers do not apply to basket total for Offer voucher purposes
- Offer and Gift vouchers can be used together on a basket (subject to their own respective rules)
- If a voucher is not valid for any reason the application sends a message back to the client with details
- Final basket value (including all products and applied vouchers) can be calculated by calling the appropriate method

## Architectural Decisions

- I have used types and inheritance in the application to power decision making such as whether a product is valid for particular voucher etc
- Part of the reason I designed the application to be very type oriented was to keep repetition low and to try and follow the Principle of Least Knowledge
- 'Product' is the base abstract class that all Product concrete classes inherit from
- Products with sub categories such as the 'HeadGear' class inherit from Product initially, and then the actual concrete implementation inherits from the 'HeadGear' abstract class.
- The 'GiftVoucher' class inherits from 'Product' (because they are a product that can be bought) but also implement the 'IVoucher' interface so they have the functionality of vouchers available to them


### SOLID principles

In the design of the BasketService component I have tried to keep to SOLID principles where applicable i.e: 

- Single responsibility (methods have been designed to only perform 1 task)
- Open/closed principle (BasketService designed in a way that a different implementation could be added based on the same interface that behaved different without modifying the original code)
- Interface segregation (all interfaces deal with only their relevant functionality), 
- Liskov substitution principle (no subclass overrides base class functionality so subclasses could be swapped for the base class) 
- Dependency Inversion (depending on abstractions i.e. interfaces, dependency injection). 

### Code complexity

I have used the Visual Studio Code metric tools to review the cyclomatic complexity of the methods and reduced this where it got too high (>5 was the metric I used to trigger a refactor).

## Futher work to be considered

- Repository/unit of work pattern to interact with a datastore to represent a product stock database
- Add functionality to deal with the excess amounts from applied gift vouchers i.e. if a £50 voucher is used but max discountable is £25
- Look into why £ symbol does not load from Xunit project correctly when tests are run in Github action
- If other types of voucher needed to be handled i.e. an offer voucher for different clients or special events, Strategy pattern could be employed to handle this. I initially employed this in my code but decided it was unnessecary for the current requirements

## Test status

![BasketTestLib](https://github.com/MDBarbier/BasketTest/actions/workflows/dotnet.yml/badge.svg)