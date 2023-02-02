using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using JorgeSalineroSanchezPracticaADO.Models;
using System.Data;
using System.Drawing;

namespace JorgeSalineroSanchezPracticaADO.Repositories
{
    #region
    // procedimientos


    //get all clientes
    /*    CREATE PROCEDURE SP_CLIENTES
            AS
            SELECT* FROM clientes
            GO*/

    //get pedidos de cliente
    /*   
   CREATE PROCEDURE SP_PEDIDOS_CLIENTE(@CODCLIENTE NVARCHAR(50))
   AS
   SELECT* FROM pedidos WHERE CodigoCliente=@CODCLIENTE
   GO



    //Update clientes
    
         CREATE PROCEDURE SP_UPDATE_CLIENTES(@CODCLIENTE NVARCHAR(50),@EMPRESA NVARCHAR(50),@CONTACTO NVARCHAR(50),@CARGO NVARCHAR(50),@CIUDAD NVARCHAR(50),@TELEFONO INT)
        AS
        UPDATE clientes SET CodigoCliente=@CODCLIENTE,Empresa=@EMPRESA,Contacto=@CONTACTO,Cargo=@CARGO,Ciudad=@CIUDAD,Telefono=@TELEFONO 
        WHERE CodigoCliente=@CODCLIENTE
        GO




    INSERT PEDIDOS
         CREATE PROCEDURE SP_CREATE_PEDIDO(@CODPEDIDO NVARCHAR(50),@CODCLIENTE NVARCHAR(50),@FECHAENTREGA NVARCHAR(50),@FORMAENVIO NVARCHAR(50),@IMPORTE INT)
        AS
        INSERT INTO pedidos VALUES(@CODPEDIDO,@CODCLIENTE,@FECHAENTREGA,@FORMAENVIO,@IMPORTE)
        GO

    

    DELETE PEDIDOS
    CREATE PROCEDURE SP_DELETE_PEDIDO(@CODPEDIDO NVARCHAR(50))
        AS
        DELETE FROM pedidos WHERE CodigoPedido=@CODPEDIDO
        GO


     * 
     * */

    #endregion
    public class RepositoryPracAdo
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;

        public RepositoryPracAdo()
        {
            string connectionString = @"Data Source=DESKTOP-AIUEHVJ\SQLEXPRESS;Initial Catalog=PRACTICAADO;User ID=SA;Password=MCSD2023";

            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
        }

        public List<Cliente> GetClientes()
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<Cliente> clientes = new List<Cliente>();
            while (this.reader.Read())
            {
                Cliente cli = new Cliente();
                cli.CodCliente = this.reader["CodigoCliente"].ToString();
                cli.Empresa = this.reader["Empresa"].ToString();
                cli.Contacto = this.reader["Contacto"].ToString();
                cli.Cargo = this.reader["Cargo"].ToString();
                cli.Ciudad = this.reader["Ciudad"].ToString();
                cli.Telefono = int.Parse(this.reader["Telefono"].ToString());
                clientes.Add(cli);

            }
            this.reader.Close();
            this.cn.Close();
            return clientes;
        }

        public List<Pedido> GetPedidosDeCliente(string cliente)
        {
            SqlParameter pamidcliente = new SqlParameter("@CODCLIENTE", cliente);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS_CLIENTE";
            this.com.Parameters.Add(pamidcliente);
            this.cn.Open();
            List<Pedido> pedidos =new List<Pedido>();
            this.reader = this.com.ExecuteReader();
            while (this.reader.Read())
            {
                Pedido ped = new Pedido();
                ped.FormaEnvio = this.reader["FormaEnvio"].ToString();
                ped.CodCliente = this.reader["CodigoCliente"].ToString();
                ped.importe = int.Parse(this.reader["Importe"].ToString());
                ped.CodPedido = this.reader["CodigoPedido"].ToString();
                ped.FechaEntrega = this.reader["FechaEntrega"].ToString();
                pedidos.Add(ped);
            }
            this.reader.Close();
            this.com.Parameters.Clear();
            this.cn.Close();
            return pedidos;

        }
        public void UpdateCliente(Cliente cli)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_UPDATE_CLIENTES";
            SqlParameter pamcodcliente = new SqlParameter("@CODCLIENTE",cli.CodCliente);
            SqlParameter pamempresa = new SqlParameter("@EMPRESA", cli.Empresa);
            SqlParameter pamcontacto = new SqlParameter("@CONTACTO", cli.Contacto);
            SqlParameter pamcargo = new SqlParameter("@CARGO", cli.Cargo);
            SqlParameter pamciudad = new SqlParameter("@CIUDAD", cli.Ciudad);
            SqlParameter pamtelefono = new SqlParameter("@TELEFONO", cli.Telefono);
            this.com.Parameters.Add(pamcodcliente);
            this.com.Parameters.Add(pamempresa);
            this.com.Parameters.Add(pamcontacto);
            this.com.Parameters.Add(pamcargo);
            this.com.Parameters.Add(pamciudad);
            this.com.Parameters.Add(pamtelefono);
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.com.Parameters.Clear();
            this.cn.Close();
        }

        public int InsertPedidos(Pedido ped)
        {

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CREATE_PEDIDO";
            SqlParameter pamcodenvio = new SqlParameter("@CODPEDIDO", ped.CodPedido);
            SqlParameter pamcodcliente = new SqlParameter("@CODCLIENTE",ped.CodCliente);
            SqlParameter pamfechaentrega = new SqlParameter("@FECHAENTREGA",ped.FechaEntrega);
            SqlParameter pamformaenvio = new SqlParameter("@FORMAENVIO",ped.FormaEnvio);
            SqlParameter pamimporte = new SqlParameter("@IMPORTE", ped.importe);
            this.com.Parameters.Add(pamcodenvio);
            this.com.Parameters.Add(pamcodcliente);
            this.com.Parameters.Add(pamfechaentrega);
            this.com.Parameters.Add(pamformaenvio);
            this.com.Parameters.Add(pamimporte);
            this.cn.Open();
            int insertados=this.com.ExecuteNonQuery();
            this.com.Parameters.Clear();
            this.cn.Close();

            return insertados;

        }

        public int DeletePedidos(string CodPedido)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_PEDIDO";
            SqlParameter pamcodenvio = new SqlParameter("@CODPEDIDO", CodPedido);
            this.com.Parameters.Add(pamcodenvio);
            this.cn.Open();
            int borrados = this.com.ExecuteNonQuery();
            this.com.Parameters.Clear();
            this.cn.Close();

            return borrados;
        }

    }
}
