#### QUERY1 ANTERIOR
```
db.posts
  .find({ status: "active" }, { title: 1, likes: 1 })
  .sort({ likes: -1 });
```

#### QUERY1 REFACTORIZADO
```
// 1. Crear un índice compuesto para optimizar la búsqueda y la ordenación.
// Este índice permite que MongoDB filtre por 'status' y ordene por 'likes' de manera eficiente.
db.posts.createIndex({ status: 1, likes: -1 });

/*
   Con el índice compuesto { status: 1, likes: -1 }, MongoDB puede:
   - Filtrar rápidamente los documentos donde 'status' sea "active".
   - Ordenar los documentos por 'likes' en orden descendente sin necesidad de ordenar en memoria, ya que el índice ya está ordenado.
*/

// Consulta optimizada que filtra por 'status', selecciona solo 'title' y 'likes', y ordena por 'likes' de manera descendente.
db.posts
  .find({ status: "active" }, { title: 1, likes: 1 })
  .sort({ likes: -1 })
  .limit(10);

/*
   Explicación:
   - El filtro { status: "active" } busca solo los documentos que están activos.
   - La proyección { title: 1, likes: 1 } significa que solo recuperaremos los campos 'title' y 'likes', reduciendo la cantidad de datos que se envían al cliente.
   - El método .sort({ likes: -1 }) ordena los documentos por el número de 'likes' en orden descendente, lo que garantiza que los posts con más 'likes' aparezcan primero.
   - El método .limit(10) limita la cantidad de resultados devueltos a 10, lo cual es útil si solo necesitas una página de resultados o los 10 posts más populares.
*/

```

#### QUERY2 ANTERIOR
```
db.users.aggregate([
  { $match: { status: "active" } },
  { $group: { _id: "$location", totalUsers: { $sum: 1 } } },
]);

```

#### QUERY2 REFACTORIZADO
```
// 1. Crear índices
db.users.createIndex({ status: 1 });  // Índice en 'status' para acelerar la búsqueda
db.users.createIndex({ location: 1 });  // Índice en 'location' para acelerar la agrupación

// 2. Consulta optimizada
db.users.aggregate([
  // Filtrar solo los usuarios activos
  {
    $match: { status: "active" }
  },

  // Agrupar por ubicación (location) y contar el total de usuarios por ubicación
  {
    $group: {
      _id: "$location",
      totalUsers: { $sum: 1 }
    }
  },

  // Ordenar los resultados por ubicación o por el total de usuarios
  {
    $sort: { totalUsers: -1 }  // Ordena por el total de usuarios en orden descendente
  }
]);


```

