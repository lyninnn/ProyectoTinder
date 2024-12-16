using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using TinderApp.DTOs;

namespace TinderApp.Models
{
    public class TinderDB
    {

        private static string dbpath;
        private static string cadenaConexion;

        public TinderDB()
        {

            SetPath();
            cadenaConexion = $"Data Source = {dbpath};Foreign keys = True";

        }

        private void SetPath()
        {

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                dbpath = Path.Combine(FileSystem.AppDataDirectory, "TinderAPP.db");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {

                // Construye la ruta absoluta desde el directorio base
                dbpath = "C:\\Users\\turis\\Desktop\\ProyectoTinder\\TinderApp\\TinderDB\\TinderAPP.db";
            }
            else
            {
                Console.WriteLine("nothing happened here");
            }

            System.Diagnostics.Debug.WriteLine($"Database path: {dbpath}");

        }


        public async Task<Usuario> VerificarCredencial(string nombre, string contraseña)
        {
            Usuario usuario = null;
            using (var connection = new SqliteConnection(cadenaConexion))
            {

                await connection.OpenAsync();
                var createCommand = connection.CreateCommand();
                createCommand.CommandText = @"SELECT * FROM Usuario where nombre = @nombre AND contraseña = @contraseña;";
                createCommand.Parameters.AddWithValue("@nombre", nombre);
                createCommand.Parameters.AddWithValue("@contraseña", contraseña);
                using (var reader = await createCommand.ExecuteReaderAsync())
                {

                    await reader.ReadAsync();

                    usuario = new Usuario
                    {
                        UsuarioId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Edad = reader.GetInt32(2),
                        Genero = reader.GetString(3),
                        Ubicacion = reader.GetString(4),
                        Preferencias = reader.GetString(5),
                        Foto = reader.GetString(6)

                    };
                }
            }
            return usuario;
        }


        /* USUARIOS */

        public async Task<int> InsertarUsuario(Usuario usuario)
        {
            int row;
            using (var connection = new SqliteConnection(cadenaConexion))
            {

                await connection.OpenAsync();
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"INSERT INTO Usuario (nombre, edad, genero, ubicacion, preferencias, foto, contraseña) values (@nombre, @edad, @genero, @ubicacion, @preferencias, @foto, @contraseña)";
                insertCommand.Parameters.AddWithValue("@nombre", usuario.Nombre);
                insertCommand.Parameters.AddWithValue("@edad", usuario.Edad);
                insertCommand.Parameters.AddWithValue("@genero", usuario.Genero);
                insertCommand.Parameters.AddWithValue("@ubicacion", usuario.Ubicacion);
                insertCommand.Parameters.AddWithValue("@preferencias", usuario.Preferencias);
                insertCommand.Parameters.AddWithValue("@foto", usuario.Foto);
                insertCommand.Parameters.AddWithValue("@contraseña", usuario.Contraseña);
                row = await insertCommand.ExecuteNonQueryAsync();

            }

            return row;

        }


        public async Task<int> ActualizarUsuario(Usuario usuario)
        {
            int row = 0;

            using (var connection = new SqliteConnection(cadenaConexion))
            {

                await connection.OpenAsync();
                var updateCommand = connection.CreateCommand();
                updateCommand.CommandText = @"UPDATE Usuario SET nombre = @nombre, edad = @edad, genero = @genero, ubicacion = @ubicacion, preferencias = @preferencias,foto = @foto,Contraseña = @contraseña where id = @id";
                updateCommand.Parameters.AddWithValue("@nombre", usuario.Nombre);
                updateCommand.Parameters.AddWithValue("@edad", usuario.Edad);
                updateCommand.Parameters.AddWithValue("@genero", usuario.Genero);
                updateCommand.Parameters.AddWithValue("@ubicacion", usuario.Ubicacion);
                updateCommand.Parameters.AddWithValue("@preferencias", usuario.Preferencias);
                updateCommand.Parameters.AddWithValue("@foto", usuario.Foto);
                updateCommand.Parameters.AddWithValue("@contraseña", usuario.Contraseña);
                updateCommand.Parameters.AddWithValue("@id", usuario.UsuarioId);
                row =await updateCommand.ExecuteNonQueryAsync();

            }
            return row;

        }


        public async Task EliminarUsuario(int id)
        {

            using (var connection = new SqliteConnection(cadenaConexion))
            {

                await connection.OpenAsync();
                var deleteCommand = connection.CreateCommand();
                deleteCommand.CommandText = @"DELETE FROM Usuario where id = @id";
                deleteCommand.Parameters.AddWithValue("@id", id);
                await deleteCommand.ExecuteNonQueryAsync();

            }

        }

        public async Task<List<Usuario>> VerUsuario()
        {

            List<Usuario> usuarios = new List<Usuario>();

            using (var connection = new SqliteConnection(cadenaConexion))
            {

                await connection.OpenAsync();
                var createCommand = connection.CreateCommand();
                createCommand.CommandText = "SELECT * FROM Usuario";
                using (var reader = await createCommand.ExecuteReaderAsync())
                {

                    while (await reader.ReadAsync())
                    {

                        usuarios.Add(new Usuario
                        {
                            UsuarioId = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Edad = reader.GetInt32(2),
                            Genero = reader.GetString(3),
                            Ubicacion = reader.GetString(4),
                            Preferencias = reader.GetString(5),
                            Foto = reader.GetString(6)

                        });
                    }
                }
            }
            return usuarios;
        }
       
