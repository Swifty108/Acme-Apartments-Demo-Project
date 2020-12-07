# Peach Grove Apartments Demo Web App

This is an ASP.NET Core MVC Web App used to manage a fictional apartment complex.

## Technology Stacks Used

ASP.NET Core MVC 3.1, EF Core 3.1, Automapper 10.1.1, SQL Server, HTML5, CSS, Bootstrap, Microsoft SQL Server Management Studio, Git for Version Control, Azure DevOps for project management, and Microsoft Teams for code reviews.  

For productivity purposes, the following Visual Studio extensions were used: Productivity Power Tools, CodeMaid, Github extension for VS 2019, Snippet Designer, and Web Essentials. Also, these Android apps were used to facilitate effective time management: Microsoft To-Do app, Trello app, and the Pomodoro app.

The architectural pattern used for this project is the Three-Layer Architecture.

## Features

There are three primary roles in the application, `Applicant`, `Resident`, and `Manager`.

The following is the flow of user interactions within these three roles: 

1. A visitor to the website applies for an apartment through the floorplans page and acquires the status of an `Applicant`. The applicant account dashboard features the ability to view the application status and contact the apartment staff.
2. Once the manager approves the application, the `Applicant` is promoted to `Resident` status. Residents can submit maintenance requests and view their application status if they applied. 
3. A `Manager` can approve or disapprove an application along with the authority to approve or deny maintenance requests.
