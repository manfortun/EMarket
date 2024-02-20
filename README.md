# EMarket

To setup your database, first configure the server connection in the `appsettings.json` to match your server name:
`"DefaultConnection": "Server=localhost\\SQLEXPRESS..."`

Then go to Package Manager Console in Visual Studio and make sure that the selected Default Project is **`EMarket.DataAccess`**.

Enter the following command: `update-database`

Your database set up should now be complete.
