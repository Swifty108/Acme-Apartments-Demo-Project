<p align="center">
  <img width=238 src="https://user-images.githubusercontent.com/19508650/136641363-54ee714c-3495-4f74-93ab-d0f91e8b4ed9.jpg" />
</p>

# Acme Apartments Demo Web App

This is an ASP.NET Core MVC demo Web App used to manage a fictional apartment complex.

## Features:

There are three primary roles in the application: `Applicant`, `Resident`, and `Manager`.

The following is the flow of user interactions in the application: 

1. A visitor to the website applies for an apartment through the floorplans page and acquires the status of an `Applicant`. The applicant account dashboard features the ability to view the application status and contact the apartment staff.
2. Once the manager approves the application, the `Applicant` is promoted to `Resident` status. Residents can submit maintenance requests and view their application status if they applied to other apartment units. 
3. A `Manager` can approve or deny an application along with the authority to approve or deny maintenance requests.

## Technology Stacks and Tools Used

**Frameworks, Languages, and Tools:** C#, ASP.NET Core MVC 3.1, EF Core 3.1, Automapper 10.1.1, SQL Server, HTML5, CSS3, Bootstrap, jQuery, AJAX, Microsoft SQL Server Management Studio, Git, Git Flow, GitHub, xUnit and Moq libraries, Azure DevOps for project management, and Microsoft Teams for sprint planning mettings, code reviews, and pair programming

**Design & Architectural Patterns:** Generic repository pattern, Unit of work pattern, IOptions pattern, Dependency injection segregation pattern, MVC pattern, 3 Layer architectural pattern

**Visual Studio extensions:** Productivity Power Tools, CodeMaid, Github extension for Visual Studio 2019, Snippet Designer, and Web Essentials.

[Click here](https://github.com/rajndev/Acme-Apartments-ASP.NET-Core-Demo-Web-App/wiki) for the application wiki.

[Click here](https://acmeapartments.rajnarayanan.com) for a live demo of the project
