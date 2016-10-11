using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DESSAU.ControlGestion.Web.Models
{
    public class LogoDessauEncabezadoPdfEventHandler : PdfPageEventHelper
    {
        public Image ImageHeader { get; set; }
        public string Titulo { get; set; }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // cell height 
            float cellHeight = document.TopMargin;
            // PDF document size      
            Rectangle page = document.PageSize;
            PdfPTable head = new PdfPTable(new float[] {1,3,1});
            head.TotalWidth = page.Width-document.LeftMargin-document.RightMargin;

            PdfPCell c = new PdfPCell(ImageHeader);
            c.HorizontalAlignment = Element.ALIGN_LEFT;
            c.VerticalAlignment = Element.ALIGN_BOTTOM;
            c.FixedHeight = cellHeight;
            c.Border = PdfPCell.NO_BORDER;
            head.AddCell(c);

            c = new PdfPCell(new Phrase(Titulo, FontFactory.GetFont("Arial", 16, Font.BOLD,BaseColor.BLACK)));
            c.HorizontalAlignment = Element.ALIGN_CENTER;
            c.VerticalAlignment = Element.ALIGN_BOTTOM;
            c.FixedHeight = cellHeight;
            c.Border = PdfPCell.NO_BORDER;
            head.AddCell(c);
            
            c = new PdfPCell(new Phrase(
              DateTime.UtcNow.ToString("dd/MM/yyyy"),
              FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)
            ));
            c.Border = PdfPCell.NO_BORDER;
            c.HorizontalAlignment = Element.ALIGN_RIGHT;
            c.VerticalAlignment = Element.ALIGN_BOTTOM;
            c.FixedHeight = cellHeight;
            head.AddCell(c);

            // since the table header is implemented using a PdfPTable, we call
            // WriteSelectedRows(), which requires absolute positions!
            head.WriteSelectedRows(
              0, -1,  // first/last row; -1 flags all write all rows
              document.LeftMargin,      // left offset
              // ** bottom** yPos of the table
              page.Height - cellHeight + head.TotalHeight,
              writer.DirectContent
            );
        }
    }
}