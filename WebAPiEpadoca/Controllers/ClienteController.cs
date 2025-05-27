using System;
using System.Configuration;
using System.Data;  
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPiEpadoca.Models;
using System.Web.Http;
using System.Data.SqlClient;
using System.Linq.Expressions;
namespace WebAPiEpadoca.Controllers
{

   

    [RoutePrefix("api/clientes")]
    public class ClienteController : ApiController
    {
        List<Cliente> clientes = new List<Cliente>
            {
            new Cliente {
                ID=1,
                Nome="Benjamin",
                Email="benjamin123@gmail.com",
                Telefone = "1140028922",
                Senha = "1234",
                Tipo = "admin"
            },

            new Cliente {
                ID=2,
                Nome="Ruth",
                Email="123ruth@gmail.com",
                Telefone = "11955512222",
                Senha = "4321",
                Tipo = "cliente"
            }


        };
        //// GET: Resultado
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllClientes()
        {
            List<Cliente> clientes = new List<Cliente>();

            string conexao = ConfigurationManager.ConnectionStrings["ePadocaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                string query = "SELECT ID, Nome, Email, Telefone, Senha, Tipo FROM Cliente";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Nome = reader["Nome"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefone = reader["Telefone"].ToString(),
                        Senha = reader["Senha"].ToString(),
                        Tipo = reader["Tipo"].ToString()
                    });
                }
            }

            return Ok(clientes);
        }



        //// POST: Postar 
        [HttpPost]
        [Route("registrar")]
        public IHttpActionResult PostCliente([FromBody] Cliente novoCliente)
        {
            try
            { 
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["ePadocaDB"].ConnectionString;

           
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                string query = "INSERT INTO Cliente (Nome, Email, Telefone, Senha, Tipo) VALUES (@Nome, @Email, @Telefone, @Senha, @Tipo)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nome", novoCliente.Nome);
                cmd.Parameters.AddWithValue("@Email", novoCliente.Email);
                cmd.Parameters.AddWithValue("@Telefone", novoCliente.Telefone);
                cmd.Parameters.AddWithValue("@Senha", novoCliente.Senha);
                cmd.Parameters.AddWithValue("@Tipo", novoCliente.Tipo);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Cliente cadastrado com sucesso!");

            }
            catch (Exception ex) {
                return BadRequest("Erro" + ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] Cliente loginInfo)
        {
            try
            {
                string conexao = ConfigurationManager.ConnectionStrings["ePadocaDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(conexao))
                {
                    string query = "SELECT * FROM Cliente WHERE Email = @Email AND Senha = @Senha";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", loginInfo.Email);
                    cmd.Parameters.AddWithValue("@Senha", loginInfo.Senha);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {

                        Cliente cliente = new Cliente
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Nome = reader["Nome"].ToString(),
                            Email = reader["Email"].ToString(),
                            Telefone = reader["Telefone"].ToString(),
                            Senha = "", // nunca devolve a senha
                            Tipo = reader["Tipo"].ToString()
                        };

                        return Ok(cliente);
                    }

                    else
                    {
                        return Unauthorized();
                    }
                }

            }
            catch (Exception ex) {
                return BadRequest("Erro no Login: " + ex.Message);
            }
        }
    } 
}
