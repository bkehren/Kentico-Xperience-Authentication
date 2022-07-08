# Kentic Xperience 13 .NET Core Authentication Implementation

This repository shows a simple example of getting authentication up and running quickly to secure your site.  This does not include any security roles, just log in and out functionality.

## Prerequisites
* You'll need your own Kentico Xperience installation with a connected database.

## Implementation

There are 3 main areas to ensure you are successful with this implementation 

1. Implementing the code in this repository.  
1. Enabling the settings to ensure authentication will work on your site.
1. Setting the authentication permissions in the content tree of your Xperience implementation.

**Note **
This implementation does not come with a Kentico Xperience implementation or the database.  It's expected you'll have this set up on your own.

### Code Implementation
This project includes everything you need to get your .NETCore 6 site up and running in less than 30 minutes.



1. Copy the following files into your project:  
    a. `/Controllers/AccountController.cs`  
    b. `/Models/Users/Account/SignInViewModel.cs`  
    c. `/Views/Account/PermissionDenied.cshtml`  
    d. `/Views/Account/SignIn.cshtml`  
    e. `/Views/Account/WaitingForApproval.cshtml`  
    f. `/Views/Shared/_SignInLayout.cshtml`  

1. Merge the changes from the following files into your project:  
    a. 


