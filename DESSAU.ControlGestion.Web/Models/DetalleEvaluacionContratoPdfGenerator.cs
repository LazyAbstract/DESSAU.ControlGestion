using DESSAU.ControlGestion.Core;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models
{
    public class DetalleEvaluacionContratoPdfGenerator
    {
        private EvaluacionContrato _evaluacion { get; set; }

        public DetalleEvaluacionContratoPdfGenerator(EvaluacionContrato evaluacion)
        {
            _evaluacion = evaluacion;
        }

        private Font Titulo = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);
        private Font Normal = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.DARK_GRAY);
        private Font NormalNegrita = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.DARK_GRAY);
        private Font NormalSubrayado = FontFactory.GetFont("Arial", 12, Font.UNDERLINE, BaseColor.BLACK);
        private Font Cabecera = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.WHITE);

        private PdfPCell getCabeceraCell(string frase)
        {
            return new PdfPCell(new Phrase(frase, Cabecera))
            {
                VerticalAlignment = Rectangle.ALIGN_MIDDLE,
                BackgroundColor = new BaseColor(0, 32, 96),
                Border = Rectangle.TOP_BORDER,
                BorderColor = BaseColor.WHITE,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            };

        }

        private PdfPCell getCabeceraHorizontal(string frase)
        {
            PdfPCell buffer = getCabeceraCell(frase);
            buffer.HorizontalAlignment = Rectangle.ALIGN_LEFT;
            return buffer;
        }

        private PdfPCell getNormalCell(string frase)
        {
            return new PdfPCell(new Phrase(frase, Normal))
            {
                VerticalAlignment = Rectangle.ALIGN_MIDDLE,
                BorderColor = BaseColor.LIGHT_GRAY
            };
        }

        private PdfPCell getStripedNormalCell(string frase, int fila)
        {
            PdfPCell buffer = new PdfPCell(new Phrase(frase, Normal))
            {
                VerticalAlignment = Rectangle.ALIGN_MIDDLE,
                BorderColor = new BaseColor(221, 221, 221),
                Border = Rectangle.TOP_BORDER,
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            };
            if (fila % 2 == 0)
            {
                buffer.BackgroundColor = new BaseColor(249, 249, 249);
            }
            return buffer;
        }

        private PdfPCell getStripedNormalCellAlighnLeft(string frase, int fila)
        {
            PdfPCell buffer = new PdfPCell(new Phrase(frase, Normal))
            {
                VerticalAlignment = Rectangle.ALIGN_MIDDLE,
                BorderColor = new BaseColor(221, 221, 221),
                Border = Rectangle.TOP_BORDER,
                HorizontalAlignment = Rectangle.ALIGN_LEFT
            };
            if (fila % 2 == 0)
            {
                buffer.BackgroundColor = new BaseColor(249, 249, 249);
            }
            return buffer;
        }

        public byte[] getDetalleEvaluacionContratoPdf(DESSAUControlGestionDataContext db, Image imagenLogo, PdfReader plantilla)
        {
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            using (Document document = new Document(PageSize.LETTER, 30f, 30f, 40f, 30f))
            using (PdfWriter writer = PdfWriter.GetInstance(document, ms))
            {
                LogoDessauEncabezadoPdfEventHandler e = new LogoDessauEncabezadoPdfEventHandler()
                {
                    ImageHeader = imagenLogo,
                    Titulo = "Evaluación Contrato"
                };
                writer.PageEvent = e;
                document.Open();
                generarDetalleEvaluacionContratoPdf(document, db);
                PdfImportedPage pagina1 = writer.GetImportedPage(plantilla, 1);
                PdfImportedPage pagina2 = writer.GetImportedPage(plantilla, 2);
                PdfContentByte cb = writer.DirectContent;
                document.NewPage();
                cb.AddTemplate(pagina1, 1f, 0, 0, 1f, 0, 0);
                document.NewPage();
                cb.AddTemplate(pagina2, 1f, 0, 0, 1f, 0, 0);
                document.NewPage();
                document.Close();
                buffer = ms.ToArray();
                ms.Close();
            }
            return buffer;
        }

        public void generarDetalleEvaluacionContratoPdf(Document doc, DESSAUControlGestionDataContext db)
        {
            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));
            doc.Add(new Paragraph("I.- Identificación del Contrato Evaluado", Titulo));
            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));

            PdfPTable identificacion = new PdfPTable(new float[] { 1, 5 }) { WidthPercentage = 100f };

            identificacion.AddCell(getCabeceraHorizontal("Contrato"));
            identificacion.AddCell(getNormalCell(_evaluacion.Contrato.Nombre));

            identificacion.AddCell(getCabeceraHorizontal("Período de evaluación"));
            identificacion.AddCell(getNormalCell(String.Format("{0} de {1}", _evaluacion.FechaEvaluacion.ToString("MMMM"), _evaluacion.FechaEvaluacion.Year)));
            doc.Add(identificacion);

            //II.- EVALUACION DE DESEMPEÑO:
            //Se evalúan 8 competencias para el cargo del evaluado. 

            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));
            doc.Add(new Paragraph("II.- Evaluación de Contrato", Titulo));
            //doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));
            //doc.Add(new Paragraph(String.Format("Se evalúan {0} competencias para el contrato.", _evaluacion.EvaluacionContratoPreguntas.Count()), Normal));

            //Valoración de Comportamiento	
            //1   No Alcanza lo mínimo Requerido
            //2   Inferior a lo Esperado
            //3   Suficiente(lo esperado para el cargo)
            //4   Superior a lo Esperado
            //5   Supera en forma Excepcional lo Esperado
            //List valoracionComportamiento = new List(true);
            //valoracionComportamiento.Add("No Alcanza lo mínimo Requerido");
            //valoracionComportamiento.Add("Inferior a lo Esperado");
            //valoracionComportamiento.Add("Suficiente(lo esperado para el cargo)");
            //valoracionComportamiento.Add("Superior a lo Esperado");
            //valoracionComportamiento.Add("Supera en forma Excepcional lo Esperado");
            //doc.Add(new Paragraph("Valoración de Comportamiento:", Normal));
            //doc.Add(valoracionComportamiento);
            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));

            //HABILIDADES, DEFINICION, Total
            PdfPTable evaluacionTable = new PdfPTable(new float[] { 8, 1, 1, 1, 1, 1 }) { WidthPercentage = 100f };
            evaluacionTable.AddCell(getCabeceraCell("Habilidades"));
            //evaluacionTable.AddCell(getCabeceraCell("Definición"));
            for (int j = 1; j <= 5; j++)
            {
                evaluacionTable.AddCell(getCabeceraCell(((double)j/5).ToString("P0")));
            }
            //evaluacionTable.AddCell(getCabeceraCell("Total"));
            int i = 0;
            foreach (var evaluacionPregunta in _evaluacion.EvaluacionContratoPreguntas)
            {
                evaluacionTable.AddCell(getStripedNormalCellAlighnLeft(evaluacionPregunta.Pregunta.Habilidad, i));
                //evaluacionTable.AddCell(getStripedNormalCell(evaluacionPregunta.Pregunta.FormulacionPregunta, i));
                for (int j = 1; j <= 5; j++)
                {
                    evaluacionTable.AddCell(getStripedNormalCell(evaluacionPregunta.ValorObtenido == j ? "X" : "", i));
                }
                //evaluacionTable.AddCell(getStripedNormalCell("1", i));
                i++;
            }

            //PdfPCell total = getCabeceraCell("Total de valoraciones");
            //total.Colspan = 2;
            //evaluacionTable.AddCell(total);
            //for (int j = 1; j <= 5; j++)
            //{
            //    evaluacionTable.AddCell(
            //        getStripedNormalCell(
            //            _evaluacion.EvaluacionContratoPreguntas.Count(x => x.ValorObtenido == j).ToString("N0"),
            //            i));
            //}
            //evaluacionTable.AddCell(
            //        getStripedNormalCell(
            //            _evaluacion.EvaluacionContratoPreguntas.Count().ToString("N0"), i));
            //i++;


            PdfPCell totalPonderado = getCabeceraCell("Promedio Ponderado");
            totalPonderado.Colspan = 1;
            evaluacionTable.AddCell(totalPonderado);

            PdfPCell valorTotalPonderado = getStripedNormalCell(
                        _evaluacion.PromedioContrato.ToString("P2"), i);
            valorTotalPonderado.Colspan = 6;
            evaluacionTable.AddCell(valorTotalPonderado);
            i++;
            //PdfPCell apreciacionGlobal = getCabeceraCell("Promedio Ponderado");
            //apreciacionGlobal.Colspan = 2;

            ////  debería calcular apreciación global para evaluación de contrato
            //evaluacionTable.AddCell(apreciacionGlobal);
            //PdfPCell valorApreciacionGlobal = getStripedNormalCell(
            //            _evaluacion.ApreciacionGlobal, i);

            //valorApreciacionGlobal.Colspan = 6;
            //evaluacionTable.AddCell(valorApreciacionGlobal);
            doc.Add(evaluacionTable);

            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));
            doc.Add(new Paragraph("II.- Evaluación de Desempeño", Titulo));
            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));

            //desempeño
            PdfPTable desempenoTable = new PdfPTable(new float[] { 4, 2, 1 }) { WidthPercentage = 100f }; ;
            desempenoTable.AddCell(getCabeceraCell("ODS"));
            desempenoTable.AddCell(getCabeceraCell("Cliente"));
            desempenoTable.AddCell(getCabeceraCell("Promedio"));

            i = 0;
            foreach(var proyecto in db.Proyectos.OrderBy(x => x.IdProyecto))
            {
                desempenoTable.AddCell(getStripedNormalCellAlighnLeft(proyecto.Nombre, i));
                desempenoTable.AddCell(getStripedNormalCellAlighnLeft(proyecto.Usuario.NombreCompleto, i));
                if (proyecto.UsuarioCategoriaProyectos.SelectMany(x => x.Evaluacions.Where(y => y.FechaEvaluacion == _evaluacion.FechaEvaluacion)).Any())
                {
                    desempenoTable.AddCell(getStripedNormalCell(
                        proyecto.UsuarioCategoriaProyectos
                            .SelectMany(x => x.Evaluacions.Where(y => y.FechaEvaluacion == _evaluacion.FechaEvaluacion))
                            .Average(x => x.PromedioPorcentual).ToString("P2"), i));
                }
                else
                {
                    desempenoTable.AddCell(getStripedNormalCell("0,00%", i));
                }                    
                i++;
            }

            PdfPCell totalDesempenoPonderado = getCabeceraCell("Cumplimiento");
            totalPonderado.Colspan = 7;
            desempenoTable.AddCell(totalDesempenoPonderado);

            IEnumerable<Evaluacion> evalucionesDesempeno = db.Evaluacions.Where(x => x.FechaEvaluacion == _evaluacion.FechaEvaluacion);
            if (evalucionesDesempeno.Any())
            {
                string valor = evalucionesDesempeno.Average(x => x.PromedioPorcentual).ToString("P2");
                PdfPCell valorTotalDesempenoPonderado = getStripedNormalCell(valor, i);
                valorTotalDesempenoPonderado.Colspan = 1;
                desempenoTable.AddCell(valorTotalDesempenoPonderado);
            }
            else
            {
                PdfPCell valorTotalDesempenoPonderado = getStripedNormalCell("0,00%", i);
                valorTotalDesempenoPonderado.Colspan = 1;
                desempenoTable.AddCell(valorTotalDesempenoPonderado);
            }

            doc.Add(desempenoTable);

            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));
            doc.Add(new Paragraph("III.- Resumen Evaluación del Servicio", Titulo));
            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));

            //HABILIDADES, DEFINICION, Total
            PdfPTable resumenTable = new PdfPTable(new float[] { 4, 1 }) { WidthPercentage = 100f }; ;
            resumenTable.AddCell(getCabeceraCell("Resumen Evaluación"));
            resumenTable.AddCell(getCabeceraCell(""));
            
            i = 0;
            resumenTable.AddCell(getStripedNormalCellAlighnLeft("Promedio Evaluación directores Ctto Marco (70%)", i));
            resumenTable.AddCell(getStripedNormalCellAlighnLeft(_evaluacion.PromedioDesempeno.ToString("P2"), i));
            i++;

            resumenTable.AddCell(getStripedNormalCellAlighnLeft("PromedioEvaluación Administrador de Ctto Marco (30%)", i));
            resumenTable.AddCell(getStripedNormalCellAlighnLeft(_evaluacion.PromedioContrato.ToString("P2"), i));
            i++;

            resumenTable.AddCell(getStripedNormalCellAlighnLeft("Promedio total Evaluación Ctto Marco (ND)", i));
            resumenTable.AddCell(getStripedNormalCellAlighnLeft(_evaluacion.PromedioPorcentual.ToString("P2"), i));
            i++;

            resumenTable.AddCell(getStripedNormalCellAlighnLeft("Total Factor Desempeño (FD)", i));
            resumenTable.AddCell(getStripedNormalCellAlighnLeft(_evaluacion.FactorDesempeno.ToString("P2"), i));
            i++;

            doc.Add(resumenTable);

            doc.Add(new Paragraph("\n\n", FontFactory.GetFont("Arial", 6)));
            doc.Add(new Paragraph(String.Format("Evaluador: {0}", _evaluacion.Usuario.NombreCompleto), NormalNegrita));

           

        }
    }
}