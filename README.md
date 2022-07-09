# Kentic Xperience 13 .NET Core Authentication Implementation

This repository shows a simple example of getting authentication up and running quickly to secure your site.  This does not include any security roles, just log in and out functionality.

## Prerequisites
* Your own Kentico Xperience installation with a connected database.
* Import the "Home" and "Page" page types into your Kentico Xperience instance.

## Implementation

To be successful with this implementation, you'll need to pay special attention to these items.

1. Implementing the code in this repository.  
1. Enabling the settings to ensure authentication will work on your site.
1. Setting the authentication permissions in the content tree of your Xperience implementation.


### Code Implementation
This project includes everything you need to get your .NETCore 6 site up and running in less than 30 minutes.

However, you really only need to pay attention to the files listed below to successfully implement authentication.


 `/Controllers/AccountController.cs`  
 `/Models/Users/Account/SignInViewModel.cs`  
 `/Views/Account/PermissionDenied.cshtml`  
 `/Views/Shared/_SignInLayout.cshtml`  
 `/Views/Account/SignIn.cshtml`  
 `/Views/Account/WaitingForApproval.cshtml`  
 `/Startup.cs`

 These 7 files contain the core elements to integrate into your existing implementation OR just use the whole project to test the waters.  

 ## Further Reading

 Check out this [blog post](https://kehrendev.com/blog/brenden-kehren/july-2022/implementing-authentication-in-kentico-xperience-13){:target="_blank"} with more specifics and how it all works together. 



