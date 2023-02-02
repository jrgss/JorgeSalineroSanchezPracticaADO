using JorgeSalineroSanchezPracticaADO.Models;
using JorgeSalineroSanchezPracticaADO.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JorgeSalineroSanchezPracticaADO
{
    public partial class FormPractica : Form
    {
        RepositoryPracAdo persistencia;
        List<Cliente> clientes;
        List<Pedido> pedidos;
        public FormPractica()
        {
            InitializeComponent();
            this.persistencia = new RepositoryPracAdo();
            this.clientes= new List<Cliente>();
            this.pedidos=new List<Pedido>();
            this.LoadClientes();
        }

        private void LoadClientes()
        {
            cmbclientes.Items.Clear();
            this.clientes = this.persistencia.GetClientes();
            foreach(Cliente cli in clientes)
            {
                cmbclientes.Items.Add(cli.Empresa);
              
            }
        }
        
        private void LoadPedidos()
        {
            lstpedidos.Items.Clear();
            pedidos.Clear();
            pedidos = this.persistencia.GetPedidosDeCliente(clientes[cmbclientes.SelectedIndex].CodCliente);
            // MessageBox.Show("a"+pedidos.Count);

            foreach (Pedido ped in pedidos)
            {
                lstpedidos.Items.Add(ped.CodPedido);
            }
            txtcodigopedido.Text = "";
            txtfechaentrega.Text = "";
            txtformaenvio.Text = "";
            txtimporte.Text = "";
        }
        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbclientes.SelectedIndex != -1){
                Cliente cli = clientes[cmbclientes.SelectedIndex];
                txtempresa.Text = cli.Empresa;
                txtcargo.Text = cli.Cargo;
                txtciudad.Text = cli.Ciudad;
                txtcontacto.Text = cli.Contacto;
                txttelefono.Text = cli.Telefono.ToString();
               


                this.LoadPedidos();
               
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstpedidos.SelectedIndex != -1)
            {
                Pedido ped= pedidos[lstpedidos.SelectedIndex];
                txtcodigopedido.Text = ped.CodPedido;
                txtfechaentrega.Text = ped.FechaEntrega;
                txtformaenvio.Text = ped.FormaEnvio;
                txtimporte.Text = ped.importe.ToString();
            }
        }

        private void btnmodificarcliente_Click(object sender, EventArgs e)
        {
            Cliente cli = new Cliente();
            cli.CodCliente = clientes[cmbclientes.SelectedIndex].CodCliente;
            cli.Empresa = txtempresa.Text;
            cli.Telefono = int.Parse(txttelefono.Text);
            cli.Contacto=txtcontacto.Text;
            cli.Cargo= txtcargo.Text;
            cli.Ciudad=txtciudad.Text;

            //MessageBox.Show("Se va a modificar el cliente con codigo" + cli.CodCliente + " de " + clientes[cmbclientes.SelectedIndex].Ciudad + " y se va a ir a "+txtciudad.Text);
            persistencia.UpdateCliente(cli);
            this.LoadClientes();
            this.VaciarTodo();

        }

        private void VaciarTodo()
        {
            txtempresa.Text = "";
            txtcargo.Text = "";
            txtciudad.Text = "";
            txttelefono.Text = "";
            txtcontacto.Text = "";
            lstpedidos.Items.Clear();
            txtcodigopedido.Text = "";
            txtfechaentrega.Text = "";
            txtformaenvio.Text = "";
            txtimporte.Text = "";

        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            if (cmbclientes.SelectedIndex != -1) { 
            Pedido ped = new Pedido();
            ped.CodPedido = txtcodigopedido.Text;
            ped.CodCliente = clientes[cmbclientes.SelectedIndex].CodCliente;
            ped.FechaEntrega= txtfechaentrega.Text;
            ped.FormaEnvio = txtformaenvio.Text;
            ped.importe = int.Parse(txtimporte.Text);
                //MessageBox.Show("A" + clientes[cmbclientes.SelectedIndex].CodCliente);
                persistencia.InsertPedidos(ped);
               
                this.LoadPedidos();
            }
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            if (txtcodigopedido.Text != "")
            {

                //  MessageBox.Show("BORRO");
                string CodigoPedido = txtcodigopedido.Text;
             int borrados = persistencia.DeletePedidos(CodigoPedido);
                MessageBox.Show("Se han borrado " + borrados + " envios");
              
                this.LoadPedidos();
            }

        }
    }
}
