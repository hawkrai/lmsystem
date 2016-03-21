using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADL.SCORM;
using ADL.Xml;

namespace SCORMHost
{
    using ADL.Diagnostics;
    using ADL.SCORM.RTE;

    public class Scorm
    {
        private FileInfo pManifestFileInfo;
        private UserMessageCollection pMessages;
        private Manifest pManifest;

        public List<TreeActivity> TreeActivity { get; set; }

        private void TreeActivityLoad(ItemCollection children)
        {
            foreach (Item child in children)
            {
                if (child.Children != null && child.Children.Any())
                {
                    TreeActivityLoad(child.Children);
                }
                else
                {
                    var t = child.GetReferencedResource();
                    TreeActivity.Add(new TreeActivity()
                                                       {
                                                           Name = child.Title,
														   Url = t.Href.ToString() + child.Parameters,
                                                       });
                }
            }
           
        }

        public void OpenImsManifest(FileInfo file)
        {
            try
            {
                this.pManifestFileInfo = file;

                this.LoadImsManifest(true, true);

                TreeActivity = new List<TreeActivity>();

                foreach (var organization in pManifest.Organizations)
                {
                    if (organization.Children != null && organization.Children.Any())
                    {
                        TreeActivityLoad(organization.Children);
                    }
                    
                }
            }
            catch (Exception)
            {
                
            }
           
        }

        private void CreateCourseDom()
        {
            var d = new DomDocument<Manifest>();

            ADL.SCORM.Namespaces.LoadNamespaceMappings(d);
            pMessages = new UserMessageCollection();
            if (d.Load(this.pMessages, this.pManifestFileInfo))
            {
                this.pManifest = d.DocumentElement;
            }
            else
            {   
                throw new ApplicationException("The imsmanifet that was selected could not be loaded.");
            }
        }

        private void LoadImsManifest(bool createDom, bool createData)
        {
           
            if (createDom)
            {
                this.CreateCourseDom();
            }

            //if (createData)
            //{
            //    this.CreateCourseDataManager();
            //}

            //// Fire Event indicating that the course was loaded. Course loading and succesful sequencing are two different things.
            //this.OnCourseLoadingComplete(args);

            //// Start the sequencing session.
            //this.StartSequencingSession();
        }


    }
}
