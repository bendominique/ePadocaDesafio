using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using WebAPiEpadoca.Models;

namespace WebAPiEpadoca.Controllers
{
    [RoutePrefix("api/padarias")]
    public class PadariaController : ApiController
    {

        List<Padaria> padarias = new List<Padaria>()
        {
            new Padaria
            {
                ID = 1,
                Nome_Padaria = "Padoca da Dona Florinda",
                Endereco = "Rua da Florinda, 123"
            }
        };

        [HttpPost]
        [Route("RegistrarPadaria")]
        public IHttpActionResult PostPadaria([FromBody] Padaria novaPadaria)
        {
            string conexao = ConfigurationManager.ConnectionStrings["ePadocaDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(conexao))
                {
                    conn.Open();

                    string verificaNome = "SELECT COUNT(*) FROM Padarias WHERE Nome_Padaria = @Nome_Padaria";
                    using (SqlCommand cmdVerifica = new SqlCommand(verificaNome, conn))
                    {
                        cmdVerifica.Parameters.AddWithValue("@Nome_Padaria", novaPadaria.Nome_Padaria);
                        int count = (int)cmdVerifica.ExecuteScalar();

                        if (count > 0)
                            return BadRequest("Já existe uma padaria com esse nome!");
                    }

                    string query = "INSERT INTO Padarias (nome_padaria, data_cadastro_padaria, endereco) VALUES (@nome_Padaria, @data_cadastro_padaria, @endereco)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome_Padaria", novaPadaria.Nome_Padaria);
                        cmd.Parameters.AddWithValue("@data_cadastro_padaria", novaPadaria.DataCadastro);
                        cmd.Parameters.AddWithValue("@endereco", novaPadaria.Endereco);

                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok("Padaria Cadastrada com Sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest("Erro: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllPadarias()
        {
            List<Padaria> padarias = new List<Padaria>();

            string connectionString = ConfigurationManager.ConnectionStrings["ePadocaDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT ID, Nome_Padaria, Endereco, data_cadastro_padaria FROM Padarias";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())

                        while (reader.Read())
                        {
                            Padaria padaria = new Padaria
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Nome_Padaria = reader["Nome_Padaria"].ToString(),
                                Endereco = reader["Endereco"].ToString(),
                                DataCadastro = Convert.ToDateTime(reader["data_cadastro_padaria"])
                            };

                            padarias.Add(padaria);

                 
                        }
                 }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao ler os dados: " + ex.Message);
                }
            }

            return Ok(padarias);
        }
    }
}
