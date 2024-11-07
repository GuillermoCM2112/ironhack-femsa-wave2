#### QUERY1 ANTERIOR
```
SELECT Orders.OrderID,
SUM(OrderDetails.Quantity * OrderDetails.UnitPrice) AS TotalPrice
FROM Orders
JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
WHERE OrderDetails.Quantity > 10
GROUP BY Orders.OrderID;
```

#### QUERY1 REFACTORIZADO
```
-- Seleccionamos el OrderID de la tabla Orders y la suma de la cantidad multiplicada por el precio unitario de la tabla OrderDetails como TotalPrice
SELECT Orders.OrderID,
       SUM(OrderDetails.Quantity * OrderDetails.UnitPrice) AS TotalPrice
FROM Orders
-- Realizamos una subconsulta para filtrar los registros de OrderDetails donde la cantidad (Quantity) sea mayor a 10
JOIN (
    SELECT OrderID, Quantity, UnitPrice
    FROM OrderDetails
    WHERE Quantity > 10
) AS OrderDetails ON Orders.OrderID = OrderDetails.OrderID  -- Hacemos un JOIN con la tabla Orders usando el campo OrderID
GROUP BY Orders.OrderID;  -- Agrupamos los resultados por OrderID para obtener la suma de los precios por cada orden
```
Adicional agregar indices
```
CREATE INDEX idx_orders_orderid ON Orders(OrderID);

CREATE INDEX idx_orderdetails_orderid ON OrderDetails(OrderID);

CREATE INDEX idx_orderdetails_quantity ON OrderDetails(Quantity);
```

#### QUERY2 ANTERIOR
```
SELECT CustomerName
FROM Customers
WHERE City = 'London' ORDER BY CustomerName;
```

#### QUERY2 REFACTORIZADO

```
SELECT CustomerName
FROM Customers
WHERE CityID = 123
ORDER BY CustomerName;

-- Crear un índice en CityID para optimizar la búsqueda
CREATE INDEX idx_customers_cityid ON Customers (CityID);

```

