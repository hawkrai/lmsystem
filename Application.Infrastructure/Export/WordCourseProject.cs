using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using LMPlatform.Models.CP;
using Microsoft.Office.Interop.Word;
using Font = System.Drawing.Font;

namespace Application.Infrastructure.Export
{
    public static class WordCourseProject
    {
        #region Export Word document

        public static void CourseProjectToWord(string fileName, CourseProject work, HttpResponseBase response)
        {
            response.Clear();
            response.Charset = "ru-ru";
            response.HeaderEncoding = Encoding.UTF8;
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "application/vnd.ms-word";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".doc");
            CreateDoc(work, response);
            response.Flush();
            response.End();
        }

        private static void CreateDoc(CourseProject work, HttpResponseBase response)
        {
            Microsoft.Office.Interop.Word.Application app = null;
            string tempfileName = null;
            object falseValue = false;
            var missing = Type.Missing;
            object save = WdSaveOptions.wdSaveChanges;
            object original = WdOriginalFormat.wdOriginalDocumentFormat;
            try
            {
                var url = string.Format("{0}.Export.cptasklist.doc", Assembly.GetExecutingAssembly().GetName().Name);
                using (var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(url))
                {
                    object tempdot = tempfileName = SaveToTemp(templateStream);

                    app = new Microsoft.Office.Interop.Word.Application();
                    var doc = app.Documents.Open(ref tempdot, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                    if (doc == null)
                    {
                        throw new ApplicationException("Unable to open the word template! Try to add Launch and Activation permissions for Word DCOM component for current IIS user (IIS_IUSRS for example). Or set Identity to Interactive User.");
                    }

                    FillDoc(doc, work);
                    doc.Save();
                    doc.Close(ref save, ref original, ref falseValue);

                    SaveToResponse(tempfileName, response);
                }
            }
            finally
            {
                if (app != null)
                {
                    object dontSave = WdSaveOptions.wdDoNotSaveChanges;
                    app.Quit(ref dontSave, ref original, ref falseValue);
                }

                if (tempfileName != null)
                {
                    try
                    {
                        File.Delete(tempfileName);
                    }
                    catch (Exception)
                    {
                        //todo: log
                    }
                }
            }
        }

        private static void FillDoc(Document document, CourseProject work)
        {
            var adp = work.AssignedCourseProjects.Count == 1 ? work.AssignedCourseProjects.First() : null;
            var cinfo = CultureInfo.CreateSpecificCulture("ru-ru");
            var xmlData = adp == null ? CourseProjectToXml(work, cinfo) : CourseProjectToXml(adp, cinfo);

            var nodes = xmlData.SelectNodes("//YearlyWorks/@year");
            foreach (XmlNode node in nodes)
            {
                const string Bookn = "year";
                var value = node.InnerText;
                ReplaceBookmarkText(document, Bookn, value);
                break;
            }

            nodes = xmlData.SelectNodes("//YearlyWorks/item[@name]");
            foreach (XmlNode node in nodes)
            {
                var name = node.Attributes["name"];
                var line = node.Attributes["line"];
                var bookn = line != null ? string.Format("{0}_{1}", name.Value, line.Value) : name.Value;
                var value = node.InnerText;
                ReplaceBookmarkText(document, bookn, value);
            }
        }

        private static void ReplaceBookmarkText(Document doc, string bookmarkName, string text)
        {
            if (doc.Bookmarks.Exists(bookmarkName))
            {
                object name = bookmarkName;
                var range = doc.Bookmarks.get_Item(ref name).Range;

                range.Text = text;
                object newRange = range;
                doc.Bookmarks.Add(bookmarkName, ref newRange);
            }
        }

        private static string SaveToTemp(Stream stream)
        {
            var tmpfile = Path.ChangeExtension(Path.Combine(Path.GetTempPath(), Path.GetTempFileName()), "doc");
            stream.Seek(0, SeekOrigin.Begin);
            using (var writer = File.Create(tmpfile))
            {
                var bt = new byte[1024];
                var count = 0;
                while ((count = stream.Read(bt, 0, 1024)) > 0)
                {
                    writer.Write(bt, 0, count);
                }

                writer.Flush();
                writer.Close();
            }

            return tmpfile;
        }

        private static void SaveToResponse(string fileName, HttpResponseBase stream)
        {
            using (var reader = File.OpenRead(fileName))
            {
                var bt = new byte[1024];
                var count = 0;
                while ((count = reader.Read(bt, 0, 1024)) == 1024)
                {
                    stream.BinaryWrite(bt);
                }

                if (count > 0)
                {
                    var bt2 = new byte[count];
                    Array.Copy(bt, bt2, count);
                    stream.BinaryWrite(bt2);
                }
            }
        }

        #endregion

        #region Export Html view

        public static string CourseProjectToDocView(CourseProject work)
        {
            var sb = new StringBuilder();
            var cinfo = CultureInfo.CreateSpecificCulture("ru-ru");
            var doc = CourseProjectToXml(work, cinfo);
            var xslt = new XslTransform();
            var url = string.Format("{0}.Export.cptasklist.xslt", Assembly.GetExecutingAssembly().GetName().Name);
            var xsltFile = Assembly.GetExecutingAssembly().GetManifestResourceStream(url);
            xsltFile.Seek(0, SeekOrigin.Begin);
            using (var xmlr = XmlReader.Create(xsltFile))
            {
                xslt.Load(xmlr);
                using (TextWriter tw = new StringWriter(sb))
                {
                    var result = new XsltArgumentList();
                    xslt.Transform(doc, result, tw);
                }
            }

            return sb.ToString();
        }

        public static string CourseProjectToDocView(AssignedCourseProject work)
        {
            var sb = new StringBuilder();
            var cinfo = CultureInfo.CreateSpecificCulture("ru-ru");
            var doc = CourseProjectToXml(work, cinfo);
            var xslt = new XslTransform();
            var url = string.Format("{0}.Export.cptasklist.xslt", Assembly.GetExecutingAssembly().GetName().Name);
            var xsltFile = Assembly.GetExecutingAssembly().GetManifestResourceStream(url);
            xsltFile.Seek(0, SeekOrigin.Begin);
            using (var xmlr = XmlReader.Create(xsltFile))
            {
                xslt.Load(xmlr);
                using (TextWriter tw = new StringWriter(sb))
                {
                    var result = new XsltArgumentList();
                    xslt.Transform(doc, result, tw);
                }
            }

            return sb.ToString();
        }

        private static XmlDocument CourseProjectToXml(CourseProject work, CultureInfo cultureInfo)
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement("YearlyWorks");
            root.SetAttribute("DiplomProjectId", work.CourseProjectId.ToString());
            root.SetAttribute("year", string.Empty);

            var children = new List<XmlElement>();

            children.AddRange(CreateStringNodes(doc, "Theme", work.Theme, 523, 638, 5));

            var univer = doc.CreateElement("item");
            univer.SetAttribute("name", "Univer");
            univer.InnerText = work.Univer;
            children.Add(univer);

            var faculty = doc.CreateElement("item");
            faculty.SetAttribute("name", "Faculty");
            faculty.InnerText = work.Faculty;
            children.Add(faculty);

            var head = doc.CreateElement("item");
            head.SetAttribute("name", "HeadCathedra");
            head.InnerText = work.HeadCathedra;
            children.Add(head);

            children.AddRange(CreateStringNodes(doc, "InputData", work.InputData, 439, 638,8));

            children.AddRange(CreateStringNodes(doc, "RPZContent", work.RpzContent, 331, 638, 15));

            children.AddRange(CreateStringNodes(doc, "DrawMaterials", work.DrawMaterials, 403, 638, 7));

            children.AddRange(CreateStringNodes(doc, "Consultants", work.Consultants, 271, 638, 6));

            var ed = doc.CreateElement("item");
            ed.SetAttribute("name", "EndData");
            ed.InnerText = work.DateEnd.HasValue ? work.DateEnd.Value.ToString("d' 'MMMM' 'yyyy'г.'", cultureInfo.DateTimeFormat) : string.Empty;
            children.Add(ed);

            var pd = doc.CreateElement("item");
            pd.SetAttribute("name", "PublishData");
            pd.InnerText = work.DateStart.HasValue ? work.DateStart.Value.ToString("d' 'MMMM' 'yyyy'г.'", cultureInfo.DateTimeFormat) : string.Empty;
            children.Add(pd);
            children.AddRange(CreateStringNodes(doc, "Workflow", string.Empty, 638, 638, 14));

            foreach (var item in children)
            {
                root.AppendChild(item);
            }

            doc.AppendChild(root);
            return doc;
        }

