using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ReportCardGenerator
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string studentName = NameTextBox.Text;
            string regNo = RegNoTextBox.Text;
            string semester = SemesterTextBox.Text;
            string branch = BranchTextBox.Text;
            string remarks = Remarks.Text;

            string[] subjectCodes = new[] { SubjectCode1.Text, SubjectCode2.Text, SubjectCode3.Text, SubjectCode4.Text, SubjectCode5.Text, SubjectCode6.Text };
            string[] grades = new[] { Grade1.Text, Grade2.Text, Grade3.Text, Grade4.Text, Grade5.Text, Grade6.Text };

            string[] labCodes = new[] { LabCode1.Text, LabCode2.Text, LabCode3.Text, LabCode4.Text, LabCode5.Text, LabCode6.Text };
            string[] labGrades = new[] { LabGrade1.Text, LabGrade2.Text, LabGrade3.Text, LabGrade4.Text, LabGrade5.Text, LabGrade6.Text };

            PdfReportGenerator.GenerateReportCard(studentName, regNo, semester, branch, subjectCodes, grades, labCodes, labGrades, remarks);
        }



    }



    public class PdfReportGenerator
    {
        public static void GenerateReportCard(string studentName, string regNo, string semester, string branch,
                                              string[] subjectCodes, string[] grades, string[] labCodes, string[] labGrades, string remarks)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Student Report Card";

            // Create an empty page
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Define fonts
            XFont fontTitle = new XFont("Times New Roman", 20);
            XFont fontSection = new XFont("Times New Roman", 14);
            XFont fontText = new XFont("Times New Roman", 12);

            // Define gold pen for the border
            XPen goldPen = new XPen(XColor.FromArgb(255, 212, 175), 5); // RGB color for gold with thicker width

            // Draw a gold border around the page
            gfx.DrawRectangle(goldPen, 10, 10, page.Width - 20, page.Height - 20);

            // Add the college logo
            

            // Draw the title
            gfx.DrawString("Student Report Card", fontTitle, XBrushes.Black,
                           new XRect(0, 100, page.Width, 40), XStringFormats.Center);

            // Draw border and student details
            gfx.DrawString("Student Details", fontSection, XBrushes.Black, 40, 230);
            gfx.DrawString($"Name: {studentName}", fontText, XBrushes.Black, 40, 260);
            gfx.DrawString($"Registration No: {regNo}", fontText, XBrushes.Black, 40, 280);
            gfx.DrawString($"Semester: {semester}", fontText, XBrushes.Black, 40, 300);
            gfx.DrawString($"Branch: {branch}", fontText, XBrushes.Black, 40, 320);

            // Draw border and lecture section
            gfx.DrawString("Lecture", fontSection, XBrushes.Black, 40, 360);
            int yOffset = 390;
            for (int i = 0; i < subjectCodes.Length; i++)
            {
                gfx.DrawString($"Subject Code: {subjectCodes[i]}", fontText, XBrushes.Black, 40, yOffset);
                gfx.DrawString($"Grade: {grades[i]}", fontText, XBrushes.Black, 300, yOffset);
                yOffset += 20;
            }

            // Draw border and labs section
            gfx.DrawString("Labs", fontSection, XBrushes.Black, 40, yOffset + 40);
            yOffset += 70;
            for (int i = 0; i < labCodes.Length; i++)
            {
                gfx.DrawString($"Lab Code: {labCodes[i]}", fontText, XBrushes.Black, 40, yOffset);
                gfx.DrawString($"Grade: {labGrades[i]}", fontText, XBrushes.Black, 300, yOffset);
                yOffset += 20;
            }

            gfx.DrawString($"Remarks: {remarks}", fontSection, XBrushes.Black, 40, yOffset + 40);



            // Save the document
            try
            {
                string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"ReportCard_{studentName.Replace(" ", "_")}.pdf");
                document.Save(filename);

                // Optional: Open the file automatically
                Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine($"An error occurred while saving the PDF: {ex.Message}");
            }
        }
    }
}

