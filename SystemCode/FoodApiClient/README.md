# Run food REST-API server
\drf-bootstrapping\foodrec_proj

# Run FoodApiClient
```bash
cd FoodApiClient
# run api client with default connection to http://127.0.0.1:8000/
dotnet run
# run api client with defined URI
dotnet run http://127.0.0.1:8000/
```

# Output
The api client will display the get requests to "Api Root", "Api Root/Food List" and "Api Root/Food List/Food Detail".
```bash
ApiRoot:        Uri = http://127.0.0.1:8000/foods/
        FoodList:       ID = 1,
                        Name = 6-inch Oven Roasted Chicken Breast Sandwich, Subway,
                        Uri = http://127.0.0.1:8000/foods/1/
                FoodDetail:     ID = 1,
                                Name = 6-inch Oven Roasted Chicken Breast Sandwich, Subway,
                                Group = FAST FOODS,
                                Nutrient Protein = 23 g,
                                Nutrient Sugar = 5.98 g,
                                Uri = http://127.0.0.1:8000/foods/1/
        FoodList:       ID = 2,
                        Name = Chicken, breast, fried, skinless,
                        Uri = http://127.0.0.1:8000/foods/2/
                FoodDetail:     ID = 2,
                                Name = Chicken, breast, fried, skinless,
                                Group = MEAT AND MEAT PRODUCTS,
                                Nutrient Protein = 40.13 g,
                                Nutrient Sugar = 0 g,
                                Uri = http://127.0.0.1:8000/foods/2/
```