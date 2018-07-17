using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using POSManagementProject.Models.EntityModels;

namespace POSManagementProject.Report
{
    public class StockInfoExportToFile
    {
        FileSaveLocation fileSaveLocation = new FileSaveLocation();
        DefaultReport defaultReport = new DefaultReport();

        public void ExportAllInfoToPdf(IEnumerable<Stock> stockInfoCollection)
        {
            List<Stock> stockInfoList = stockInfoCollection.ToList();
            Document document;
            string fileSavePath = fileSaveLocation.StockReportPdfLocation;
            string fileName = "StockReport_" + stockInfoList[0].Branch.Address + "_" + DateTime.Now.ToString("dd'.'MM'.'yyyy") + ".pdf";

            FileStream fileStream = new FileStream(fileSavePath + fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            document = new Document(PageSize.A4, 50f, 50f, 30f, 30f);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, fileStream);

            document.Open();

            document.Add(defaultReport.ReportHeader());

            // Report Info
            BaseFont bfnInfo = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntInfo = new Font(bfnInfo, 10, 1, BaseColor.BLACK);
            Font fntInfoSmall = new Font(bfnInfo, 8, 1, BaseColor.BLACK);
            Paragraph prgInfo = new Paragraph();
            prgInfo.Alignment = Element.ALIGN_LEFT;
            prgInfo.Add((new Chunk("Branch: ", fntInfo)));
            prgInfo.Add((new Chunk(stockInfoList[0].Branch.Address, fntInfoSmall)));
            prgInfo.Add((new Chunk("\nDate: ", fntInfo)));
            prgInfo.Add((new Chunk(DateTime.Now.ToString("D"), fntInfoSmall)));
            document.Add(prgInfo);
            
            // Break
            document.Add(new Chunk("\n", fntInfo));

            // Table Start
            PdfPTable table = new PdfPTable(4);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.TotalWidth = 700f;
            table.SetTotalWidth(new float[] { 50f, 200f, 150f, 250f });

            // Table Header
            BaseFont bfnTableHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableHeader = new Font(bfnTableHeader, 10, 1, BaseColor.BLACK);

            PdfPCell cellSl = new PdfPCell();
            cellSl.AddElement(new Chunk("SL", fntTableHeader));
            cellSl.Border = 0;
            table.AddCell(cellSl);

            PdfPCell cellItem = new PdfPCell();
            cellItem.AddElement(new Chunk("Item Name", fntTableHeader));
            cellItem.Border = 0;
            table.AddCell(cellItem);

            PdfPCell cellStockQty = new PdfPCell();
            cellStockQty.AddElement(new Chunk("Stock Quantity", fntTableHeader));
            cellStockQty.Border = 0;
            table.AddCell(cellStockQty);

            PdfPCell cellAvgPrice = new PdfPCell();
            cellAvgPrice.AddElement(new Chunk("Average Price", fntTableHeader));
            cellAvgPrice.Border = 0;
            table.AddCell(cellAvgPrice);

            // Table Data
            int sl = 1;
            BaseFont bfnTableData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableData = new Font(bfnTableData, 8, 1, BaseColor.BLACK);
            
            foreach (var item in stockInfoList)
            {
                PdfPCell cellItemSl = new PdfPCell();
                cellItemSl.AddElement(new Chunk((sl++).ToString(), fntTableData));
                cellItemSl.Border = 0;
                table.AddCell(cellItemSl);

                PdfPCell cellItemName = new PdfPCell();
                cellItemName.AddElement(new Chunk(item.Item.Name, fntTableData));
                cellItemName.Border = 0;
                table.AddCell(cellItemName);

                PdfPCell cellItemStkQty = new PdfPCell();
                cellItemStkQty.AddElement(new Chunk(item.StockQuantity.ToString(), fntTableData));
                cellItemStkQty.Border = 0;
                table.AddCell(cellItemStkQty);

                PdfPCell cellItemAvgPrice = new PdfPCell();
                cellItemAvgPrice.AddElement(new Chunk(item.AveragePrice.ToString("N"), fntTableData));
                cellItemAvgPrice.Border = 0;
                table.AddCell(cellItemAvgPrice);
            }

            PdfPCell borderBottom = new PdfPCell();
            borderBottom.AddElement(new Chunk("\n", fntTableData));
            borderBottom.Colspan = 4;
            borderBottom.Border = PdfPCell.BOTTOM_BORDER;
            table.AddCell(borderBottom);
            

            document.Add(table);
            document.Close();
            pdfWriter.Close();
            fileStream.Close();
        }
    }
}