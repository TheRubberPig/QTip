# Setup

Please run `docker compose up` in the root directory to initialize the following
- Postgres SQL on port 5432 using a default user
- .NET 9.0 API at http://localhost:8080
- React UI App using Vite at http://localhost:3000

# Notes
Overall I found this a fun and interesting challenge, it took me longer to complete than I'd have liked due to being rusty with React applications and using postgres sql with docker. This took me roughly 5-6 hours to complete (excluding writing this readme) over the course of a couple of days as I unfortunately did not have a free block of time to complete this in one go. I'm happy with how this has turned out and while I've outlined some things I would have done differently below I feel that they are all acceptable for a fast prototype application.  

# Architectural Decisions

## Frontend
- I decided to use **React with Vite** for this project as React handles state changes in real time well and I have some past experience with it. I have tried to apply SOLID principles to the UI to keep things maintainable, though towards the end of the project it became difficult tracking the different components and ensuring data matched the API responses. Given more time, I would refactor this for better separation of concerns.
- For styling I used **tailwindcss** as it is widespread within the react ecosystem and was very easy to set up with Vite. Whilst a polished visual design was not a requirement tailwindcss added the necessary functionality to provide the highlighting requirement whilst also providing a modern styling solution.
- I chose **Vite** over Next.Js, Create React App (CRA), Angular etc was because Vite is highly optimized for small applications by using ES modules instead of Webpack. Along with faster build times, hot reloads and built in modern web browser support it felt like the best choice. For a small application like this the smaller overhead is another advantage but for production workloads Next.js or Vue.js would be a reasonable choice.
- I was going to use the axios package to manage API calls but after taking longer than planned I cut this and just used the native Javascript fetch.
- **Known Bug:** There is an issue with large bodys of text that makes the multi line input hard to edit. I skipped fixing this bug due to it being a minor visual problem and did not affect functionality. It is likely due to the scrollbar of the textarea messing up with the overlay for the tooltips. 

## Backend
- I used **.NET 9.0 with Minimal APIs** as I felt this provided the best solution for fast implementation, strong support within Visual Studio Code and good docker implementation. I am also comfortable with .NET and did not want to add further complexities by using a different technology stack.
- I used **EntityFramework** to connected to the DB and manage data as its quick, easy to follow and feature rich. Combined with **Npgsql** this felt like a very solid integration that manages complexities such as performance, security and postgressql type mapping.
- For testing and debugging I added a `/status` endpoint to confirm the API is running which can then be used in an AKS environment to detect if a container is down.
- To save time I skipped on unit tests but I'd have liked to added some even though it was not a requirement.
- I'd also like to add some error handling around accessing the database but I cut this due to time constraints.
- Given more time I'd also have like to handle CORS better and add an API standard like OpenAPI with swagger or an equivalent.  

## Database
- Initially I considered SQLite but switched to **PostgreSQL** for a quick set up, small container size and is widely adopted in production environments.
- I mapped submission records to tokens with a simple id to keep things clear and simple and so tokenized data can then be linked to the specific submission if required.
- I was going to add a way to get existing tokens if the same email was entered twice but decided against it as it could be a data security issue if someone was able to identify which email a specific token was linked to.

# Optional Extension Notes

While I didn't complete this due to time, I did however design the API with it in mind to handle different classification types and be able to get different counts based on the classification type. This was mainly to show how the API can be RESTful going forward and show how the API can be easily expanded on.