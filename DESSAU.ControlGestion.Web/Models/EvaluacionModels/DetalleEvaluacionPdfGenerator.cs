using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DESSAU.ControlGestion.Web.Models.EvaluacionModels
{
    public class DetalleEvaluacionPdfGenerator
    {
        private Evaluacion _evaluacion { get; set; }

        public DetalleEvaluacionPdfGenerator(Evaluacion evaluacion)
        {
            _evaluacion = evaluacion;
        }



        public byte[] getDetalleEvaluacionPdf(DESSAUControlGestionDataContext db, Image imagenLogo) {
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            using (Document document = new Document(PageSize.LETTER, 30f, 30f, 40f, 30f))
            using (PdfWriter writer = PdfWriter.GetInstance(document, ms))
            {
                LogoDessauEncabezadoPdfEventHandler e = new LogoDessauEncabezadoPdfEventHandler()
                {
                    ImageHeader = imagenLogo,
                    Titulo = "Evaluación de desempeño"
                };
                writer.PageEvent = e;
                document.Open();
                generarDetalleEvaluacionPdf(document, db);
                document.Close();
                buffer = ms.ToArray();
                ms.Close();
            }
            return buffer;
        }


        private Font Titulo = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);
        private Font Normal = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.DARK_GRAY);
        private Font Cabecera = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.DARK_GRAY);
        public void generarDetalleEvaluacionPdf(Document doc, DESSAUControlGestionDataContext db)
        {
            //            I.- IDENTIFICACIÓN TRABAJADOR EVALUADO
            //NOMBRE: YANITZA ARAYA
            //SERVICIO: Control Documental
            //DIRECCION: DFE
            //CONTRATO:	Servicio de apoyo para proyectos especiales GPRO
            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));
            doc.Add(new Paragraph("I.- Identificación de trabajador evaluado", Titulo));
            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));
            PdfPTable identificacion = new PdfPTable(new float[] { 1, 5 }) {  WidthPercentage=100f};
            identificacion.AddCell(getCabeceraCell("Nombre"));
            identificacion.AddCell(getNormalCell(_evaluacion.UsuarioCategoriaProyecto.Usuario.NombreCompleto));
            identificacion.AddCell(getCabeceraCell("Sevicio"));
            identificacion.AddCell(getNormalCell(_evaluacion.UsuarioCategoriaProyecto.Categoria.Nombre));
            identificacion.AddCell(getCabeceraCell("Dirección"));
            identificacion.AddCell(getNormalCell(_evaluacion.UsuarioCategoriaProyecto.Proyecto.Nombre));
            identificacion.AddCell(getCabeceraCell("Contrato"));
            identificacion.AddCell(getNormalCell(_evaluacion.UsuarioCategoriaProyecto.Proyecto.Contrato.Nombre));
            identificacion.AddCell(getCabeceraCell("Período de evaluación"));
            identificacion.AddCell(getNormalCell(String.Format("{0}-{1}", _evaluacion.FechaEvaluacion.Month, _evaluacion.FechaEvaluacion.Year)));
            doc.Add(identificacion);
        }

        private PdfPCell getCabeceraCell(string frase) {
            return new PdfPCell(new Phrase(frase, Cabecera))
            {
                VerticalAlignment = Rectangle.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                Border = Rectangle.NO_BORDER
            };
        }

        private PdfPCell getNormalCell(string frase)
        {
            return new PdfPCell(new Phrase(frase, Normal))
            {
                VerticalAlignment = Rectangle.ALIGN_MIDDLE,
                BorderColor = BaseColor.LIGHT_GRAY
            };
        }
    }
}