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
    public partial class FormReportePlanilla : Form
    {
        public FormReportePlanilla()
        {
            InitializeComponent();

            var _planillaBL = new PlanillaBL();
            var bindingSource = new BindingSource();

            bindingSource.DataSource = _planillaBL.ObtenerPlanillas();

            var reporte = new ReportePlanilla();
            reporte.SetDataSource(bindingSource);

            crystalReportViewer1.ReportSource = reporte;
            crystalReportViewer1.RefreshReport();
        }
    }
}
