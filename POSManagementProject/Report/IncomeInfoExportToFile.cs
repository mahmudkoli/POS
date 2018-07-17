using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using POSManagementProject.Models.ViewModels;

namespace POSManagementProject.Report
{
    public class IncomeInfoExportToFile
    {
        FileSaveLocation fileSaveLocation = new FileSaveLocation();
        DefaultReport defaultReport = new DefaultReport();
        double totalPurchaseAmount = 0;
        double totalSalesAmount = 0;
        double totalExpenseAmount = 0;

        public void ExportAllInfoToPdf(IEnumerable<IncomePurchaseReport> incomePurchaseInfoCollection, IEnumerable<IncomeSalesReport> incomeSalesInfoCollection, IEnumerable<IncomeExpenseReport> incomeExpenseInfoCollection, string branchAddress, DateTime fromDate, DateTime toDate)
        {
            List<IncomePurchaseReport> incomePurchaseInfoList = incomePurchaseInfoCollection.ToList();
            List<IncomeSalesReport> incomeSalesInfoList = incomeSalesInfoCollection.ToList();
            List<IncomeExpenseReport> incomeExpenseInfoList = incomeExpenseInfoCollection.ToList();

            Document document;
            string fileSavePath = fileSaveLocation.IncomeReportPdfLocation;
            string fileName = "IncomeReport_" + branchAddress + "_" +
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
            prgInfo.Add((new Chunk(branchAddress, fntInfoSmall)));
            prgInfo.Add((new Chunk("\nFrom Date: ", fntInfo)));
            prgInfo.Add((new Chunk(fromDate.ToString("D"), fntInfoSmall)));
            prgInfo.Add((new Chunk("\nTo Date: ", fntInfo)));
            prgInfo.Add((new Chunk(toDate.ToString("D"), fntInfoSmall)));
            document.Add(prgInfo);
            
            // Break
            document.Add(new Chunk("\n", fntInfo));

            BaseFont bfnTable = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTable = new Font(bfnTable, 11, 1, BaseColor.BLACK);

            Paragraph prgTablePur = new Paragraph();
            prgTablePur.Alignment = Element.ALIGN_LEFT;
            prgTablePur.Add((new Chunk("\nPurchase Report\n", fntTable)));
            document.Add(prgTablePur);
            document.Add(incomePurchaseTable(incomePurchaseInfoList));

            Paragraph prgTableSal = new Paragraph();
            prgTableSal.Alignment = Element.ALIGN_LEFT;
            prgTableSal.Add((new Chunk("\nSales Report\n", fntTable)));
            document.Add(prgTableSal);
            document.Add(incomeSalesTable(incomeSalesInfoList));

            Paragraph prgTableExp = new Paragraph();
            prgTableExp.Alignment = Element.ALIGN_LEFT;
            prgTableExp.Add((new Chunk("\nExpense Report\n", fntTable)));
            document.Add(prgTableExp);
            document.Add(incomeExpenseTable(incomeExpenseInfoList));

            Paragraph prgTablePro = new Paragraph();
            prgTablePro.Alignment = Element.ALIGN_LEFT;
            prgTablePro.Add((new Chunk("\nSummary\n", fntTable)));
            document.Add(prgTablePro);
            document.Add(summaryTable());


            document.Close();
            pdfWriter.Close();
            fileStream.Close();

        }

        public PdfPTable incomePurchaseTable(List<IncomePurchaseReport> incomePurchaseInfoList)
        {
            // Table Start
            PdfPTable table = new PdfPTable(4);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.TotalWidth = 500f;
            table.SetTotalWidth(new float[] { 50f, 150f, 100f, 200f });

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

            PdfPCell cellCount = new PdfPCell();
            cellCount.AddElement(new Chunk("Purchase Count", fntTableHeader));
            cellCount.Border = 0;
            table.AddCell(cellCount);

            PdfPCell cellLineTotal = new PdfPCell();
            cellLineTotal.AddElement(new Chunk("Purchase Amount", fntTableHeader));
            cellLineTotal.Border = 0;
            table.AddCell(cellLineTotal);

            // Table Data
            int sl = 1;
            BaseFont bfnTableData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableData = new Font(bfnTableData, 8, 1, BaseColor.BLACK);

            totalPurchaseAmount = 0;
            foreach (var item in incomePurchaseInfoList)
            {
                PdfPCell cellItemSl = new PdfPCell();
                cellItemSl.AddElement(new Chunk((sl++).ToString(), fntTableData));
                cellItemSl.Border = 0;
                table.AddCell(cellItemSl);

                PdfPCell cellItemDate = new PdfPCell();
                cellItemDate.AddElement(new Chunk(item.PurchaseDate.ToString("d"), fntTableData));
                cellItemDate.Border = 0;
                table.AddCell(cellItemDate);

                PdfPCell cellItemCount = new PdfPCell();
                cellItemCount.AddElement(new Chunk(item.PurchaseCount.ToString(), fntTableData));
                cellItemCount.Border = 0;
                table.AddCell(cellItemCount);

                PdfPCell cellItemLineTotal = new PdfPCell();
                cellItemLineTotal.AddElement(new Chunk(item.PurchaseTotalAmount.ToString("N"), fntTableData));
                cellItemLineTotal.Border = 0;
                table.AddCell(cellItemLineTotal);

                totalPurchaseAmount += item.PurchaseTotalAmount;
            }

            PdfPCell borderBottom = new PdfPCell();
            borderBottom.AddElement(new Chunk("\n", fntTableData));
            borderBottom.Colspan = 4;
            borderBottom.Border = PdfPCell.BOTTOM_BORDER;
            table.AddCell(borderBottom);

            // Total Amount
            PdfPCell nullCell = new PdfPCell();
            nullCell.Colspan = 2;
            nullCell.Border = 0;
            table.AddCell(nullCell);

            PdfPCell cellTotal = new PdfPCell();
            cellTotal.AddElement(new Chunk("Total Amount", fntTableHeader));
            cellTotal.Border = 0;
            table.AddCell(cellTotal);

            PdfPCell cellTotalValue = new PdfPCell();
            cellTotalValue.AddElement(new Chunk(totalPurchaseAmount.ToString("N"), fntTableHeader));
            cellTotalValue.Border = 0;
            table.AddCell(cellTotalValue);

            return table;
        }

