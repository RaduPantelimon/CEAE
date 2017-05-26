using System.IO;
using System.Linq;
using CEAE.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
// ReSharper disable PossiblyMistakenUseOfParamsMethod

namespace CEAE.Utils
{
    public static class ExcelReportGenerator
    {
        public static byte[] GenerateExcelReportForContacts(CEAEDBEntities db)
        {
            #region GenerateDocument

            var memoryStream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookpart = document.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                //add Style
                var stylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                var sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());

                uint sheetID = 1;


                CreateContactsSheet(document, sheets, db, sheetID);
                sheetID++;
                CreateUsersSheet(document, sheets, db, sheetID);
                sheetID++;
                CreateTestResultSheet(document, sheets, db, sheetID);
                //save data

                document.Close();
            }

            var result = memoryStream.ToArray();

            #endregion

            return result;
        }


        private static void CreateContactsSheet(SpreadsheetDocument document, OpenXmlElement sheets, CEAEDBEntities db, uint sheetID = 1)
        {
            var newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();

            var relationshipId = document.WorkbookPart.GetIdOfPart(newWorksheetPart);
            var sheet = new Sheet
            {
                Id = relationshipId,
                SheetId = sheetID,
                Name = "Anonymous Users Contacts"
            };
            sheets.AppendChild(sheet);

            var sheetData = new SheetData();


            newWorksheetPart.Worksheet = new Worksheet(sheetData);

            //create header rows

            var projectRow = CreateHeaderRow(1);
            sheetData.AppendChild(projectRow);

            //creating content rows
            var colIndex = 2;

            var contacts = db.Contacts.ToList();

            foreach (var contact in contacts)
            {
                //creating row for this user
                var row = CreateContactRow(colIndex, contact);
                sheetData.AppendChild(row);

                colIndex++;
            }


            //set col width
            var columns = new Columns();
            columns.Append(new Column {Min = 1, Max = 250, Width = 20, CustomWidth = true});
            columns.Append(new Column {Min = 2, Max = 250, Width = 12, CustomWidth = true});
            newWorksheetPart.Worksheet.Append(columns);

            newWorksheetPart.Worksheet.Save();
        }

        private static void CreateTestResultSheet(SpreadsheetDocument document, OpenXmlElement sheets, CEAEDBEntities db, uint sheetID = 1)
        {
            var newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();

            var relationshipId = document.WorkbookPart.GetIdOfPart(newWorksheetPart);
            var sheet = new Sheet
            {
                Id = relationshipId,
                SheetId = sheetID,
                Name = "Test Results Statistics"
            };
            sheets.AppendChild(sheet);

            var sheetData = new SheetData();


            newWorksheetPart.Worksheet = new Worksheet(sheetData);

            //create header rows

            var projectRow = CreateResultsHeaderRow(1);
            sheetData.AppendChild(projectRow);

            //creating content rows
            var colIndex = 2;

            var testresults = db.TestResults.ToList();

            foreach (var tst in testresults)
            {
                //creating row for this user
                var row = CreateTestResultRow(colIndex, tst);
                sheetData.AppendChild(row);

                colIndex++;
            }


            //set col width
            var columns = new Columns();
            columns.Append(new Column {Min = 1, Max = 250, Width = 20, CustomWidth = true});
            columns.Append(new Column {Min = 1, Max = 250, Width = 20, CustomWidth = true});
            columns.Append(new Column {Min = 1, Max = 175, Width = 20, CustomWidth = true});
            columns.Append(new Column {Min = 1, Max = 250, Width = 20, CustomWidth = true});

            newWorksheetPart.Worksheet.Append(columns);

            newWorksheetPart.Worksheet.Save();
        }


        private static void CreateUsersSheet(SpreadsheetDocument document, OpenXmlElement sheets, CEAEDBEntities db, uint sheetID = 1)
        {
            var newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();

            var relationshipId = document.WorkbookPart.GetIdOfPart(newWorksheetPart);
            var sheet = new Sheet
            {
                Id = relationshipId,
                SheetId = sheetID,
                Name = "Registered Users Contacts"
            };
            sheets.AppendChild(sheet);

            var sheetData = new SheetData();


            newWorksheetPart.Worksheet = new Worksheet(sheetData);

            //create header rows

            var projectRow = CreateUserHeaderRow(1);
            sheetData.AppendChild(projectRow);

            //creating content rows
            var colIndex = 2;

            var users = db.Users.ToList();

            foreach (var user in users)
            {
                //creating row for this user
                var row = CreateUserRow(colIndex, user);
                sheetData.AppendChild(row);

                colIndex++;
            }


            //set col width
            var columns = new Columns();
            columns.Append(new Column {Min = 1, Max = 250, Width = 20, CustomWidth = true});
            columns.Append(new Column {Min = 2, Max = 250, Width = 12, CustomWidth = true});
            newWorksheetPart.Worksheet.Append(columns);

            newWorksheetPart.Worksheet.Save();
        }

        private static Row CreateHeaderRow(int rowIndex)
        {
            var index = -1;
            var headerRow = new Row {RowIndex = (uint) rowIndex};

            var firstcell = CreateCell("Email Address", ref index, rowIndex, 2);
            headerRow.AppendChild(firstcell);


            var totalcell = CreateCell("Date", ref index, rowIndex, 2);
            headerRow.AppendChild(totalcell);

            return headerRow;
        }

        private static Row CreateResultsHeaderRow(int rowIndex)
        {
            var index = -1;
            var headerRow = new Row {RowIndex = (uint) rowIndex};

            var firstcell = CreateCell("Email Address", ref index, rowIndex, 2);
            headerRow.AppendChild(firstcell);

            var secondCell = CreateCell("Authenticated?", ref index, rowIndex, 2);
            headerRow.AppendChild(secondCell);

            var thirdCell = CreateCell("Correct Answers", ref index, rowIndex, 2);
            headerRow.AppendChild(thirdCell);

            var lastCell = CreateCell("Date", ref index, rowIndex, 2);
            headerRow.AppendChild(lastCell);

            return headerRow;
        }

        private static Row CreateUserHeaderRow(int rowIndex)
        {
            var index = -1;
            var headerRow = new Row {RowIndex = (uint) rowIndex};

            var firstcell = CreateCell("Account", ref index, rowIndex, 3);
            headerRow.AppendChild(firstcell);

            var totalcell = CreateCell("Email Address", ref index, rowIndex, 3);
            headerRow.AppendChild(totalcell);

            var phoneNumber = CreateCell("Phone Number", ref index, rowIndex, 3);
            headerRow.AppendChild(phoneNumber);

            return headerRow;
        }

        private static Row CreateContactRow(int colIndex, Contact contact)
        {
            // New Row
            var row = new Row {RowIndex = (uint) colIndex};
            var rowIndex = -1;


            var titleCell = CreateCell(contact.Email, ref rowIndex, colIndex);
            row.AppendChild(titleCell);

            var dateCell = CreateCell($"{contact.SignInDate:d/M/yyyy}", ref rowIndex, colIndex);
            row.AppendChild(dateCell);

            return row;
        }

        private static Row CreateTestResultRow(int colIndex, TestResult tst)
        {
            // New Row
            var row = new Row {RowIndex = (uint) colIndex};
            var rowIndex = -1;


            var participantName = "(no name)";
            var authenticated = "No";
            if (!string.IsNullOrEmpty(tst.User?.Email))
            {
                participantName = tst.User.Email;
                authenticated = "Yes";
            }
            else if (!string.IsNullOrEmpty(tst.Contact?.Email))
            {
                participantName = tst.Contact.Email;
            }


            var titleCell = CreateCell(participantName, ref rowIndex, colIndex);
            row.AppendChild(titleCell);

            var authenticatedCell = CreateCell(authenticated, ref rowIndex, colIndex);
            row.AppendChild(authenticatedCell);

            var statusCell = CreateCell(tst.Status, ref rowIndex, colIndex);
            row.AppendChild(statusCell);

            var dateCell = CreateCell($"{tst.Date:d/M/yyyy}", ref rowIndex, colIndex);
            row.AppendChild(dateCell);

            return row;
        }

        private static Row CreateUserRow(int colIndex, User user)
        {
            // New Row
            var row = new Row {RowIndex = (uint) colIndex};
            var rowIndex = -1;


            var userNameCell = CreateCell(user.Account, ref rowIndex, colIndex);
            row.AppendChild(userNameCell);

            var titleCell = CreateCell(user.Email, ref rowIndex, colIndex);
            row.AppendChild(titleCell);

            var phoneNumberCell = CreateCell(user.PhoneNumber, ref rowIndex, colIndex);
            row.AppendChild(phoneNumberCell);

            return row;
        }

        private static Cell CreateCell(string value, ref int index, int colIndex, uint styleIndex = 0)
        {
            var cell = new Cell
            {
                DataType = CellValues.InlineString,
                CellReference = ColumnLetter(++index) + colIndex,
                StyleIndex = styleIndex
            };
            // Column A1, 2, 3 ... and so on
            // Create Text object
            var t = new Text {Text = value};

            // Append Text to InlineString object
            var inlineString = new InlineString();
            inlineString.AppendChild(t);

            // Append InlineString to Cell
            cell.AppendChild(inlineString);
            return cell;
        }

        private static string ColumnLetter(int intCol)
        {
            var intFirstLetter = intCol / 676 + 64;
            var intSecondLetter = intCol % 676 / 26 + 64;
            var intThirdLetter = intCol % 26 + 65;

            var firstLetter = intFirstLetter > 64
                ? (char) intFirstLetter
                : ' ';
            var secondLetter = intSecondLetter > 64
                ? (char) intSecondLetter
                : ' ';
            var thirdLetter = (char) intThirdLetter;

            return string.Concat(firstLetter, secondLetter,
                thirdLetter).Trim();
        }


        private static Stylesheet GenerateStylesheet()
        {
            var fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize {Val = 10}
                ),
                new Font( // Index 1 - header
                    new FontSize {Val = 10},
                    new Bold(),
                    new Color {Rgb = "FFFFFF"}
                ),
                new Font(
                    new FontSize {Val = 10},
                    new Bold(),
                    new Color {Rgb = "FFFFFF"}
                ),
                new Font(
                    new FontSize {Val = 16},
                    new Bold(),
                    new Color {Rgb = "FFFFFF"}
                ));

            var fills = new Fills(
                new Fill(new PatternFill {PatternType = PatternValues.None}), // Index 0 - default
                new Fill(new PatternFill {PatternType = PatternValues.Gray125}), // Index 1 - default
                new Fill(new PatternFill(new ForegroundColor {Rgb = new HexBinaryValue {Value = "66666666"}})
                    {PatternType = PatternValues.Solid}), // Index 2 - header
                new Fill(new PatternFill(new ForegroundColor {Rgb = new HexBinaryValue {Value = "428BCA"}})
                    {PatternType = PatternValues.Solid}), //projects
                new Fill(new PatternFill(new ForegroundColor {Rgb = new HexBinaryValue {Value = "DDDDDD"}})
                    {PatternType = PatternValues.Solid}), //weekend Days
                new Fill(new PatternFill(new ForegroundColor {Rgb = new HexBinaryValue {Value = "47C9AF"}})
                    {PatternType = PatternValues.Solid}) //sprints
            );

            var borders = new Borders(
                new Border(), // index 0 default
                new Border( // index 1 black border
                    new LeftBorder(new Color {Auto = true}) {Style = BorderStyleValues.Thin},
                    new RightBorder(new Color {Auto = true}) {Style = BorderStyleValues.Thin},
                    new TopBorder(new Color {Auto = true}) {Style = BorderStyleValues.Thin},
                    new BottomBorder(new Color {Auto = true}) {Style = BorderStyleValues.Thin},
                    new DiagonalBorder())
            );

            var cellFormats = new CellFormats(
                new CellFormat(), // default
                new CellFormat {FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, ApplyFont = true}, // body
                new CellFormat
                {
                    Alignment = new Alignment
                    {
                        Horizontal = HorizontalAlignmentValues.Center,
                        Vertical = VerticalAlignmentValues.Center
                    },
                    FontId = 1,
                    FillId = 2,
                    BorderId = 1,
                    ApplyFill = true,
                    ApplyFont = true,
                    ApplyAlignment = true
                }, // employees / dates
                new CellFormat
                {
                    Alignment = new Alignment
                    {
                        Horizontal = HorizontalAlignmentValues.Center,
                        Vertical = VerticalAlignmentValues.Center
                    },
                    FontId = 1,
                    FillId = 5,
                    BorderId = 1,
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyFont = true,
                    ApplyAlignment = true
                }, // sprints
                new CellFormat {FontId = 3, FillId = 3, BorderId = 1, ApplyFill = true, ApplyFont = true}, // project
                new CellFormat
                {
                    FontId = 0,
                    FillId = 4,
                    BorderId = 1,
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyFont = true
                } // weekend
            );

            var styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }
    }
}