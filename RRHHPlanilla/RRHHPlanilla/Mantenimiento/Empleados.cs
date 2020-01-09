using RRHH.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace RRHHPlanilla
{
    public partial class Empleados : Form
    {
        TrabajadoresBL _trabajadores;
        CargosBL _cargosBL;
        JornadaBL _jornadaBL;
        EstadoCivilBL _estadocivilBL;
        MetodoPagoBL _metodopagoBL;
        SexoBL _sexoBL;

        public Empleados()
        {
            InitializeComponent();

            _sexoBL = new SexoBL();
            listaSexosBindingSource.DataSource = _sexoBL.ObtenerSexos();

            _trabajadores = new TrabajadoresBL();
            listaTrabajadoresBindingSource.DataSource = _trabajadores.ListaTrabajadores;  //ObtenerTrabajador();

            _cargosBL = new CargosBL();
            listaCargosBindingSource.DataSource = _cargosBL.ObtenerCargos();

            _jornadaBL = new JornadaBL();
            listaJornadasBindingSource.DataSource = _jornadaBL.ObtenerJornadas();

            _estadocivilBL = new EstadoCivilBL();
            listaEstadoCivilesBindingSource.DataSource = _estadocivilBL.ObtenerEstadoCiviles();

            _metodopagoBL = new MetodoPagoBL();
            listaMetodoPagosBindingSource.DataSource = _metodopagoBL.ObtenerMetodoPagos();

        }

        #region MenuStrip
        //GUARDAR
        private void listaTrabajadoresBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            if (codigoBarrasPictureBox.Image == null && nombreTextBox.Text != "" || nombreTextBox.Text != "")
            {
                BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode();
                Codigo.IncludeLabel = true;
                codigoBarrasPictureBox.Image = Codigo.Encode(BarcodeLib.TYPE.CODE128, nombreTextBox.Text + " " + apellidoTextBox.Text, Color.Black, Color.White, 400, 100);
            }


            listaTrabajadoresBindingSource.EndEdit();
            var trabajador = (Trabajador)listaTrabajadoresBindingSource.Current;

            if (fotoPictureBox.Image != null)
            {
                trabajador.Foto = Program.imageToByteArray(fotoPictureBox.Image);
            }
            else
            {
                trabajador.Foto = null;
            }


            if (codigoBarrasPictureBox.Image != null)
            {
                trabajador.CodigoBarras = Program.imageToByteArray(codigoBarrasPictureBox.Image);
            }
            else
            {
                trabajador.CodigoBarras = null;
            }

            var resultado = _trabajadores.GuardarTrabajador(trabajador);


            if (resultado.Exitoso == true)
            {
                listaTrabajadoresBindingSource.ResetBindings(false);
                DialogResult resul = MessageBox.Show("Usuario Guardado", "Exitoso...!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                DeshabilitarHabilitarBotones(true);
                textBox1.Text = "";
                button3.PerformClick();
                DesabilitarEdicion();
            }
            else
            {
                MessageBox.Show(resultado.Mensaje);
                HabilitarEdicion();
            }

            listaTrabajadoresBindingNavigatorSaveItem.Enabled = false;

        }

        //AGREGAR
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            //textBox1.Text = null;
            //button3.PerformClick();
            listaTrabajadoresBindingSource.DataSource = _trabajadores.ObtenerTrabajador();

            listaTrabajadoresBindingNavigatorSaveItem.Enabled = true;
            _trabajadores.AgregarTrabajador();
            listaTrabajadoresBindingSource.MoveLast();
            HabilitarEdicion();

            DeshabilitarHabilitarBotones(false);
        }

        //BORRAR
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {

            if (idTextBox.Text != "")
            {
                DialogResult resulto = MessageBox.Show("¿Desea Eliminar el Registro?", "Eliminar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resulto == DialogResult.Yes)
                {
                    var id = Convert.ToInt32(idTextBox.Text);
                    Eliminar(id);
                }
            }
        }

        //ELIMINAR
        private void Eliminar(int id)
        {
            var resultado = _trabajadores.EliminarTrabajador(id);

            if (resultado == true)
            {
                listaTrabajadoresBindingSource.ResetBindings(false);
            }
            else
            {
                MessageBox.Show("Ocurrio un error al eliminar un trabajador");
            }
        }

        //CANCELAR CAMBIOS
        private void toolStripCancelar_Click(object sender, EventArgs e)
        {
            _trabajadores.CancelarCambios();
            DesabilitarEdicion();
            listaTrabajadoresBindingNavigatorSaveItem.Enabled = false;
            DeshabilitarHabilitarBotones(true);


            if (textBox1.Text == "")
            {
                _trabajadores = new TrabajadoresBL();
                listaTrabajadoresBindingSource.DataSource = _trabajadores.ListaTrabajadores;  //ObtenerTrabajador();
                textBox1.Text = "";
                listaTrabajadoresBindingSource.Clear();

            }
            else
            {
                if (textBox1.Text != "")
                {
                    textBox1.Contains(button3);
                }
            }
        }
    
        //EDITAR
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                DesabilitarEdicion();
                DialogResult result = MessageBox.Show("Busque primero un empleado antes de editar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                if (textBox1.Text != "")
            {
                listaTrabajadoresBindingNavigatorSaveItem.Enabled = true;
                bindingNavigatorAddNewItem.Enabled = false;
                toolStripCancelar.Visible = true;
                toolStripCancelar.Enabled = true;
                HabilitarEdicion();
            }
        }
        #endregion

        #region BotonesEdicion Y Desabilitar
        public void HabilitarEdicion()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            nombreTextBox.Enabled = true;
            apellidoTextBox.Enabled = true;
            edadTextBox.Enabled = true;
            sueldoTextBox.Enabled = true;
            direccionTextBox.Enabled = true;
            cedulaTextBox.Enabled = true;
            fechaInicioDateTimePicker.Enabled = true;
            comboBox1.Enabled = true;
            estadoCivilIdComboBox.Enabled = true;
            cargoIdComboBox.Enabled = true;
            metodoPagoIdComboBox.Enabled = true;
            jornadaIdComboBox.Enabled = true;
        }

        public void DesabilitarEdicion()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            nombreTextBox.Enabled = false;
            apellidoTextBox.Enabled = false;
            edadTextBox.Enabled = false;
            sueldoTextBox.Enabled = false;
            direccionTextBox.Enabled = false;
            cedulaTextBox.Enabled = false;
            fechaInicioDateTimePicker.Enabled = false;
            comboBox1.Enabled = false;
            estadoCivilIdComboBox.Enabled = false;
            cargoIdComboBox.Enabled = false;
            metodoPagoIdComboBox.Enabled = false;
            jornadaIdComboBox.Enabled = false;
        }

        private void DeshabilitarHabilitarBotones(bool valor)
        {
            bindingNavigatorMoveFirstItem.Enabled = valor;
            bindingNavigatorMoveLastItem.Enabled = valor;
            bindingNavigatorMovePreviousItem.Enabled = valor;
            bindingNavigatorMoveNextItem.Enabled = valor;
            bindingNavigatorPositionItem.Enabled = valor;
            bindingNavigatorAddNewItem.Enabled = valor;
            bindingNavigatorDeleteItem.Enabled = valor;
            toolStripCancelar.Visible = !valor;
            toolStripButton1.Enabled = valor;

        }
        #endregion

        #region BOTONES
        //FOTO
        private void button1_Click(object sender, EventArgs e)
        {
            var trabajador = (Trabajador)listaTrabajadoresBindingSource.Current;

            if (trabajador != null)
            {
                openFileDialog1.ShowDialog();
                var archivo = openFileDialog1.FileName;

                if (archivo != "")
                {
                    var fileInfo = new FileInfo(archivo);
                    var filestream = fileInfo.OpenRead();

                    fotoPictureBox.Image = Image.FromStream(filestream);
                };
            }
            else
            {
                MessageBox.Show("Cree un trabajador antes de asignarle una imagen");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fotoPictureBox.Image = null;
        }

        private void piccerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var buscar = textBox1.Text;

            if (string.IsNullOrEmpty(buscar) == true)
            {
                _trabajadores = new TrabajadoresBL();
                listaTrabajadoresBindingSource.DataSource = _trabajadores.ListaTrabajadores;
            }

            if (string.IsNullOrEmpty(buscar) != true)
            {
                

                listaTrabajadoresBindingSource.DataSource =
                    _trabajadores.ObtenerTrabajadores(buscar);


                listaTrabajadoresBindingSource.ResetBindings(false);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listaTrabajadoresBindingSource.DataSource = _trabajadores.ListaTrabajadores;
            listaTrabajadoresBindingSource.ResetBindings(false);
        }
        #endregion

        private void Trabajadores_Load(object sender, EventArgs e)
        {
            toolStripCancelar.Visible = false;
            listaTrabajadoresBindingNavigatorSaveItem.Enabled = false;
            DesabilitarEdicion();
        }
    }
}
