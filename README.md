# testDaiana
El software lee el txt "testDaiana.txt", luego separa por las comas del txt, crea una lista y guarda cada cadena en su determinada entidad (Nombre, dni, altura, estado).
En el siguente paso crea una conexion con una base de datos a través de Mysql y un servidor creado con Xampp, luego se inserta los datos de la lista.
A continuacion realiza un comando Mysql para obtener de vuelta dichos datos (SELECT *), lo vuelve a guardar en una lista para despues poder realizar el filtrado de personas "activas" y mayores a "7.56 de altura" con LINQ, y por ultimo lo serializa en JSon e imprime en pantalla los datos filtrados.

Imagen Output de consola:

![c#](https://user-images.githubusercontent.com/80378795/117054238-45510c00-acf0-11eb-88ed-2b13630a72dc.png)
