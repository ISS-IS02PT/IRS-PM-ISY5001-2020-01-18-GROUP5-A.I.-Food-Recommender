cd FoodApiClient
dotnet build
start dotnet run ApiUri=http://$(docker-machine ip):8000/ &
start http://localhost:5000 

