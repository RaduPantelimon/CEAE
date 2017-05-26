using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

// ReSharper disable PossiblyMistakenUseOfParamsMethod


namespace CEAE.Utils
{
    public static class TestReportGenerator
    {
        public static byte[] GenerateExcelReportForContacts()
        {
            #region GenerateDocument
            var memoryStream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                CreateParts(document);

                //save data

                document.Close();
            }

            var result = memoryStream.ToArray();
            #endregion

            return result;
        }

        // Adds child parts and generates content of the specified part
        private static void CreateParts(SpreadsheetDocument document)
        {
            var workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPart1Content(workbookPart1);

            var worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPart1Content(worksheetPart1);
        }

        // Generates content of workbookPart1. 
        private static void GenerateWorkbookPart1Content(WorkbookPart workbookPart1)
        {
            var workbook1 = new Workbook();
            workbook1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            var sheets1 = new Sheets();
            var sheet1 = new Sheet { Name = "Anonymous Users Contacts", SheetId = 1U, Id = "rId1" };
            sheets1.Append(sheet1);

            workbook1.Append(sheets1);
            workbookPart1.Workbook = workbook1;
        }

        // Generates content of worksheetPart1. 
        private static void GenerateWorksheetPart1Content(WorksheetPart worksheetPart1)
        {
            var worksheet1 = new Worksheet();
            var sheetData1 = new SheetData();

            var row1 = new Row();
            var cell1 = new Cell { CellReference = "A1", DataType = CellValues.InlineString };
            var inlineString1 = new InlineString();
            var text1 = new Text {Text = "hello"};
            inlineString1.Append(text1);
            cell1.Append(inlineString1);
            row1.Append(cell1);

            sheetData1.Append(row1);
            worksheet1.Append(sheetData1);
            worksheetPart1.Worksheet = worksheet1;
        }
    }



}