        public PdfPTable incomeSalesTable(List<IncomeSalesReport> incomeSalesInfoList)
        {
            // Table Start
            PdfPTable table = new PdfPTable(4);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.TotalWidth = 500f;
            table.SetTotalWidth(new float[] { 50f, 150f, 100f, 200f });

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

            PdfPCell cellCount = new PdfPCell();
            cellCount.AddElement(new Chunk("Sales Count", fntTableHeader));
            cellCount.Border = 0;
            table.AddCell(cellCount);

            PdfPCell cellLineTotal = new PdfPCell();
            cellLineTotal.AddElement(new Chunk("Sales Amount", fntTableHeader));
            cellLineTotal.Border = 0;
            table.AddCell(cellLineTotal);

            // Table Data
            int sl = 1;
            BaseFont bfnTableData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableData = new Font(bfnTableData, 8, 1, BaseColor.BLACK);

            totalSalesAmount = 0;
            foreach (var item in incomeSalesInfoList)
            {
                PdfPCell cellItemSl = new PdfPCell();
                cellItemSl.AddElement(new Chunk((sl++).ToString(), fntTableData));
                cellItemSl.Border = 0;
                table.AddCell(cellItemSl);

                PdfPCell cellItemDate = new PdfPCell();
                cellItemDate.AddElement(new Chunk(item.SalesDate.ToString("d"), fntTableData));
                cellItemDate.Border = 0;
                table.AddCell(cellItemDate);

                PdfPCell cellItemCount = new PdfPCell();
                cellItemCount.AddElement(new Chunk(item.SalesCount.ToString(), fntTableData));
                cellItemCount.Border = 0;
                table.AddCell(cellItemCount);

                PdfPCell cellItemLineTotal = new PdfPCell();
                cellItemLineTotal.AddElement(new Chunk(item.SalesTotalAmount.ToString("N"), fntTableData));
                cellItemLineTotal.Border = 0;
                table.AddCell(cellItemLineTotal);

                totalSalesAmount += item.SalesTotalAmount;
            }

            PdfPCell borderBottom = new PdfPCell();
            borderBottom.AddElement(new Chunk("\n", fntTableData));
            borderBottom.Colspan = 4;
            borderBottom.Border = PdfPCell.BOTTOM_BORDER;
            table.AddCell(borderBottom);

            // Total Amount
            PdfPCell nullCell = new PdfPCell();
            nullCell.Colspan = 2;
            nullCell.Border = 0;
            table.AddCell(nullCell);

            PdfPCell cellTotal = new PdfPCell();
            cellTotal.AddElement(new Chunk("Total Amount", fntTableHeader));
            cellTotal.Border = 0;
            table.AddCell(cellTotal);

            PdfPCell cellTotalValue = new PdfPCell();
            cellTotalValue.AddElement(new Chunk(totalSalesAmount.ToString("N"), fntTableHeader));
            cellTotalValue.Border = 0;
            table.AddCell(cellTotalValue);

            return table;
        }

