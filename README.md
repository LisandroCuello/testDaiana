# testDaiana
El software lee el txt "testDaiana.txt", luego separa con las comas, crea una lista y guarda cada cadena en su determinada entidad (Nombre, dni, altura, estado).
En el siguente paso crea una conexion con una base de datos atras de Mysql, luego la inserta los datos de la lista.
Luego realiza un comando Mysql para traer de vuelta dichos datos (SELECT *), lo vuelve a guardar en una lista para despues poder realizar el filtrado de personas activas y mayores a 7.56 de altura con LINQ, y por ultimo lo serializa con JSon y lo imprime en pantalla los datos filtrados.
Imagen Output de consola:
![c#](https://user-images.githubusercontent.com/80378795/117054238-45510c00-acf0-11eb-88ed-2b13630a72dc.png)
