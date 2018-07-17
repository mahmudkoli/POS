using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace POSManagementProject.Report
{
    public class DefaultReport
    {
        public Paragraph ReportHeader()
        {
            string reportHeader = "Exception Handlers";
            string reportHeaderAddress = "Dhaka, Bangladesh";

            BaseFont bfnHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntHead = new Font(bfnHead, 18, 1, BaseColor.BLACK);
            Font fntAddress = new Font(bfnHead, 10, 1, BaseColor.BLACK);

            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk(reportHeader, fntHead));
            prgHeading.Add(new Chunk("\n", fntHead));
            prgHeading.Add(new Chunk(reportHeaderAddress, fntAddress));
            prgHeading.Add(new Chunk("\n", fntHead));
            prgHeading.Add(new Chunk(new LineSeparator(2.0f, 100.0f, BaseColor.GRAY, Element.ALIGN_LEFT, 1)));
            prgHeading.Add(new Chunk("\n\n", fntHead));

            return prgHeading;
        }
    }
}