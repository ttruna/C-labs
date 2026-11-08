# Practice 3 - Products (ASP.NET Core MVC)

    dotnet run

Open http://localhost:5003/products

Routes: /products, /products/index, /items, /items/index, /products/{id},
/products/create, /products/new, /products/edit/{id}, /products/delete/{id}

Filter: /products?name=milk&category=Dairy&minPrice=10&maxPrice=100

Admin: open /users/login, enter key df2323eoT (sent in the POST body to /users/index).
Without the key /users/index returns 401 Unauthorized.

    curl -i -X POST -d "key=df2323eoT" http://localhost:5003/users/index
