# MovieApi
## TODO: 
- Fels�ka /api/Movies/{movieId}/actors/{actorId}? Maybe it was just that I killed the server :D
-- Move to ActorsController? 

## Todo
- Add controller and DTOs for director

### extras 1
- Todo: Add a new endpoint to get all movies by a specific director
- Todo: Add a new endpoint to get all movies by a specific genre
- Todo: Add a new endpoint to get all movies by a specific actor
- Todo: Add a new endpoint to get all movies by a specific year, or range of years (1985-1990 for example)

## Extras 2
- Todo: Add ReportsController listing movies by review data:
- (top5 per genre, average ratings per genre, most active actors movies with most reviews, most common genre, etc.)
 
## Extras 3
- Add faker data to the database to replace the current seed data?
- Move the seeddata method and data into a service that can be called from the controller?