        public PdfPTable incomeExpenseTable(List<IncomeExpenseReport> incomeExpenseInfoList)
        {
            // Table Start
            PdfPTable table = new PdfPTable(4);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.TotalWidth = 500f;
            table.SetTotalWidth(new float[] { 50f, 150f, 100f, 200f });

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

            PdfPCell cellCount = new PdfPCell();
            cellCount.AddElement(new Chunk("Expense Count", fntTableHeader));
            cellCount.Border = 0;
            table.AddCell(cellCount);

            PdfPCell cellLineTotal = new PdfPCell();
            cellLineTotal.AddElement(new Chunk("Expense Amount", fntTableHeader));
            cellLineTotal.Border = 0;
            table.AddCell(cellLineTotal);

            // Table Data
            int sl = 1;
            BaseFont bfnTableData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableData = new Font(bfnTableData, 8, 1, BaseColor.BLACK);

            totalExpenseAmount = 0;
            foreach (var item in incomeExpenseInfoList)
            {
                PdfPCell cellItemSl = new PdfPCell();
                cellItemSl.AddElement(new Chunk((sl++).ToString(), fntTableData));
                cellItemSl.Border = 0;
                table.AddCell(cellItemSl);

                PdfPCell cellItemDate = new PdfPCell();
                cellItemDate.AddElement(new Chunk(item.ExpenseDate.ToString("d"), fntTableData));
                cellItemDate.Border = 0;
                table.AddCell(cellItemDate);

                PdfPCell cellItemCount = new PdfPCell();
                cellItemCount.AddElement(new Chunk(item.ExpenseCount.ToString(), fntTableData));
                cellItemCount.Border = 0;
                table.AddCell(cellItemCount);

                PdfPCell cellItemLineTotal = new PdfPCell();
                cellItemLineTotal.AddElement(new Chunk(item.ExpenseTotalAmount.ToString("N"), fntTableData));
                cellItemLineTotal.Border = 0;
                table.AddCell(cellItemLineTotal);

                totalExpenseAmount += item.ExpenseTotalAmount;
            }

            PdfPCell borderBottom = new PdfPCell();
            borderBottom.AddElement(new Chunk("\n", fntTableData));
            borderBottom.Colspan = 4;
            borderBottom.Border = PdfPCell.BOTTOM_BORDER;
            table.AddCell(borderBottom);

            // Total Amount
            PdfPCell nullCell = new PdfPCell();
            nullCell.Colspan = 2;
            nullCell.Border = 0;
            table.AddCell(nullCell);

            PdfPCell cellTotal = new PdfPCell();
            cellTotal.AddElement(new Chunk("Total Amount", fntTableHeader));
            cellTotal.Border = 0;
            table.AddCell(cellTotal);

            PdfPCell cellTotalValue = new PdfPCell();
            cellTotalValue.AddElement(new Chunk(totalExpenseAmount.ToString("N"), fntTableHeader));
            cellTotalValue.Border = 0;
            table.AddCell(cellTotalValue);

            return table;
        }

        public PdfPTable summaryTable()
        {
            // Table Start
            PdfPTable table = new PdfPTable(2);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.TotalWidth = 500f;
            table.SetTotalWidth(new float[] { 200f, 300f });

            // Table Header
            BaseFont bfnTableHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableHeader = new Font(bfnTableHeader, 10, 1, BaseColor.BLACK);

            // Table Data
            BaseFont bfnTableData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTableData = new Font(bfnTableData, 8, 1, BaseColor.BLACK);

            PdfPCell cellPurchase = new PdfPCell();
            cellPurchase.AddElement(new Chunk("Purchase Amount", fntTableHeader));
            cellPurchase.Border = 0;
            table.AddCell(cellPurchase);

            PdfPCell cellPurchaseValue = new PdfPCell();
            cellPurchaseValue.AddElement(new Chunk(totalPurchaseAmount.ToString("N"), fntTableData));
            cellPurchaseValue.Border = 0;
            table.AddCell(cellPurchaseValue);

            PdfPCell cellSales = new PdfPCell();
            cellSales.AddElement(new Chunk("Sales Amount", fntTableHeader));
            cellSales.Border = 0;
            table.AddCell(cellSales);

            PdfPCell cellSalesValue = new PdfPCell();
            cellSalesValue.AddElement(new Chunk(totalSalesAmount.ToString("N"), fntTableData));
            cellSalesValue.Border = 0;
            table.AddCell(cellSalesValue);

            PdfPCell cellExpense = new PdfPCell();
            cellExpense.AddElement(new Chunk("Expense Amount", fntTableHeader));
            cellExpense.Border = 0;
            table.AddCell(cellExpense);

            PdfPCell cellExpenseValue = new PdfPCell();
            cellExpenseValue.AddElement(new Chunk(totalExpenseAmount.ToString("N"), fntTableData));
            cellExpenseValue.Border = 0;
            table.AddCell(cellExpenseValue);

            PdfPCell borderBottom = new PdfPCell();
            borderBottom.AddElement(new Chunk("\n", fntTableData));
            borderBottom.Colspan = 2;
            borderBottom.Border = PdfPCell.BOTTOM_BORDER;
            table.AddCell(borderBottom);

            PdfPCell cellProfit = new PdfPCell();
            cellProfit.AddElement(new Chunk("Total Profit", fntTableHeader));
            cellProfit.Border = 0;
            table.AddCell(cellProfit);

            double totalProfit = totalSalesAmount - (totalPurchaseAmount + totalExpenseAmount);

            PdfPCell cellProfitValue = new PdfPCell();
            cellProfitValue.AddElement(new Chunk(totalProfit.ToString("N"), fntTableData));
            cellProfitValue.Border = 0;
            table.AddCell(cellProfitValue);

            return table;
        }

    }
}