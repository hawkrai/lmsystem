namespace SCORMHost
{
    using System.Collections.Generic;

    public class TreeActivity
    {
        public TreeActivity()
        {
            ChildActivity = new List<TreeActivity>();
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public List<TreeActivity> ChildActivity { get; set; }
    }
}