        // Like

        public async Task<int> InsertarLike(Like like)
        {
            int row;
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                await connection.OpenAsync();
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"INSERT INTO Like (user1_id, user2_id, fecha_like) values (@id1, @id2, @fecha)";
                insertCommand.Parameters.AddWithValue("@id1", like.id_user1);
                insertCommand.Parameters.AddWithValue("@id2", like.id_user2);
                insertCommand.Parameters.AddWithValue("@fecha", like.fechaLike);
                row = await insertCommand.ExecuteNonQueryAsync();
            }

            return row;
        }

        public async Task EliminarLike(int id)
        {

            using (var connection = new SqliteConnection(cadenaConexion))
            {

                await connection.OpenAsync();
                var deleteCommand = connection.CreateCommand();
                deleteCommand.CommandText = @"DELETE FROM like where id_like = @id";
                deleteCommand.Parameters.AddWithValue("@id", id);
                await deleteCommand.ExecuteNonQueryAsync();

            }

        }

        public async Task<List<Like>> VerLike()
        {

            List<Like> likes = new List<Like>();

            using (var connection = new SqliteConnection(cadenaConexion))
            {

                await connection.OpenAsync();
                var createCommand = connection.CreateCommand();
                createCommand.CommandText = "SELECT * FROM Like";
   
                using (var reader = await createCommand.ExecuteReaderAsync())
                {

                    while (await reader.ReadAsync())
                    {

                        likes.Add(new Like
                        {
                            id_like = reader.GetInt32(0),
                            id_user1 = reader.GetInt32(1),
                            id_user2 = reader.GetInt32(2),
                            fechaLike = reader.GetString(3)

                        });
                    }
                }
            }
            return likes;
        }
        public async Task<bool> ExisteLikeReciproco(int usuarioActualId, int usuarioLikeadoId)
        {
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                await connection.OpenAsync();

                // Corregir el paréntesis faltante
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Like WHERE ((user1_id = @usuarioActualId AND user2_id = @usuarioLikeadoId) OR (user1_id = @usuarioLikeadoId AND user2_id = @usuarioActualId))";
                command.Parameters.AddWithValue("@usuarioActualId", usuarioActualId);
                command.Parameters.AddWithValue("@usuarioLikeadoId", usuarioLikeadoId);

                var result = await command.ExecuteScalarAsync();
                if (Convert.ToInt32(result) > 0)
                {
                    return true;
                }

                return false;
            }
        }





        /* MATCHS */

        public async Task<int> InsertarMatch(Match match)
        {
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                await connection.OpenAsync();

              

                // Insertar el Match
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = 
                    "INSERT INTO Match (user1_id, user2_id, fecha_match) VALUES (@id1, @id2, @fecha)";
                insertCommand.Parameters.AddWithValue("@id1", match.Usuario1Id);
                insertCommand.Parameters.AddWithValue("@id2", match.Usuario2Id);
                insertCommand.Parameters.AddWithValue("@fecha", match.FechaMatch);

                return await insertCommand.ExecuteNonQueryAsync();
            }
        }


        public async Task ActualizarMatch(Match match)
        {

            using (var connection = new SqliteConnection(cadenaConexion))
            {

                await connection.OpenAsync();
                var updateCommand = connection.CreateCommand();
                updateCommand.CommandText = @"UPDATE Match SET user1_id = @id1, user2_id = @id2 where match_id = @id)";
                updateCommand.Parameters.AddWithValue("@id1", match.Usuario1Id);
                updateCommand.Parameters.AddWithValue("@id2", match.Usuario2Id);
                updateCommand.Parameters.AddWithValue("@id", match.MatchId);
                await updateCommand.ExecuteNonQueryAsync();

            }

        }


        public async Task EliminarMatch(int id)
        {

            using (var connection = new SqliteConnection(cadenaConexion))
            {

                await connection.OpenAsync();
                var deleteCommand = connection.CreateCommand();
                deleteCommand.CommandText = @"DELETE FROM Match where match_id = @id)";
                deleteCommand.Parameters.AddWithValue("@id", id);
                await deleteCommand.ExecuteNonQueryAsync();

            }

        }

        public async Task<List<Match>> VerMatch()
        {

            List<Match> match = new List<Match>();

            using (var connection = new SqliteConnection(cadenaConexion))
            {

                await connection.OpenAsync();
                var createCommand = connection.CreateCommand();
                createCommand.CommandText = "SELECT * FROM Match";
                using (var reader = await createCommand.ExecuteReaderAsync())
                {

                    while (await reader.ReadAsync())
                    {

                        match.Add(new Match
                        {
                            MatchId = reader.GetInt32(0),
                            Usuario1Id = reader.GetInt32(1),
                            Usuario2Id = reader.GetInt32(2),
                            FechaMatch =reader.GetString(3)

                        });
                    }
                }
            }
            return match;
        }




    }
}
