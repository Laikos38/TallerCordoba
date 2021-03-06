﻿using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TallerAutos.BusinessLayer;
using TallerAutos.Entities;

namespace TallerAutos.GUILayer
{
    public partial class frmABMClientes : Form
    {
        private FormMode formMode = FormMode.insert;

        private bool nuevoVeh;
        private SexoService oSexService;
        private ModeloService oModeloService;
        private MarcaService oMarcaService;
        private Cliente clienteSeleccionado;       
        private ClienteService clienteService;
        private VehiculoService vehiculoService;
        
        public frmABMClientes()
        {
            InitializeComponent();
            InitializeDataGridView();
            tabFrmCliente.SelectedTab = tabCliente;
            oModeloService = new ModeloService();
            oMarcaService = new MarcaService();
            oSexService = new SexoService();
            clienteSeleccionado = new Cliente();
            clienteService = new ClienteService();
            vehiculoService = new VehiculoService();
        }

        public enum FormMode
        {
            insert,
            update,         
            details
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void FrmABMClientes_Load(object sender, EventArgs e)
        {
            LlenarCombo(cboMarca, oMarcaService.RecuperarMarcas(), "nombre", "codMarca");
            LlenarCombo(cboModelo, oModeloService.RecuperarModelos(), "nombre", "codModelo");
            LlenarCombo(cboSexo, oSexService.RecuperarSexos(), "nombre", "codSexo");
            //Cargo el DGV de los vehiculos
            CargarDGVehiculos();
            switch (formMode)
            {
                case FormMode.insert:
                    {
                        this.Text = "Cargar cliente";
                        HabilitarTxtCliente(true);
                        //Invisible el label, ya que aun no se tiene el nombre del usuario.
                        this.lblProfile.Visible = false;
                        this.btnVehiculos.Enabled = false;
                        HabilitarTxtVeh(false);
                        break;
                    }

                case FormMode.update:
                    {
                        this.Text = "Editar cliente";
                        this.MostrarDatos(clienteSeleccionado);
                        HabilitarTxtCliente(true);
                        HabilitarTxtVeh(false);
                        this.txtDNI.Enabled = false;
                        this.btnAceptarVeh.Enabled = false;
                        this.btnCancelarVeh.Enabled = false;

                        break;
                    }

                case FormMode.details:
                    {
                        this.Text = "Detalle de cliente";
                        this.MostrarDatos(clienteSeleccionado);
                        HabilitarTxtCliente(false);
                        HabilitarTxtVeh(false);
                        this.btnAceptarInfo.Enabled = false;
                        this.btnAceptarVeh.Enabled = false;
                        this.btnCancelarVeh.Enabled = false;
                        this.btnEliminar.Enabled = false;
                        this.btnEditar.Enabled = false;
                        this.btnNuevo.Enabled = false;

                        break;
                    }
            }
        }

        public void SeleccionarCliente(FormMode modo, Cliente clienteSel)
        {
            formMode = modo;
            clienteSeleccionado = clienteSel;
        }

        private void LlenarCombo(ComboBox combo, Object fuente, string display, String value)
        {
            combo.DataSource = fuente;
            combo.DisplayMember = display;
            combo.ValueMember = value;
            combo.SelectedIndex = -1;
        }

        private void MostrarDatos(Cliente C)
        {
            this.lblProfile.Text = C.Nombre + " " + C.Apellido;
            this.txtDNI.Text = C.Dni.ToString();
            this.txtNom.Text = C.Nombre;
            this.txtApellido.Text = C.Apellido;
            this.txtDomicilio.Text = C.Domicilio;
            this.txtEmail.Text = C.Email;
            this.txtTel.Text = C.Telefono;
            this.txtCel.Text = C.Celular;
            this.cboSexo.SelectedValue = C.Sexo.CodSexo;
            this.dtpFechaNac.Value = C.FechaNacimiento;
        }

