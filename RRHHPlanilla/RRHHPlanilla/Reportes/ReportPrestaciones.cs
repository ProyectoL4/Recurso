﻿using RRHH.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RRHHPlanilla
{
    public partial class ReportPrestaciones : Form
    {
        public ReportPrestaciones()
        {
            InitializeComponent();

            var _exTrabajadorBL = new ExTrabajadoresBL();
            var _cargoBL = new CargosBL();
            var _metododePagoBL = new MetodoPagoBL();
            var _jornadaBL = new JornadaBL();

            var bindingSource = new BindingSource();
            bindingSource.DataSource = _exTrabajadorBL.ObtenerExTrabajador();

            var bindingSource2 = new BindingSource();
            bindingSource2.DataSource = _cargoBL.ObtenerCargos();

            var bindingSource3 = new BindingSource();
            bindingSource3.DataSource = _metododePagoBL.ObtenerMetodoPagos();

            var bindingSource4 = new BindingSource();
            bindingSource4.DataSource = _jornadaBL.ObtenerJornadas();

            var reporte = new ReporteExEmpleado();
            ////reporte.SetDataSource(bindingSource);
            reporte.Database.Tables["ExTrabajador"].SetDataSource(bindingSource);
            reporte.Database.Tables["Cargo"].SetDataSource(bindingSource2);
            reporte.Database.Tables["MetodoPago"].SetDataSource(bindingSource3);
            reporte.Database.Tables["Jornada"].SetDataSource(bindingSource4);


            crystalReportViewer1.ReportSource = reporte;
            crystalReportViewer1.RefreshReport();

        }
    }
}