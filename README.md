# Professional Partnerships

## Technology Stack
The project is built using:
 * [ASP.net core](https://docs.microsoft.com/en-us/aspnet/core/)
 * [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
 * [React](https://reactjs.org)
 * [Redux](https://redux.js.org)
 * [Typescript](https://www.typescriptlang.org)


## Requirements
### Visual Studio 2017 
The free Community edition of Visual Studio 2017 needs to be installed. You can [find the installer here.](https://www.visualstudio.com/vs/). When you install Visual Studio 2017 Community Edition, be sure to select the following components:
 * ASP.net and web development
 * Node.js development
 
### Visual Studio for Mac
The free Community edition of Visual Studio for Mac also seems to work. Make sure to install the .NET Core componennts.
 
## Configuration
The application pulls the database connection string from an environment variable.  You will need to create an environment variable called (using the correct DB credentials) `ConnectionStrings:PartnershipsDatabase` with the value `Server=emeraldcampus.database.windows.net;Database=Partnerships;User Id=USER_ID_GOES_HERE;Password='PASSWORD_GOES_HERE'`
 

## Using Entity Framework to Generate Model Classes
Entity Frame is able to generate the model classes necessary to communicate with the database.
 * Open up the Package Manager Console in VS2017 (Tools -> NuGet Package Manager -> Package Manager Console)
 * Make sure ProfessionalPartnerships.Data is selected in the Default Project Dropdown
 * Enter this command (using the correct DB credentials): `Scaffold-DbContext "Server=emeraldcampus.database.windows.net;Database=Partnerships;User Id=USER_ID_GOES_HERE;Password='PASSWORD_GOES_HERE'" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force`