        private void HabilitarTxtCliente(bool hab)
        {
            if (!hab)
            {
                this.txtDNI.Enabled = false;
                this.txtNom.Enabled = false;
                this.txtApellido.Enabled = false;
                this.txtDomicilio.Enabled = false;
                this.txtEmail.Enabled = false;
                this.txtTel.Enabled = false;
                this.txtCel.Enabled = false;
                this.cboSexo.Enabled = false;
                this.dtpFechaNac.Enabled = false;
              
            }
            else
            {
                this.txtDNI.Enabled = true;
                this.txtNom.Enabled = true;
                this.txtApellido.Enabled = true;
                this.txtDomicilio.Enabled = true;
                this.txtEmail.Enabled = true;
                this.txtTel.Enabled = true;
                this.txtCel.Enabled = true;
                this.cboSexo.Enabled = true;
                this.dtpFechaNac.Enabled = true;
            }
        }

        private void HabilitarTxtVeh(bool x)
        {
            if (x) {
                this.cboMarca.Enabled = true;
                this.cboModelo.Enabled = true;
                this.txtPatente.Enabled = true;
            }
            else
            {
                this.cboMarca.Enabled = false;
                this.cboModelo.Enabled = false;
                this.txtPatente.Enabled = false;
            }
       
        }

        private bool ValidarCamposObl()
        {
            if (String.IsNullOrEmpty(this.txtDNI.Text) || String.IsNullOrEmpty(this.txtNom.Text) || String.IsNullOrEmpty(this.txtApellido.Text) || cboSexo.SelectedIndex == -1)
            {
                return false;
            }
            return true;
        }

        private bool ValidarCamposVeh()
        {
            if (String.IsNullOrEmpty(this.txtPatente.Text) || cboMarca.SelectedIndex == -1 || cboModelo.SelectedIndex == -1)
            {
                return false;
            }
            return true;
        }

