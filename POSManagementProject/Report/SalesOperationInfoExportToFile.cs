using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using POSManagementProject.Models.EntityModels;
using POSManagementProject.Models.ViewModels;
using Font = iTextSharp.text.Font;

namespace POSManagementProject.Report
{
    public class SalesOperationInfoExportToFile
    { 
        FileSaveLocation fileSaveLocation = new FileSaveLocation();
        DefaultReport defaultReport = new DefaultReport();
        public void ExportOneInfoToPdf(SalesOperationInformation salesInfo)
        {
            Document document;
            string fileSavePath = fileSaveLocation.SalesReportPdfLocation;
            string fileName = "SalesReport"+salesInfo.SalesNo+".pdf";

            FileStream fileStream = new FileStream(fileSavePath + fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            document = new Document(PageSize.A4, 70f, 70f, 50f, 50f);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, fileStream);

            document.Open();

            document.Add(defaultReport.ReportHeader());

            // Report Info
            BaseFont bfnInfo = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntInfo = new Font(bfnInfo, 10, 1, BaseColor.BLACK);
            Font fntInfoSmall = new Font(bfnInfo, 8, 1, BaseColor.BLACK);
            Paragraph prgInfo = new Paragraph();
            prgInfo.Alignment = Element.ALIGN_LEFT;
            prgInfo.Add((new Chunk("Sales No: ", fntInfo)));
            prgInfo.Add((new Chunk(salesInfo.SalesNo, fntInfoSmall)));
            prgInfo.Add((new Chunk("\nBranch: ", fntInfo)));
            prgInfo.Add((new Chunk(salesInfo.Branch.Address, fntInfoSmall)));
            prgInfo.Add((new Chunk("\nSold By: ", fntInfo)));
            prgInfo.Add((new Chunk(salesInfo.Employee.Name, fntInfoSmall)));
            prgInfo.Add((new Chunk("\nCustomer Name: ",fntInfo)));
            prgInfo.Add((new Chunk(salesInfo.CustomerName,fntInfoSmall)));
            prgInfo.Add((new Chunk("\nCustomer Contact: ", fntInfo)));
            prgInfo.Add((new Chunk(salesInfo.CustomerContact, fntInfoSmall)));
            prgInfo.Add((new Chunk("\nDate: ", fntInfo)));
            prgInfo.Add((new Chunk(salesInfo.SalesDate.ToString("D"), fntInfoSmall)));
            document.Add(prgInfo);
            
            // Break
            document.Add(new Chunk("\n", fntInfo));

            // Table Start
            PdfPTable table = new PdfPTable(5);
            table.HorizontalAlignment = Element.ALIGN_LEFT;

            // Table Header
            BaseFont bfnTableHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableHeader = new Font(bfnTableHeader, 10, 1, BaseColor.BLACK);

            PdfPCell cellSl = new PdfPCell();
            cellSl.AddElement(new Chunk("SL", fntTableHeader));
            cellSl.Border = 0;
            table.AddCell(cellSl);

            PdfPCell cellItem = new PdfPCell();
            cellItem.AddElement(new Chunk("Item", fntTableHeader));
            cellItem.Border = 0;
            table.AddCell(cellItem);

            PdfPCell cellQty = new PdfPCell();
            cellQty.AddElement(new Chunk("Qty", fntTableHeader));
            cellQty.Border = 0;
            table.AddCell(cellQty);

            PdfPCell cellPrice = new PdfPCell();
            cellPrice.AddElement(new Chunk("Price", fntTableHeader));
            cellPrice.Border = 0;
            table.AddCell(cellPrice);

            PdfPCell cellLineTotal = new PdfPCell();
            cellLineTotal.AddElement(new Chunk("Total", fntTableHeader));
            cellLineTotal.Border = 0;
            table.AddCell(cellLineTotal);

            // Table Data
            int sl = 1;
            BaseFont bfnTableData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableData = new Font(bfnTableData, 8, 1, BaseColor.BLACK);

            foreach (var item in salesInfo.SalesItems)
            {
                PdfPCell cellItemSl = new PdfPCell();
                cellItemSl.AddElement(new Chunk((sl++).ToString(), fntTableData));
                cellItemSl.Border = 0;
                table.AddCell(cellItemSl);

                PdfPCell cellItemItem = new PdfPCell();
                cellItemItem.AddElement(new Chunk(item.Item.Name.ToString(), fntTableData));
                cellItemItem.Border = 0;
                table.AddCell(cellItemItem);

                PdfPCell cellItemQty = new PdfPCell();
                cellItemQty.AddElement(new Chunk(item.Quantity.ToString(), fntTableData));
                cellItemQty.Border = 0;
                table.AddCell(cellItemQty);

                PdfPCell cellItemUnitPrice = new PdfPCell();
                cellItemUnitPrice.AddElement(new Chunk(item.UnitPrice.ToString(), fntTableData));
                cellItemUnitPrice.Border = 0;
                table.AddCell(cellItemUnitPrice);

                PdfPCell cellItemLineTotal = new PdfPCell();
                cellItemLineTotal.AddElement(new Chunk(item.LineTotal.ToString(), fntTableData));
                cellItemLineTotal.Border = 0;
                table.AddCell(cellItemLineTotal);

            }

            PdfPCell borderBottom = new PdfPCell();
            borderBottom.AddElement(new Chunk("\n", fntTableData));
            borderBottom.Colspan = 5;
            borderBottom.Border = PdfPCell.BOTTOM_BORDER;
            table.AddCell(borderBottom);

            // Total Amount
            PdfPCell nullCell = new PdfPCell();
            nullCell.Colspan = 3;
            nullCell.Border = 0;
            table.AddCell(nullCell);

            PdfPCell cellTotal = new PdfPCell();
            cellTotal.AddElement(new Chunk("Total Amount", fntTableHeader));
            cellTotal.Border = 0;
            table.AddCell(cellTotal);

            PdfPCell cellTotalValue = new PdfPCell();
            cellTotalValue.AddElement(new Chunk(salesInfo.TotalAmount.ToString(), fntTableHeader));
            cellTotalValue.Border = 0;
            table.AddCell(cellTotalValue);

            // Discount Amount
            table.AddCell(nullCell);

            PdfPCell cellDiscount = new PdfPCell();
            cellDiscount.AddElement(new Chunk("Discount", fntTableHeader));
            cellDiscount.Border = 0;
            table.AddCell(cellDiscount);

            PdfPCell cellDicountValue = new PdfPCell();
            cellDicountValue.AddElement(new Chunk(salesInfo.DiscountAmount.ToString(), fntTableHeader));
            cellDicountValue.Border = 0;
            table.AddCell(cellDicountValue);

            // Sub Total Amount
            table.AddCell(nullCell);

            PdfPCell cellSubTotal = new PdfPCell();
            cellSubTotal.AddElement(new Chunk("Sub Total", fntTableHeader));
            cellSubTotal.Border = 0;
            table.AddCell(cellSubTotal);

            double subTotalAmount = (double)(salesInfo.TotalAmount - salesInfo.DiscountAmount);
            PdfPCell cellSubTotalValue = new PdfPCell();
            cellSubTotalValue.AddElement(new Chunk(subTotalAmount.ToString(), fntTableHeader));
            cellSubTotalValue.Border = 0;
            table.AddCell(cellSubTotalValue);

            // VAT
            table.AddCell(nullCell);

            PdfPCell cellVat = new PdfPCell();
            cellVat.AddElement(new Chunk("VAT ("+salesInfo.VAT+"%)", fntTableHeader));
            cellVat.Border = 0;
            table.AddCell(cellVat);

            double vatAmount = (double)(subTotalAmount * (salesInfo.VAT / 100));
            PdfPCell cellVatValue = new PdfPCell();
            cellVatValue.AddElement(new Chunk(vatAmount.ToString(), fntTableHeader));
            cellVatValue.Border = 0;
            table.AddCell(cellVatValue);

            // Payable Amount
            table.AddCell(nullCell);

            PdfPCell cellPayable = new PdfPCell();
            cellPayable.AddElement(new Chunk("Payable Amount", fntTableHeader));
            cellPayable.Border = 0;
            table.AddCell(cellPayable);

            PdfPCell cellPayableValue = new PdfPCell();
            cellPayableValue.AddElement(new Chunk(salesInfo.PayableAmount.ToString(), fntTableHeader));
            cellPayableValue.Border = 0;
            table.AddCell(cellPayableValue);

            document.Add(table);
            document.Close();
            pdfWriter.Close();
            fileStream.Close();
        }

