using Core.DTOs;
using Core.Modelos.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Modelos;

namespace EF.Servicios
{
    public class TurnoService : ITurnoService
    {
        private readonly string _connectionString;

        public TurnoService(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) 
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
            }
            _connectionString = connectionString;
        }

        public async Task<List<Turnos>> GetTurnosActivadosAsync()
        {
            var turnosList = new List<Turnos>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ObtenerTurnosActivados", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var turno = new Turnos
                            {
                                IdTurno = reader.GetInt32(0),
                                IdUsuario = reader.GetInt32(1),
                                IdSucursal = reader.GetInt32(2),
                                FechaTurno = reader.GetDateTime(3),
                                Estado = reader.GetString(4)
                            };

                            turnosList.Add(turno);
                        }
                    }
                }
            }

            return turnosList;
        }

        public async Task<(Turnos turno, string message)> GetTurnoByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetTurnoById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdTurno", id);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {                            
                            if (reader.FieldCount == 5) 
                            {
                                var turno = new Turnos
                                {
                                    IdTurno = reader.GetInt32(0),
                                    IdUsuario = reader.GetInt32(1),
                                    IdSucursal = reader.GetInt32(2),
                                    FechaTurno = reader.GetDateTime(3),
                                    Estado = reader.GetString(4)
                                };

                                return (turno, "Consulta exitosa.");
                            }
                            else
                            {
                                // Caso en el que la primera fila es un mensaje de error
                                var message = reader.GetString(0);
                                return (null, message);
                            }
                        }

                        // Caso cuando no hay datos
                        return (null, "No se encontró el turno.");
                    }
                }
            }
        }

        public async Task<int> CreateTurnoAsync(Turnos turnos)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("InsertTurno", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdUsuario", turnos.IdUsuario);
                    command.Parameters.AddWithValue("@IdSucursal", turnos.IdSucursal);
                    command.Parameters.AddWithValue("@FechaTurno", turnos.FechaTurno);
                    command.Parameters.AddWithValue("@Estado", turnos.Estado);

                    var outputIdParameter = new SqlParameter("@NewIdTurno", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputIdParameter);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    return (int)outputIdParameter.Value;
                }
            }
        }

        public async Task<(bool success, string message)> UpdateTurnoAsync(Turnos turnos)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("UpdateTurno", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdTurno", turnos.IdTurno);
                    command.Parameters.AddWithValue("@IdUsuario", turnos.IdUsuario);
                    command.Parameters.AddWithValue("@IdSucursal", turnos.IdSucursal);
                    command.Parameters.AddWithValue("@FechaTurno", turnos.FechaTurno);
                    command.Parameters.AddWithValue("@Estado", turnos.Estado);

                    var messageParam = new SqlParameter("@Message", SqlDbType.VarChar, -1)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var operationExecutedParam = new SqlParameter("@OperationExecuted", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(messageParam);
                    command.Parameters.Add(operationExecutedParam);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    
                    bool success = (bool)operationExecutedParam.Value;
                    string message = (string)messageParam.Value;

                    return (success, message);
                }
            }
        }

    }
}
