# EMarket
This is an e-commerce web application that allows users to browse, search for, and purchase products. Users can create accounts, log in, etc.

## Database Setup
#### 1. Configure Server Connection:
* Open the `appsettings.json` file in your project.
* Locate the `"DefaultConnection"` property and set its value to match your server name. For example: `"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=EMarket;Trusted_Connection=True;TrustServerCertificate=True;"`
Make sure that your machine has a local server instance.

#### 2. Open Package Manager Console
* In Visual Studio, go to `Tools`>`NuGet Package Manager`>`Package Manager Console`.
* Make sure that the selected `Default Project` is set to `EMarket.DataAccess`

#### 3. Run Database Migration
* In the Package Manager Console, enter the following command: `update-database`

  Your database setup should now be complete. You can now proceed with running your application.

## Challenges
1. Passing an object argument of complex type from the view to the controller was quite challenging especially when you need to retain a user input from the view. This would mean, as far as my experience helps me, that I would need to submit the form to the server to retain these inputs (routing would pass null properties for some reason). When there are `ModelErrors` for instance, I would have to reload the whole screen and the `Model` as if its new. This means that the `ModelErrors` wouldn't display. Took me a while to realize that I need to store a static list of `ModelErrors` so that I can reapply and display these on the next reload. However, I am not sure if this is the best approach.
2. When the database is deleted and the browser is closed and reopened, the user is still logged in as long as the cookies are stored. I was trying to experiment on cookie settings but nothing worked. To remedy this, I would check if the user exists in the database everytime the "/Home/Index" is called. If the user does not exist, the user is logged out and redirected to the login page.
3. Initially, every click of a button or checkbox caused the window to reload. This prevented `ErrorModels` to show, or caused the screen to jump to the top. To prevent this, I used AJAX requests, retrieved partial views, and modified the HTML rather than reloading the whole screen.
4. To remedy prodlem #1, I would often use JSON de/serialization to pass complex objects around. This would cause errors especially in models that is associated with another model where it is constrained by foreign key, creating circular references. These circular references would cause the system to break.
