## Monkey Fist: A Devise-ish Membership Library for .NET

One of the nice things about using Rails is that you can install libraries that "bolt on" and give you a horse-load of functionality. One of the things **I really dislike** about Rails is that you can install libraries that "bolt on" and give you a horse-load of functionality.

Devise is one of those libraries. When you install it, you instantly have a full membership system. It will extend (or create) a User class for you, create routes, give you global tools to use throughout your app - it's an amazing tool.

When you're first building out your app, this can be wonderful. After a year of running things you might want to customize a bit - that's when slugging it out with Rails and a Big Library like Devise can become a bother.

What I've always wanted is to unbolt what Devise does to how it injects itself into Rails. Specifically, I want:

 - The storage 
 - The classes and logic
 - The Remember Me/Reminder/Mailer stuff

In addition, I wish Devise would:

 - Have a logging system built-in
 - Store mailer templates in the DB so I can edit!
 - Track what was sent to whom and when

Basically I never really know "what happened" when using Devise, short of implementing my own logging bits. So I decided to build it myself, using .NET.

## The Building of This Is Documented in Video

I built this as part of a "Pragmatic BDD in .NET" course that I've created for Pluralsight. The course should be published within a few months and you can see how I created the specs using BDD and XUnit.

## Installation

Take the source (the MonkeyFist project) and add it to your solution. You could build it yourself if you like and drop in a DLL. My experience with NuGet thus far has been less than ideal so I don't really want to take the time to create/maintain it. If you'd like to help with this - I'd appreciate it.

## Usage

Have a look through the test suite and you'll see some examples on how to use MonkeyFist. Here are the basics:

```csharp
//register someone
using MonkeyFist;

//this is an Application for membership - not an executable
var app = new Application("rob@tekpub.com", "password", "password");
//our Registrator is a person who evaluates Applications
var result = new Registrator().ApplyForMembership(app);

```

The `result` in this example is a "response manifest" - which basically means it tells you what happened. It uses a type called RegistrationResponse:

```csharp
public class RegistrationResult {
  public User NewUser { get; set; }
  public Application Application { get; set; }
  public Guid SessionToken { get; set; }
  public bool Successful {
    get {
      return this.Application == null ? false : this.Application.IsAccepted();
    }
  }
}
```
This response tells you all you need to know about what just happened. 

The same deal applies with Authentication:

```csharp
var auth = new Authenticator();
_result = auth.AuthenticateUser(new Credentials { Email = "rob@tekpub.com", Password = "password" });
```

The result you get back is AuthenticationResult that tells you everything about what just happened (rather than the ever-helpful true/false):

```csharp
public class AuthenticationResult {
  public bool Authenticated { get; set; }
  public string Message { get; set; }
  public User User { get; set; }
  public UserSession Session { get; set; }
}
```

This is all wrapped up nicely in an API for you that is injectable as needed - however you're free to use only the classes above as-needed:

```csharp
using MonkeyFist;

//you can override this to pass in whatever services you like on a MonkeyFist.Configuration object
var membership = new Membership();
var result = membership.Register("rob@tekpub.com","password", "password");

```

MonkeyFist was built on top of LocalDB (just like SQL CE) and has all the migrations needed to get running. When you start up, MonkeyFist will create the tables it needs - just make sure the DB is there (although EF might build that too - not sure).

Just make sure you have a valid connectionString for MonkeyFist as well as some SMTP settings at the ready:

```xml
<connectionStrings>
    <add name="MonkeyFist"
    providerName ="System.Data.SqlClient"
    connectionString ="Data Source=(localdb)\Projects;Integrated Security=true;Initial Catalog=MonkeyFist"/>
</connectionStrings>

<system.net>
    <mailSettings>
        <smtp deliveryMethod="SpecifiedPickupDirectory" from="rob@tekpub.com">
            <specifiedPickupDirectory pickupDirectoryLocation="c:\temp\maildrop\" />
        </smtp>
    </mailSettings>
</system.net>
```

Note: the SMTP settings you see here are for testing purposes only. This allows MonkeyFist to drop mailers into a directory as mail files which you can read if you like.

## Working with ASP.NET 

MonkeyFist is designed to work directly with ASP.NET's FormsAuthentication. Personally I strongly dislike the way current .NET membership implementations are so strongly tied to the ASP.NET framework - but FormsAuth does it's job reasonably well. I created a gist for fun - this will change as I get a working web app together: https://gist.github.com/robconery/8012307



## Work in Progress

There's more to do here - including the creation of MVC-specific helpers (like "LoggedIn" etc) which I'm hoping to get to in the  near future. If you feel like helping - hurrah!