        private bool ValidarDNICliente()
        {
            string strSql = "AND C.dni = " + this.txtDNI.Text;
            if (clienteService.ConsultarClientes(strSql).Count > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidarPatente()
        {
            string strSql = "AND V.patente = " + "'" + this.txtPatente.Text + "'";
            if (vehiculoService.ConsultarVehiculos(strSql).Count > 0)
            {
                return false;
            }

            return true;
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            switch (formMode)
            {
                case FormMode.insert:
                    {
                        if (ValidarCamposObl())
                        {
                            if (ValidarDNICliente())
                            {
                                Cliente oCliente = new Cliente();
                                LlenarDatosCliente(oCliente);

                                clienteService.CargarCliente(oCliente);

                                MessageBox.Show("Usuario cargado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //this.Close(); Deshabilitado temporalmente

                                this.clienteSeleccionado = oCliente;
                                tabFrmCliente.SelectedTab = tabVehiculos;
                                this.HabilitarTxtVeh(true);
                                this.btnInfo.Enabled = false;
                                this.btnVehiculos.Enabled = true;
                                this.btnEliminar.Visible = false;
                                this.btnEditar.Visible = false;
                                this.btnNuevo.Visible = false;
                                this.btnAceptarVeh.Enabled = true;
                                this.btnCancelarVeh.Visible = false;
                            }
                            else
                                MessageBox.Show("Ya existe un cliente con el número de DNI ingresado. Por favor, ingrese un DNI diferente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show("No se han completado uno o más campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        break;
                    }

                case FormMode.update:
                    {
                        if (ValidarCamposObl())
                        {
                            
                            LlenarDatosCliente(clienteSeleccionado);

                            clienteService.ActualizarCliente(clienteSeleccionado);

                            MessageBox.Show("Usuario actualizado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //this.Dispose();

                        }
                        else
                            MessageBox.Show("No se han completado uno o más campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        break;
                    }

                case FormMode.details:

                    MostrarDatos(clienteSeleccionado);
                    this.Close();
                    break;
            }

        }

        private void LlenarDatosCliente(Cliente C)
        {
            C.Sexo = new Sexo();
            C.Dni = Convert.ToInt32(this.txtDNI.Text);
            C.Apellido = this.txtApellido.Text;
            C.Nombre = this.txtNom.Text;
            C.Domicilio = this.txtDomicilio.Text;
            C.Email = this.txtEmail.Text;
            C.Telefono = this.txtTel.Text;
            C.Celular = this.txtCel.Text;
            C.Sexo.CodSexo = Convert.ToInt32(this.cboSexo.SelectedValue);
            C.FechaNacimiento = this.dtpFechaNac.Value;
        }

        private void LlenarDatosVehiculo(Vehiculo V)
        {
            V.Marca = new Marca();
            V.Modelo = new Modelo();
            V.Cliente = new Cliente();
            V.Patente = this.txtPatente.Text;
            V.Marca.CodMarca = Convert.ToInt32(this.cboMarca.SelectedValue);
            V.Cliente.Dni = Convert.ToInt32(this.txtDNI.Text);
            V.Modelo.CodModelo = Convert.ToInt32(this.cboModelo.SelectedValue);
         
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void PanelTop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            tabFrmCliente.SelectedTab = tabCliente;
        }

        private void BtnVehiculos_Click(object sender, EventArgs e)
        {
            tabFrmCliente.SelectedTab = tabVehiculos;
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnAceptarVeh_Click(object sender, EventArgs e)
        {
            switch (formMode)
            {
                case FormMode.insert:
                    {
                        
                        if (ValidarCamposVeh())
                        {
                            if (ValidarPatente())
                            {
                                
                                Vehiculo oVehiculo = new Vehiculo();
                                LlenarDatosVehiculo(oVehiculo);

                                vehiculoService.CargarVehiculo(oVehiculo);
                                this.CargarDGVehiculos();

                                MessageBox.Show("Vehiculo cargado correctamente, el usuario ha sido creado con exito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);                              
                                this.Close();
                            }
                            else
                                MessageBox.Show("Ya existe un vehiculo con el número de Patente ingresado. Por favor, ingrese uno diferente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show("No se han completado uno o más campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        break;
                    }

                case FormMode.update:
                    {
                        if (ValidarCamposVeh())
                        {
                            Vehiculo objVehiculo = new Vehiculo();
                            LlenarDatosVehiculo(objVehiculo);

                            if (!nuevoVeh) { 
                                
                                

                                vehiculoService.ActualizarVehiculo(objVehiculo);

                                
                                MessageBox.Show("Vehículo actualizado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //this.Dispose();
                                
                            }
                            else {
                                vehiculoService.CargarVehiculo(objVehiculo);
                                MessageBox.Show("Vehículo añadido correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            this.CargarDGVehiculos();
                            this.txtPatente.Clear();
                            this.cboMarca.SelectedIndex = -1;
                            this.cboModelo.SelectedIndex = -1;
                            this.HabilitarTxtVeh(false);
                            this.btnAceptarVeh.Enabled = false;
                            this.btnCancelarVeh.Enabled = false;
                            this.btnEliminar.Enabled = true;
                            this.btnEditar.Enabled = true;
                            this.btnNuevo.Enabled = true;
                            this.dgvVehiculos.Enabled = true;
                        }
                        else
                            MessageBox.Show("No se han completado uno o más campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        break;
                    }

                case FormMode.details:

                    MostrarDatos(clienteSeleccionado);
                    this.Close();
                    break;


            }

        }

        private void InitializeDataGridView()
        {

            dgvVehiculos.ColumnCount = 3;
            dgvVehiculos.ColumnHeadersVisible = true;

            dgvVehiculos.AutoGenerateColumns = false;

            //Cargado
            dgvVehiculos.Columns[0].Name = "Patente";
            dgvVehiculos.Columns[0].DataPropertyName = "patente";

            dgvVehiculos.Columns[1].Name = "Marca";
            dgvVehiculos.Columns[1].DataPropertyName = "marca";

            dgvVehiculos.Columns[2].Name = "Modelo";
            dgvVehiculos.Columns[2].DataPropertyName = "modelo";

            /*dataGridClientes.AutoResizeColumnHeadersHeight();

            dataGridClientes.AutoResizeRows(
                DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);*/

            // Propiedades data grid view
            dgvVehiculos.ReadOnly = true;
            this.dgvVehiculos.MultiSelect = false;
            this.dgvVehiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvVehiculos.AllowUserToAddRows = false;
        }


        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.txtPatente.Clear();
            this.cboMarca.SelectedIndex = -1;
            this.cboModelo.SelectedIndex = -1;
            this.nuevoVeh = true;
            dgvVehiculos.Enabled = false;
            this.HabilitarTxtVeh(true);
            this.btnEditar.Enabled = false;
            this.btnEliminar.Enabled = false;
            this.btnNuevo.Enabled = false;
            this.btnAceptarVeh.Enabled = true;
            this.btnCancelarVeh.Enabled = true;
           
        }

        private void TabVehiculos_Click(object sender, EventArgs e)
        {

        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            this.nuevoVeh = false;
            this.HabilitarTxtVeh(true);
            this.btnEliminar.Enabled = false;
            this.btnEditar.Enabled = false;
            this.btnNuevo.Enabled = false;
            this.btnAceptarVeh.Enabled = true;
            this.btnCancelarVeh.Enabled = true;
        }

        private void CargarDGVehiculos()
        {
            this.dgvVehiculos.DataSource = vehiculoService.ConsultarVehiculos(" AND C.dni = " + clienteSeleccionado.Dni);
        }

        private void BtnCancelarVeh_Click(object sender, EventArgs e)
        {
            this.dgvVehiculos.Enabled = true;
            this.txtPatente.Clear();
            this.cboMarca.SelectedIndex = -1;
            this.cboModelo.SelectedIndex = -1;
            this.HabilitarTxtVeh(false);
            this.btnAceptarVeh.Enabled = false;
            this.btnCancelarVeh.Enabled = false;
            this.btnEliminar.Enabled = true;
            this.btnEditar.Enabled = true;
            this.btnNuevo.Enabled = true;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            Vehiculo objVeh = new Vehiculo();
            LlenarDatosVehiculo(objVeh);
            if (MessageBox.Show("Seguro que desea eiminar el vehículo seleccionado?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                vehiculoService.EliminarVehiculo(objVeh);
                MessageBox.Show("Vehículo eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.CargarDGVehiculos();
            }
        }

        private void DgvVehiculos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var vehiculo = (Vehiculo)dgvVehiculos.CurrentRow.DataBoundItem;
            txtPatente.Text = vehiculo.Patente.ToString();
            cboMarca.Text = vehiculo.Marca.Nombre.ToString();
            cboModelo.Text = vehiculo.Modelo.Nombre.ToString();
        }

        private void CboMarca_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LlenarCombo(cboModelo, oModeloService.ConsultarModelos(" AND MA.codMarca = " + cboMarca.SelectedValue), "nombre", "codModelo");
            cboModelo.SelectedIndex = 1;
        }

        private void TxtDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) //Al pulsar un número
            {
                e.Handled = false; //Se acepta
            }
            else if (Char.IsControl(e.KeyChar)) //Teclas especial como borrar
            {
                e.Handled = false; //Se acepta
            }
            else //Para todas las demas teclas
            {
                e.Handled = true; //No se acepta
            }
        }

        private void TxtCel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) //Al pulsar un número
            {
                e.Handled = false; //Se acepta
            }
            else if (Char.IsControl(e.KeyChar)) //Teclas especial como borrar
            {
                e.Handled = false; //Se acepta
            }
            else //Para todas las demas teclas
            {
                e.Handled = true; //No se acepta
            }
        }
    }


}
