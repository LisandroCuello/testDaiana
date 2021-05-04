using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Globalization;

namespace testDaiana
{
    // Clase creada para obtener la conexion a la base de datos de MySql
    class Conexion
    {
        public static MySqlConnection obtenerConexion()
        {
            string conexionStr = "server=127.0.0.1; database=bd_personas; Uid=root; pwd='';";
            MySqlConnection con = new MySqlConnection(conexionStr);

            try
            {
                Console.WriteLine("Conectando a la base de datos...");
                con.Open();
                Console.WriteLine("Guardando datos..");

                //Retorna la conexion abierta
                return con;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
                return con;
            }
            

        }

    }

    // Clase Personas con sus entidades Nombre, Dni, Altura y Estado
    public class Personas : IEquatable<Personas>
    {
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public decimal Altura { get; set; }
        public bool Estado { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Personas objPersona = obj as Personas;

            if (objPersona == null) return false;
            else return Equals(objPersona);
        }

        public bool Equals(Personas otro)
        {
            if (otro == null) return false;
            return true;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //Se crea la lista personas basada en la clase Personas
            List<Personas> personas = new List<Personas>();

            //Se crea un StremReader que nos permite leer los caracteres del .Txt con los datos de las personas
            using (StreamReader leer = new StreamReader(@"TestDaiana.txt"))
            {

                Console.WriteLine("Pulse una tecla para leer el TXT y guardar los datos...");

                while (!leer.EndOfStream)
                {
                    // Lee una linea del .Txt, se divide cada cadena cuando aparece una "," y la guarda
                    string individuo = leer.ReadLine();
                    string[] atributosPersona = individuo.Split(",");

                    //Contador para guiar en que entidad se va a guardar los datos
                    int count = 0;

                    
                    CultureInfo alturaDecimal = new CultureInfo("en-US");

                    foreach (var p in atributosPersona)
                    {
                        //definimos altura en 0 para tener inicializada en cada vuelta del foreach
                        decimal altura = 0;
                       
                        //En caso de que la cadena del txt sera "INACTIVO" le va a dar un valor booleano false
                        bool estadoPersona = true;
                        if (p.Contains("INACTIVO")) 
                        {
                            estadoPersona = false;
                        }
                        


                        /*En este switch se utiliza el contador.
                          Si contador es 0, va a guardar todos los Nombre.
                          si es 1, los DNI.
                          Si es 2, las Alturas.
                          y si es 3, el estado de la persona
                        */
                        switch (count)
                        {
                            case 0: personas.Add(new Personas() { Nombre = p });
                                break;

                            case 1: personas.Add(new Personas() { Dni = p });
                                break;

                            case 2:
                                //Convierte el String en Decimal
                                if (!string.IsNullOrEmpty(p))
                                {
                                    altura = Convert.ToDecimal(p, alturaDecimal);
                                }

                                personas.Add(new Personas() { Altura = altura });
                                break;

                            case 3: personas.Add(new Personas() { Estado = estadoPersona });
                                break;

                            default: Console.WriteLine("error");
                                break;
                        }
                        count++;                        
                    }
                }
                Console.ReadKey();
                Console.WriteLine("Exito al guardar!");
            }

            //Se crea el objeto de la conexion
            MySqlConnection conex = Conexion.obtenerConexion();
         
            //Si sale todo bien, empieza a cargar los datos de la lista en la base de datos
             for (int i = 0; i <40; i+=4)
             {

                 string nombre = personas[0 + i].Nombre;
                 string dni = personas[1 + i].Dni;
                 decimal altura = personas[2 + i].Altura;
                 bool estado = personas[3 + i].Estado;

                //Convierte el string en un comando SQL
                 string sql = "INSERT INTO personas(nombre, dni, altura, estado) VALUE(@nombre, @dni, @altura, @estado)";
                 MySqlCommand cmd = new MySqlCommand(sql, conex);

                //Ulitiza las variable locales del for en los parametros del comando
                 cmd.Parameters.AddWithValue("@nombre", nombre);
                 cmd.Parameters.AddWithValue("@dni", dni);
                 cmd.Parameters.AddWithValue("@altura", altura);
                 cmd.Parameters.AddWithValue("@estado", estado);
                 cmd.ExecuteNonQuery();
             }
             
            

             //Convierte el String en un comando SELECT SQL para traer los datos de la base de datos 
             string consulta = "SELECT * FROM personas";
             MySqlCommand comm = new MySqlCommand(consulta, conex);

            //Se Crear una lista nueva para guardar los datos obtenidos de la base de datos
             List<Personas> personasBD = new List<Personas>();

            //Se lee los datos obtenidos
            using (var lector = comm.ExecuteReader())
            {
                CultureInfo alturaDecimal = new CultureInfo("en-US");

                int indice = 0;

                while (lector.Read())
                {
                    var nombre = lector.GetString(0);
                    var dni = lector.GetString(1);
                    var altura = lector.GetDecimal(2);
                    var estado = lector.GetBoolean(3);


                    personasBD.Insert(indice, new Personas()
                    {
                        Nombre = nombre,
                        Dni = dni,
                        Altura = altura,
                        Estado = estado
                    });
                    indice++;
                }
            }    
           
            //Se filtra la lista con los datos de la base de datos
            //Por altura y estado
            var personasActivas = personasBD.Where(x => x.Altura > 7.56m && x.Estado == true).ToList();

            // Se Serializa con Json
            var jsonPersonas = JsonSerializer.Serialize(personasActivas); 

            Console.WriteLine(jsonPersonas);
            Console.ReadKey();
        }
    }
}