        public void ExportAllInfoToPdf(IEnumerable<SalesOperationInformation> salesInfoCollection, DateTime fromDate, DateTime toDate)
        {
            List<SalesOperationInformation> salesInfoList = salesInfoCollection.ToList();
            Document document;
            string fileSavePath = fileSaveLocation.SalesReportPdfLocation;
            string fileName = "SalesReport_" + salesInfoList[0].Branch.Address + "_" +
                              fromDate.ToString("dd'.'MM'.'yyyy") + "_" +
                              toDate.ToString("dd'.'MM'.'yyyy") + ".pdf";

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
            prgInfo.Add((new Chunk(salesInfoList[0].Branch.Address, fntInfoSmall)));
            prgInfo.Add((new Chunk("\nFrom Date: ", fntInfo)));
            prgInfo.Add((new Chunk(fromDate.ToString("D"), fntInfoSmall)));
            prgInfo.Add((new Chunk("\nTo Date: ", fntInfo)));
            prgInfo.Add((new Chunk(toDate.ToString("D"), fntInfoSmall)));
            document.Add(prgInfo);
            
            // Break
            document.Add(new Chunk("\n", fntInfo));

            // Table Start
            PdfPTable table = new PdfPTable(6);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.TotalWidth = 1000f;
            table.SetTotalWidth(new float[] { 50f, 120f, 180f, 180f, 180f, 220f });

            // Table Header
            BaseFont bfnTableHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableHeader = new Font(bfnTableHeader, 10, 1, BaseColor.BLACK);

            PdfPCell cellSl = new PdfPCell();
            cellSl.AddElement(new Chunk("SL", fntTableHeader));
            cellSl.Border = 0;
            table.AddCell(cellSl);

            PdfPCell cellDate = new PdfPCell();
            cellDate.AddElement(new Chunk("Date", fntTableHeader));
            cellDate.Border = 0;
            table.AddCell(cellDate);

            PdfPCell cellBranch = new PdfPCell();
            cellBranch.AddElement(new Chunk("Branch", fntTableHeader));
            cellBranch.Border = 0;
            table.AddCell(cellBranch);

            PdfPCell cellEmployee = new PdfPCell();
            cellEmployee.AddElement(new Chunk("Employee", fntTableHeader));
            cellEmployee.Border = 0;
            table.AddCell(cellEmployee);

            PdfPCell cellSupplier = new PdfPCell();
            cellSupplier.AddElement(new Chunk("Customer", fntTableHeader));
            cellSupplier.Border = 0;
            table.AddCell(cellSupplier);

            PdfPCell cellLineTotal = new PdfPCell();
            cellLineTotal.AddElement(new Chunk("Sales Total", fntTableHeader));
            cellLineTotal.Border = 0;
            table.AddCell(cellLineTotal);

            // Table Data
            int sl = 1;
            BaseFont bfnTableData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableData = new Font(bfnTableData, 8, 1, BaseColor.BLACK);

            double TotalAmount = 0;
            foreach (var item in salesInfoList)
            {
                PdfPCell cellItemSl = new PdfPCell();
                cellItemSl.AddElement(new Chunk((sl++).ToString(), fntTableData));
                cellItemSl.Border = 0;
                table.AddCell(cellItemSl);

                PdfPCell cellItemDate = new PdfPCell();
                cellItemDate.AddElement(new Chunk(item.SalesDate.ToString("d"), fntTableData));
                cellItemDate.Border = 0;
                table.AddCell(cellItemDate);

                PdfPCell cellItemBranch = new PdfPCell();
                cellItemBranch.AddElement(new Chunk(item.Branch.Address.ToString(), fntTableData));
                cellItemBranch.Border = 0;
                table.AddCell(cellItemBranch);

                PdfPCell cellItemEmployee = new PdfPCell();
                cellItemEmployee.AddElement(new Chunk(item.Employee.Name.ToString(), fntTableData));
                cellItemEmployee.Border = 0;
                table.AddCell(cellItemEmployee);

                PdfPCell cellItemSupplier = new PdfPCell();
                cellItemSupplier.AddElement(new Chunk(item.CustomerName.ToString(), fntTableData));
                cellItemSupplier.Border = 0;
                table.AddCell(cellItemSupplier);

                PdfPCell cellItemLineTotal = new PdfPCell();
                cellItemLineTotal.AddElement(new Chunk(item.TotalAmount.ToString(), fntTableData));
                cellItemLineTotal.Border = 0;
                table.AddCell(cellItemLineTotal);

                TotalAmount += item.TotalAmount;
            }

            PdfPCell borderBottom = new PdfPCell();
            borderBottom.AddElement(new Chunk("\n", fntTableData));
            borderBottom.Colspan = 6;
            borderBottom.Border = PdfPCell.BOTTOM_BORDER;
            table.AddCell(borderBottom);

            // Total Amount
            PdfPCell nullCell = new PdfPCell();
            nullCell.Colspan = 4;
            nullCell.Border = 0;
            table.AddCell(nullCell);

            PdfPCell cellTotal = new PdfPCell();
            cellTotal.AddElement(new Chunk("Total Amount", fntTableHeader));
            cellTotal.Border = 0;
            table.AddCell(cellTotal);

            PdfPCell cellTotalValue = new PdfPCell();
            cellTotalValue.AddElement(new Chunk(TotalAmount.ToString(), fntTableHeader));
            cellTotalValue.Border = 0;
            table.AddCell(cellTotalValue);

            document.Add(table);
            document.Close();
            pdfWriter.Close();
            fileStream.Close();
        }
    }
}
