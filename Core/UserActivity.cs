using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testWpf.Core
{
    public class UserActivity
    {
        private static ActivityWindow _activityWindow;

        public static ActivityWindow activityWindow
        {
            get { return _activityWindow; }
            set { _activityWindow = value; }
        }

        public enum ActivityWindow
        {
            Home,
            Release
        }
    }


}
