﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using CEAE.Models;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;


namespace CEAE.Utils
{
    public class ExcelReportGenerator
    {
        public static byte[] GenerateExcelReport(CEAEDBEntities db)
        {
            byte[] result = new byte[0];

            #region GenerateDocument
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookpart = document.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                //add Style
                WorkbookStylesPart stylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                //var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();


                var sheets = document.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());


                CreateContactsSheet(document, workbookpart, sheets, db);



                //save data
                //worksheetPart.Worksheet.Save();
                document.Close();
            }

            result = memoryStream.ToArray();
            #endregion

            return result;
        }

        public static byte[] GenerateExcelReportForContacts(CEAEDBEntities db)
        {
            byte[] result = new byte[0];
           
            #region GenerateDocument
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookpart = document.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                //add Style
                WorkbookStylesPart stylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                var sheets = document.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());

                uint sheetID = 1;

                    
                CreateContactsSheet(document, workbookpart, sheets, db, sheetID);
                sheetID++;
                CreateUsersSheet(document, workbookpart, sheets, db, sheetID);
                sheetID++;
                CreateTestResultSheet(document, workbookpart, sheets, db, sheetID);
                //save data

                document.Close();
            }

            result = memoryStream.ToArray();
            #endregion

        return result;
    }


        private static Sheet CreateContactsSheet(SpreadsheetDocument document,
            WorkbookPart workbookpart,
            Sheets sheets,
            CEAEDBEntities db,
            uint sheetID = 1)
        {

            WorksheetPart newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();

            string relationshipId = document.WorkbookPart.GetIdOfPart(newWorksheetPart);
            var sheet = new Sheet()
            {
                Id = relationshipId,
                SheetId = sheetID,
                Name = "Anonymous Users Contacts"
            };
            sheets.AppendChild(sheet);

            var sheetData = new SheetData();



            newWorksheetPart.Worksheet = new Worksheet(sheetData);

            //create header rows

            List<string> MergeableRows = new List<string>();

            Row projectRow = CreateHeaderRow(1);
            sheetData.AppendChild(projectRow);

            //creating content rows
            int colIndex = 2;

            List<Contact> contacts = db.Contacts.ToList();

            foreach (Contact contact in contacts)
            {
                //creating row for this user
                Row row = CreateContactRow(colIndex, contact);
                sheetData.AppendChild(row);

                colIndex++;
            }


            //set col width
            Columns columns = new Columns();
            columns.Append(new Column() { Min = 1, Max = 250, Width = 20, CustomWidth = true });
            columns.Append(new Column() { Min = 2, Max = 250, Width = 12, CustomWidth = true });
            newWorksheetPart.Worksheet.Append(columns);

            newWorksheetPart.Worksheet.Save();
            return sheet;
        }

        private static Sheet CreateTestResultSheet(SpreadsheetDocument document,
          WorkbookPart workbookpart,
          Sheets sheets,
          CEAEDBEntities db,
          uint sheetID = 1)
        {

            WorksheetPart newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();

            string relationshipId = document.WorkbookPart.GetIdOfPart(newWorksheetPart);
            var sheet = new Sheet()
            {
                Id = relationshipId,
                SheetId = sheetID,
                Name = "Test Results Statistics"
            };
            sheets.AppendChild(sheet);

            var sheetData = new SheetData();



            newWorksheetPart.Worksheet = new Worksheet(sheetData);

            //create header rows

            List<string> MergeableRows = new List<string>();

            Row projectRow = CreateResultsHeaderRow(1);
            sheetData.AppendChild(projectRow);

            //creating content rows
            int colIndex = 2;

            List<TestResult> testresults = db.TestResults.ToList();

            foreach (TestResult tst in testresults)
            {
                //creating row for this user
                Row row = CreateTestResultRow(colIndex, tst);
                sheetData.AppendChild(row);

                colIndex++;
            }


            //set col width
            Columns columns = new Columns();
            columns.Append(new Column() { Min = 1, Max = 250, Width = 20, CustomWidth = true });
            columns.Append(new Column() { Min = 1, Max = 250, Width = 20, CustomWidth = true });
            columns.Append(new Column() { Min = 1, Max = 175, Width = 20, CustomWidth = true });
            columns.Append(new Column() { Min = 1, Max = 250, Width = 20, CustomWidth = true });

            newWorksheetPart.Worksheet.Append(columns);

            newWorksheetPart.Worksheet.Save();
            return sheet;
        }


        private static Sheet CreateUsersSheet(SpreadsheetDocument document,
    WorkbookPart workbookpart,
    Sheets sheets,
    CEAEDBEntities db,
    uint sheetID = 1)
        {

            WorksheetPart newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();

            string relationshipId = document.WorkbookPart.GetIdOfPart(newWorksheetPart);
            var sheet = new Sheet()
            {
                Id = relationshipId,
                SheetId = sheetID,
                Name = "Registered Users Contacts"
            };
            sheets.AppendChild(sheet);

            var sheetData = new SheetData();



            newWorksheetPart.Worksheet = new Worksheet(sheetData);

            //create header rows

            List<string> MergeableRows = new List<string>();

            Row projectRow = CreateUserHeaderRow(1);
            sheetData.AppendChild(projectRow);

            //creating content rows
            int colIndex = 2;

            List<User> users = db.Users.ToList();

            foreach (User user in users)
            {
                //creating row for this user
                Row row = CreateUserRow(colIndex, user);
                sheetData.AppendChild(row);

                colIndex++;
            }


            //set col width
            Columns columns = new Columns();
            columns.Append(new Column() { Min = 1, Max = 250, Width = 20, CustomWidth = true });
            columns.Append(new Column() { Min = 2, Max = 250, Width = 12, CustomWidth = true });
            newWorksheetPart.Worksheet.Append(columns);

            newWorksheetPart.Worksheet.Save();
            return sheet;
        }

        private static Row CreateHeaderRow( int rowIndex)
        {
            int index = -1;
            Row headerRow = new Row();
            headerRow.RowIndex = (UInt32)rowIndex;

            Cell firstcell = CreateCell("Email Address", ref index, rowIndex, 2);
            headerRow.AppendChild(firstcell);


            Cell totalcell = CreateCell("Date", ref index, rowIndex, 2);
            headerRow.AppendChild(totalcell);

            return headerRow;
        }

        private static Row CreateResultsHeaderRow(int rowIndex)
        {
            int index = -1;
            Row headerRow = new Row();
            headerRow.RowIndex = (UInt32)rowIndex;

            Cell firstcell = CreateCell("Email Address", ref index, rowIndex, 2);
            headerRow.AppendChild(firstcell);

            Cell secondCell = CreateCell("Authenticated?", ref index, rowIndex, 2);
            headerRow.AppendChild(secondCell);

            Cell thirdCell = CreateCell("Correct Answers", ref index, rowIndex, 2);
            headerRow.AppendChild(thirdCell);

            Cell lastCell = CreateCell("Date", ref index, rowIndex, 2);
            headerRow.AppendChild(lastCell);

            return headerRow;
        }

        private static Row CreateUserHeaderRow(int rowIndex)
        {
            int index = -1;
            Row headerRow = new Row();
            headerRow.RowIndex = (UInt32)rowIndex;

            Cell firstcell = CreateCell("Account", ref index, rowIndex, 3);
            headerRow.AppendChild(firstcell);

            Cell totalcell = CreateCell("Email Address", ref index, rowIndex, 3);
            headerRow.AppendChild(totalcell);

            Cell phoneNumber = CreateCell("Phone Number", ref index, rowIndex, 3);
            headerRow.AppendChild(phoneNumber);

            return headerRow;
        }

        private static Row CreateContactRow(int colIndex, Contact contact)
        {
            // New Row
            Row row = new Row();
            row.RowIndex = (UInt32)colIndex;
            int total = 0, rowIndex = -1;


            Cell titleCell = CreateCell(contact.Email, ref rowIndex, colIndex, 0);
            row.AppendChild(titleCell);

            Cell dateCell = CreateCell(String.Format("{0:d/M/yyyy}", contact.SignInDate), ref rowIndex, colIndex, 0);
            row.AppendChild(dateCell);

            //preparing the formula
            string start = ColumnLetter(1) + colIndex;


            string finish = ColumnLetter(rowIndex) + colIndex;

            return row;
        }

        private static Row CreateTestResultRow(int colIndex,TestResult tst)
        {
            // New Row
            Row row = new Row();
            row.RowIndex = (UInt32)colIndex;
            int total = 0, rowIndex = -1;


            string participantName = "(no name)";
            string authenticated = "No";
            if(tst.User != null && !String.IsNullOrEmpty(tst.User.Email))
            {
                participantName = tst.User.Email;
                authenticated = "Yes";
            }
            else if(tst.Contact != null && !String.IsNullOrEmpty(tst.Contact.Email))
            {
                participantName = tst.Contact.Email;
            }


            Cell titleCell = CreateCell(participantName, ref rowIndex, colIndex, 0);
            row.AppendChild(titleCell);

            Cell authenticatedCell = CreateCell(authenticated, ref rowIndex, colIndex, 0);
            row.AppendChild(authenticatedCell);

            Cell statusCell = CreateCell(tst.Status, ref rowIndex, colIndex, 0);
            row.AppendChild(statusCell);

            Cell dateCell = CreateCell(String.Format("{0:d/M/yyyy}", tst.Date), ref rowIndex, colIndex, 0);
            row.AppendChild(dateCell);

            //preparing the formula
            string start = ColumnLetter(1) + colIndex;

           
            string finish = ColumnLetter(rowIndex) + colIndex;

            return row;
        }

        private static Row CreateUserRow(int colIndex, User user)
        {
            // New Row
            Row row = new Row();
            row.RowIndex = (UInt32)colIndex;
            int total = 0, rowIndex = -1;




            Cell userNameCell = CreateCell( user.Account, ref rowIndex, colIndex, 0);
            row.AppendChild(userNameCell);

            Cell titleCell = CreateCell(user.Email, ref rowIndex, colIndex, 0);
            row.AppendChild(titleCell);

            Cell phoneNumberCell = CreateCell( user.PhoneNumber, ref rowIndex, colIndex, 0);
            row.AppendChild(phoneNumberCell);


            //preparing the formula
            string start = ColumnLetter(1) + colIndex;


            string finish = ColumnLetter(rowIndex) + colIndex;

            return row;
        }

        private static Cell CreateNumberContentCell(int value, ref int index, int colIndex, uint styleIndex = 0)
        {
            Cell cell = new Cell();
            cell.DataType = CellValues.Number;
            cell.StyleIndex = styleIndex;
            // Column A1, 2, 3 ... and so on
            cell.CellReference = ColumnLetter(++index) + colIndex;

            // Append InlineString to Cell
            cell.CellValue = new CellValue(value.ToString());
            return cell;
        }

        private static Cell CreateCell(string value, ref int index, int colIndex, uint styleIndex = 0)
        {
            Cell cell = new Cell();
            cell.DataType = CellValues.InlineString;
            // Column A1, 2, 3 ... and so on
            cell.CellReference = ColumnLetter(++index) + colIndex;
            cell.StyleIndex = styleIndex;
            // Create Text object
            Text t = new Text();
            t.Text = value;

            // Append Text to InlineString object
            InlineString inlineString = new InlineString();
            inlineString.AppendChild(t);

            // Append InlineString to Cell
            cell.AppendChild(inlineString);
            return cell;
        }
        private static Cell CreateFormulaCell(string Formula, string value, ref int index, int colIndex, uint styleIndex = 0)
        {
            Cell cell = new Cell() { CellReference = ColumnLetter(++index) + colIndex };
            CellFormula cellformula = new CellFormula();
            cellformula.Text = Formula;
            CellValue cellValue = new CellValue();
            cellValue.Text = value;
            cell.StyleIndex = styleIndex;
            cell.Append(cellformula);
            cell.Append(cellValue);
            return cell;
        }

        private static string ColumnLetter(int intCol)
        {
            var intFirstLetter = ((intCol) / 676) + 64;
            var intSecondLetter = ((intCol % 676) / 26) + 64;
            var intThirdLetter = (intCol % 26) + 65;

            var firstLetter = (intFirstLetter > 64)
                ? (char)intFirstLetter : ' ';
            var secondLetter = (intSecondLetter > 64)
                ? (char)intSecondLetter : ' ';
            var thirdLetter = (char)intThirdLetter;

            return string.Concat(firstLetter, secondLetter,
                thirdLetter).Trim();
        }


        private static Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
               new Font( // Index 0 - default
                   new FontSize() { Val = 10 }

               ),
               new Font( // Index 1 - header
                   new FontSize() { Val = 10 },
                   new Bold(),
                   new Color() { Rgb = "FFFFFF" }

               ),
                new Font(
                   new FontSize() { Val = 10 },
                   new Bold(),
                   new Color() { Rgb = "FFFFFF" }

               ),
                new Font(
                   new FontSize() { Val = 16 },
                   new Bold(),
                   new Color() { Rgb = "FFFFFF" }

               ));

            Fills fills = new Fills(
                     new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                     new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                     new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } })
                     { PatternType = PatternValues.Solid }), // Index 2 - header
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "428BCA" } })
                    { PatternType = PatternValues.Solid }), //projects
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "DDDDDD" } })
                    { PatternType = PatternValues.Solid }),//weekend Days
                     new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "47C9AF" } })
                     { PatternType = PatternValues.Solid })//sprints
                 );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, ApplyFont = true }, // body
                    new CellFormat
                    {
                        Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center },
                        FontId = 1,
                        FillId = 2,
                        BorderId = 1,
                        ApplyFill = true,
                        ApplyFont = true,
                        ApplyAlignment = true
                    }, // employees / dates
                    new CellFormat
                    {
                        Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center },
                        FontId = 1,
                        FillId = 5,
                        BorderId = 1,
                        ApplyFill = true,
                        ApplyBorder = true,
                        ApplyFont = true,
                        ApplyAlignment = true
                    }, // sprints
                    new CellFormat { FontId = 3, FillId = 3, BorderId = 1, ApplyFill = true, ApplyFont = true }, // project
                    new CellFormat { FontId = 0, FillId = 4, BorderId = 1, ApplyFill = true, ApplyBorder = true, ApplyFont = true }// weekend
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }



    }

}