        private static XmlDocument CourseProjectToXml(AssignedCourseProject awork, CultureInfo cultureInfo)
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement("YearlyWorks");
            root.SetAttribute("DiplomProjectId", awork.CourseProject.CourseProjectId.ToString());
            root.SetAttribute("year", awork.ApproveDate.HasValue ? awork.ApproveDate.Value.ToString("yyyy'г.'", cultureInfo.DateTimeFormat) : string.Empty);

            var children = new List<XmlElement>();

            children.AddRange(CreateStringNodes(doc, "Theme", awork.CourseProject.Theme, 523, 638, 5));

            var student = doc.CreateElement("item");
            student.SetAttribute("name", "Student");
            student.InnerText = string.Format("{0} {1} {2}", awork.Student.LastName, awork.Student.FirstName, awork.Student.MiddleName);
            children.Add(student);

            var group = doc.CreateElement("item");
            group.SetAttribute("name", "Group");
            group.InnerText = awork.Student.Group.Name;
            children.Add(group);

            var specialty = doc.CreateElement("item");
            specialty.SetAttribute("name", "Specialty");

            //            specialty.InnerText = awork.Student.Group.Speciality.Specialty;  TODO
            children.Add(specialty);

            var specialtyShifr = doc.CreateElement("item");
            specialtyShifr.SetAttribute("name", "SpecialtyShifr");

