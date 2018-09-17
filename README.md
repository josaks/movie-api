An earlier version of the API can be seen at http://themovieapi.azurewebsites.net/api/movies  
Currently all it does is return a list of movies.  

To run the application:  
Clone the repository  
Open the Visual Studio project file in VS  
Build the project and run by pressing IIS Express button  
	
Unit tests reside in the test project and can be run by selecting "Run all tests" in VS.
	
Note that a connectionstring must be provided to connect to a database.
To use a localDB database, the following can be added to appsettings.json in the MovieApi project:

"ConnectionStrings": {
		"devDB": "Server=(localDB)\\mssqllocaldb;Database=apimodb;Initial Catalog=apimodb;Trusted_Connection=True;MultipleActiveResultSets=true"
	}