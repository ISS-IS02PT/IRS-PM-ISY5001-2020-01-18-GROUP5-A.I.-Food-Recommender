cd FoodApiClient
dotnet build
killbg() {
        for p in "${pids[@]}" ; do
                kill "$p";
        done
}
trap killbg EXIT
pids=()
#Linux
#xdg-open http://localhost:5000 &
#Windows
start dotnet run ApiUri=http://127.0.0.1:8000/ &
pids+=($!)
start http://localhost:5000 