            //            specialtyShifr.InnerText = awork.Student.Group.Speciality.SpecialtyShifr;
            children.Add(specialtyShifr);

            var specializationShifr = doc.CreateElement("item");
            specializationShifr.SetAttribute("name", "SpecializationShifr");

            //            specializationShifr.InnerText = awork.Student.Group.Speciality.SpecializationShifr;
            children.Add(specializationShifr);

            var specialization = doc.CreateElement("item");
            specialization.SetAttribute("name", "Specialization");

            //            specialization.InnerText = awork.Student.Group.Speciality.Specialization;
            children.Add(specialization);

            var univer = doc.CreateElement("item");
            univer.SetAttribute("name", "Univer");
            univer.InnerXml = awork.CourseProject.Univer;
            children.Add(univer);

            var faculty = doc.CreateElement("item");
            faculty.SetAttribute("name", "Faculty");
            faculty.InnerText = awork.CourseProject.Faculty;
            children.Add(faculty);

            var head = doc.CreateElement("item");
            head.SetAttribute("name", "HeadCathedra");
            head.InnerText = awork.CourseProject.HeadCathedra;
            children.Add(head);

            children.AddRange(CreateStringNodes(doc, "InputData", awork.CourseProject.InputData, 439, 638, 8));

            children.AddRange(CreateStringNodes(doc, "RPZContent", awork.CourseProject.RpzContent, 331, 638, 15));

            children.AddRange(CreateStringNodes(doc, "DrawMaterials", awork.CourseProject.DrawMaterials, 403, 638, 7));

            children.AddRange(CreateStringNodes(doc, "Consultants", awork.CourseProject.Consultants, 271, 638, 6));

            var pd = doc.CreateElement("item");
            pd.SetAttribute("name", "PublishData");
            pd.InnerText = awork.CourseProject.DateStart.HasValue ? awork.CourseProject.DateStart.Value.ToString("d' 'MMMM' 'yyyy'г.'", cultureInfo.DateTimeFormat) : string.Empty;
            children.Add(pd);

            var ed = doc.CreateElement("item");
            ed.SetAttribute("name", "EndData");
            ed.InnerText = awork.CourseProject.DateEnd.HasValue ? awork.CourseProject.DateEnd.Value.ToString("d' 'MMMM' 'yyyy'г.'", cultureInfo.DateTimeFormat) : string.Empty;
            children.Add(ed);

            var percentageGraph = new StringBuilder();
            var pgs = awork.CourseProject.Subject.CoursePersentagesGraphs;
            var i = 1;
            foreach (var pg in pgs)
            {
                percentageGraph.AppendFormat(CultureInfo.CreateSpecificCulture("ru-RU"), "{3}. {0} \t{1}% \t{2:d MMMM yyyy} г.\n", pg.Name, pg.Percentage, pg.Date, i++);
            }

            children.AddRange(CreateStringNodes(doc, "Workflow", percentageGraph.ToString(), 638, 638, 14));

            foreach (var item in children)
            {
                root.AppendChild(item);
            }

            doc.AppendChild(root);
            return doc;
        }

        private static List<XmlElement> CreateStringNodes(XmlDocument document, string name, string value, double firstline, double line, int linescount)
        {
            var elements = new List<XmlElement>();
            var values = SplitText(value, firstline - 30, line - 30, linescount);
            for (var i = 0; i < values.Length; i++)
            {
                var el = document.CreateElement("item");
                el.InnerText = values[i];
                el.SetAttribute("name", name);
                el.SetAttribute("line", i.ToString());
                elements.Add(el);
            }

            return elements;
        }

        private static string[] SplitText(string value, double firstline, double line, int linescount)
        {
            var result = new string[linescount];
            var font = new Font("Times New Roman", (float)12.0, FontStyle.Italic, GraphicsUnit.Point);
            var start = 0;
            var sb = new StringBuilder(value);
            for (var i = 0; i < result.Length; i++)
            {
                var len = (int)Math.Floor(i > 0 ? line : firstline);
                result[i] = CutSubstring(sb, len, font);
                start += len;
            }

            return result;
        }

        private static string CutSubstring(StringBuilder str, int len, Font font)
        {
            if (str.Length > 0)
            {
                var part = new StringBuilder();
                var index = 0;
                var lastgoodlen = 0;
                var skip = 0;
                while (true)
                {
                    var symb = str[index++];
                    if (symb == '\n')
                    {
                        lastgoodlen = index - 1;
                        skip = 1;
                        break;
                    }

                    part.Append(symb);
                    var sz = TextRenderer.MeasureText(part.ToString(), font);
                    if (sz.Width < len)
                    {
                        if (char.IsSeparator(symb) || lastgoodlen == 0)
                        {
                            lastgoodlen = index;
                        }
                    }
                    else
                    {
                        break;
                    }

                    if (index >= str.Length)
                    {
                        lastgoodlen = str.Length;
                        break;
                    }
                }

                var result = str.ToString().Substring(0, lastgoodlen);
                str.Remove(0, lastgoodlen + skip);
                return result;
            }

            return string.Empty;
        }

        #endregion
    }
}
